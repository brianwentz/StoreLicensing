// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoreLicensing.Powershell;
using StoreLicensing.Powershell.Cmdlets;
using System;
using System.Collections;
using System.Collections.Generic;
using StoreLicensing.Powershell.License;

namespace StoreLicensing.UnitTests
{
    [TestClass]
    public class TestShowLicense
    {
        [TestMethod]
        public void ExistingLicense()
        {
            using (ShimsContext.Create())
            {
                // Mock calls to clip to return the license installed and the license data
                StoreLicensing.Powershell.Fakes.ShimCLiP.AllInstances.IsLicenseInstalledString = (clip, pfn) => 
                {
                    Assert.AreEqual(pfn, "pfn1");
                    return true;
                };

                MockKey mockKey = new MockKey();
                StoreLicensing.Powershell.Fakes.ShimCLiP.AllInstances.GetLicenseDetailsString = (clip, pfn) =>
                {
                    Assert.AreEqual(pfn, "pfn1");
                    return mockKey;
                };

                // Watch writes to the cmdlet output and record them.
                List<CommandRuntimeWriteEvent> writeEvents = CommonMockSetup.CommandWatcher();

                // Initialize the key to return
                mockKey.Id = "1";
                mockKey.PackageFamilyName = "MockPfn";
                mockKey.ContentId = "MockContentId";
                mockKey.Validity = 0;
                mockKey.ExpirationDate = new DateTime(0);

                // Invoke the cmdlet & ensure it succeeded.
                ShowLicenseCommand cmdlet = new ShowLicenseCommand() { PackageFamilyName = "pfn1" };
                IEnumerator result = cmdlet.Invoke().GetEnumerator();
                Assert.IsTrue(result.MoveNext());

                // Verify no warnings or errors were sent and look for the object with the key in it
                IKey returnedKey = null;
                foreach (var writeEvent in writeEvents)
                {
                    Assert.AreNotEqual(writeEvent.Kind, CommandRuntimeWriteEvent.WriteKind.Warning);
                    Assert.AreNotEqual(writeEvent.Kind, CommandRuntimeWriteEvent.WriteKind.Error);

                    if (writeEvent.Kind == CommandRuntimeWriteEvent.WriteKind.Object)
                    {
                        if (writeEvent.Data is IKey)
                        {
                            returnedKey = writeEvent.Data as IKey;
                        }
                    }                        
                }
                Assert.IsNotNull(returnedKey);
                Assert.AreEqual(mockKey, returnedKey);
            }
        }

        [TestMethod]
        public void NoLicense()
        {
            using (ShimsContext.Create())
            {
                // Mock calls to clip to return the license as not installed
                StoreLicensing.Powershell.Fakes.ShimCLiP.AllInstances.IsLicenseInstalledString = (clip, pfn) =>
                {
                    Assert.AreEqual(pfn, "pfn1");
                    return false;
                };

                // Watch writes to the cmdlet output and record them.
                List<CommandRuntimeWriteEvent> writeEvents = CommonMockSetup.CommandWatcher();

                // Invoke the cmdlet & ensure it failed.
                ShowLicenseCommand cmdlet = new ShowLicenseCommand() { PackageFamilyName = "pfn1" };
                IEnumerator result = cmdlet.Invoke().GetEnumerator();
                Assert.IsFalse(result.MoveNext());

                // Verify there is a warning about the failure
                ValidationHelpers.VerifyWarningTextIsPresent(ref writeEvents, String.Format(Globals.Resources.GetString("LicenseNotFound"), "pfn1"));
            }
        }
    }
}
