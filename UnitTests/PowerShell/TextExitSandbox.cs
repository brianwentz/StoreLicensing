// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoreLicensing.Powershell;
using StoreLicensing.Powershell.Cmdlets;
using System;
using System.Collections;
using System.Collections.Generic;

namespace StoreLicensing.UnitTests
{
    [TestClass]
    public class TestExitSandbox
    {
        [TestMethod]
        public void ExitSandbox()
        {                        
            using (ShimsContext.Create())
            {
                // Mock calls to the registry looking for the sandbox value to return sandbox.11
                CommonMockSetup.RegistryReadReturnsSandboxName("sandbox.11");
                object sandboxValue = null;
                CommonMockSetup.RegistrySetSandboxWatcher((object newValue) => { sandboxValue = newValue; });
                List<string> servicesRestarted = CommonMockSetup.RestartServiceWatcher();
                List<CommandRuntimeWriteEvent> writeEvents = CommonMockSetup.CommandWatcher();
                
                // Invoke the cmdlet & ensure it succeeded
                ExitSandboxCommand cmdlet = new ExitSandboxCommand();
                IEnumerator result = cmdlet.Invoke().GetEnumerator();
                Assert.IsTrue(result.MoveNext());

                Assert.IsNotNull(sandboxValue);
                Assert.AreEqual(sandboxValue.ToString(), Constants.NoSandbox);

                Assert.IsNotNull(servicesRestarted[0]);
                Assert.IsNotNull(servicesRestarted[1]);

                if (servicesRestarted[0] == "XblAuthManager")
                {
                    Assert.AreEqual(servicesRestarted[1], "DiagTrack");
                }
                else
                {
                    Assert.AreEqual(servicesRestarted[0], "DiagTrack");
                    Assert.AreEqual(servicesRestarted[1], "XblAuthManager");
                }

                ValidationHelpers.EnsureSuccessAndNoWarnings(ref writeEvents, "SuccessMessage");
            }
        }

        [TestMethod]
        public void ExitSandboxWhenNotInSandbox()
        {
            using (ShimsContext.Create())
            {
                // Mock calls to the registry looking for the sandbox value to return no sandbox
                CommonMockSetup.RegistryReadReturnsSandboxName(Constants.NoSandbox);
                object sandboxValue = null;
                CommonMockSetup.RegistrySetSandboxWatcher((object newValue) => { sandboxValue = newValue; });
                List<string> servicesRestarted = CommonMockSetup.RestartServiceWatcher();
                List<CommandRuntimeWriteEvent> writeEvents = CommonMockSetup.CommandWatcher();

                // Invoke the cmdlet & ensure it succeeded
                ExitSandboxCommand cmdlet = new ExitSandboxCommand();
                IEnumerator result = cmdlet.Invoke().GetEnumerator();
                Assert.IsTrue(result.MoveNext());

                // Verify the sandbox didn't change
                Assert.IsNull(sandboxValue);

                // Verify no services restarted
                Assert.AreEqual(0, servicesRestarted.Count);

                ValidationHelpers.EnsureSuccessAndNoWarnings(ref writeEvents, "SuccessMessage");
            }
        }

        [TestMethod]
        public void NotRunningAsAdmin()
        {
            using (ShimsContext.Create())
            {
                // Mock calls to the registry looking for the sandbox value to return a value
                CommonMockSetup.RegistryReadReturnsSandboxName("sandbox.11");
                
                // Mock calls to set the registry key for the sandbox to throw an unauthorized access exception.
                CommonMockSetup.RegistrySetSandboxWatcher((object newValue) => { throw new UnauthorizedAccessException(); });

                List<string> servicesRestarted = CommonMockSetup.RestartServiceWatcher();
                List<CommandRuntimeWriteEvent> writeEvents = CommonMockSetup.CommandWatcher();

                // Invoke the cmdlet
                ExitSandboxCommand cmdlet = new ExitSandboxCommand();
                IEnumerator result = cmdlet.Invoke().GetEnumerator();
                Assert.IsFalse(result.MoveNext());

                // Verify no services were restarted
                Assert.AreEqual(0, servicesRestarted.Count);

                // Verify a warning about not running as admin was sent
                ValidationHelpers.VerifyWarningIsPresent(ref writeEvents, "NotRunningAsAdmin");                
            }
        }        
    }
}