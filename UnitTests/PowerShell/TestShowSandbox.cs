// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoreLicensing.Powershell;
using StoreLicensing.Powershell.Cmdlets;
using System.Collections;
using System.Collections.Generic;

namespace StoreLicensing.UnitTests
{
    [TestClass]
    public class TestShowSandbox
    {
        [TestMethod]
        public void InSandbox()
        {
            using (ShimsContext.Create())
            {
                // Mock calls to the registry looking for the sandbox value to return no sandbox.
                CommonMockSetup.RegistryReadReturnsSandboxName("testbox.11");                
                List<CommandRuntimeWriteEvent> writeEvents = CommonMockSetup.CommandWatcher();

                // Invoke the cmdlet & ensure it succeeded.
                ShowSandboxCommand cmdlet = new ShowSandboxCommand();
                IEnumerator result = cmdlet.Invoke().GetEnumerator();
                Assert.IsTrue(result.MoveNext());

                // Verify no warnings or errors were sent and look for the object with the sandbox name
                ValidationHelpers.EnsureStringFoundAndNoWarnings(ref writeEvents, "testbox.11");                
            }
        }

        [TestMethod]
        public void NotInSandbox()
        {
            using (ShimsContext.Create())
            {
                // Mock calls to the registry looking for the sandbox value to return no sandbox.
                CommonMockSetup.RegistryReadReturnsSandboxName(null);                
                List<CommandRuntimeWriteEvent> writeEvents = CommonMockSetup.CommandWatcher();

                // Invoke the cmdlet & ensure it succeeded.
                ShowSandboxCommand cmdlet = new ShowSandboxCommand();
                IEnumerator result = cmdlet.Invoke().GetEnumerator();
                Assert.IsTrue(result.MoveNext());

                // Verify no warnings or errors were sent and look for the object with the sandbox name
                ValidationHelpers.EnsureStringFoundAndNoWarnings(ref writeEvents, Constants.NoSandbox);                
            }
        }
    }
}
