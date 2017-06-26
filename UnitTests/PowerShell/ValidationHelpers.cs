// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoreLicensing.Powershell;
using System.Collections.Generic;

namespace StoreLicensing.UnitTests
{
    internal class ValidationHelpers
    {
        public static void VerifyWarningIsPresent(ref List<CommandRuntimeWriteEvent> writeEvents, string warningTextResourceName)
        {                           
            bool warningMessageFound = false;
            foreach (var writeEvent in writeEvents)
            {
                if (writeEvent.Kind == CommandRuntimeWriteEvent.WriteKind.Warning)
                {
                    warningMessageFound = (writeEvent.Data.ToString() == Globals.Resources.GetString(warningTextResourceName));
                    if (warningMessageFound)
                    {
                        break;
                    }
                }
            }
            Assert.IsTrue(warningMessageFound);
        }

        public static void VerifyWarningTextIsPresent(ref List<CommandRuntimeWriteEvent> writeEvents, string warningText)
        {
            bool warningMessageFound = false;
            foreach (var writeEvent in writeEvents)
            {
                if (writeEvent.Kind == CommandRuntimeWriteEvent.WriteKind.Warning)
                {
                    warningMessageFound = (writeEvent.Data.ToString() == warningText);
                    if (warningMessageFound)
                    {
                        break;
                    }
                }
            }
            Assert.IsTrue(warningMessageFound);
        }

        public static void EnsureSuccessAndNoWarnings(ref List<CommandRuntimeWriteEvent> writeEvents, string successTextResourceName)
        {
            // Verify no warnings or errors were sent and look for a success object
            bool successMessageFound = false;
            foreach (var writeEvent in writeEvents)
            {
                Assert.AreNotEqual(writeEvent.Kind, CommandRuntimeWriteEvent.WriteKind.Warning);
                Assert.AreNotEqual(writeEvent.Kind, CommandRuntimeWriteEvent.WriteKind.Error);

                if (!successMessageFound && (writeEvent.Kind == CommandRuntimeWriteEvent.WriteKind.Object))
                {
                    successMessageFound = (writeEvent.Data.ToString() == Globals.Resources.GetString(successTextResourceName));
                }
            }
            Assert.IsTrue(successMessageFound);
        }

        public static void EnsureStringFoundAndNoWarnings(ref List<CommandRuntimeWriteEvent> writeEvents, string stringToFind)
        {
            // Verify no warnings or errors were sent and there is an object with the given string value
            bool stringFound = false;
            foreach (var writeEvent in writeEvents)
            {
                Assert.AreNotEqual(writeEvent.Kind, CommandRuntimeWriteEvent.WriteKind.Warning);
                Assert.AreNotEqual(writeEvent.Kind, CommandRuntimeWriteEvent.WriteKind.Error);

                if (!stringFound && (writeEvent.Kind == CommandRuntimeWriteEvent.WriteKind.Object))
                {
                    stringFound = (writeEvent.Data.ToString() == stringToFind);
                }
            }
            Assert.IsTrue(stringFound);
        }
    }
}
