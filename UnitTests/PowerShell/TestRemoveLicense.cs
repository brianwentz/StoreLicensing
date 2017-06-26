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
    public class TestRemoveLicense
    {
        [TestMethod]
        public void RemoveLicense()
        {
            using (ShimsContext.Create())
            {
                // Mock calls to clip to return the license installed and the license data
                StoreLicensing.Powershell.Fakes.ShimCLiP.AllInstances.IsLicenseInstalledString = (clip, pfn) =>
                {
                    Assert.AreEqual(pfn, "pfn1");
                    return true;
                };

                bool licenseUninstalled = false;
                StoreLicensing.Powershell.Fakes.ShimCLiP.AllInstances.UninstallLicenseString = (clip, pfn) =>
                {
                    Assert.AreEqual(pfn, "pfn1");
                    licenseUninstalled = true;
                };

                // Watch writes to the cmdlet output and record them.
                List<CommandRuntimeWriteEvent> writeEvents = CommonMockSetup.CommandWatcher();
            
                // Invoke the cmdlet & ensure it succeeded.
                RemoveLicenseCommand cmdlet = new RemoveLicenseCommand() { PackageFamilyName = "pfn1" };
                IEnumerator result = cmdlet.Invoke().GetEnumerator();
                Assert.IsTrue(result.MoveNext());

                Assert.IsTrue(licenseUninstalled);
                ValidationHelpers.EnsureSuccessAndNoWarnings(ref writeEvents, "SuccessMessage");
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
                RemoveLicenseCommand cmdlet = new RemoveLicenseCommand() { PackageFamilyName = "pfn1" };
                IEnumerator result = cmdlet.Invoke().GetEnumerator();
                Assert.IsFalse(result.MoveNext());

                // Verify there is a warning about the failure
                ValidationHelpers.VerifyWarningTextIsPresent(ref writeEvents, String.Format(Globals.Resources.GetString("LicenseNotFound"), "pfn1"));
            }
        }

    }
}
