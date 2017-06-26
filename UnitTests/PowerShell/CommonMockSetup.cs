// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.QualityTools.Testing.Fakes;
using System.Collections.Generic;
using StoreLicensing.Powershell;

namespace StoreLicensing.UnitTests
{
    internal class CommonMockSetup
    {
        public static List<CommandRuntimeWriteEvent> CommandWatcher()
        {
            // Watch writes to the cmdlet output and record them.
            List<CommandRuntimeWriteEvent> writeEvents = new List<CommandRuntimeWriteEvent>();
            System.Management.Automation.Fakes.ShimCmdlet.AllInstances.WriteWarningString = (cmdLet, text) =>
            {
                CommandRuntimeWriteEvent.AddWriteEvent(writeEvents, CommandRuntimeWriteEvent.WriteKind.Warning, text);
                ShimsContext.ExecuteWithoutShims(() => cmdLet.WriteWarning(text));
            };

            System.Management.Automation.Fakes.ShimCmdlet.AllInstances.WriteObjectObject = (cmdLet, data) =>
            {
                CommandRuntimeWriteEvent.AddWriteEvent(writeEvents, CommandRuntimeWriteEvent.WriteKind.Object, data);
                ShimsContext.ExecuteWithoutShims(() => cmdLet.WriteObject(data));
            };

            System.Management.Automation.Fakes.ShimCmdlet.AllInstances.WriteErrorErrorRecord = (cmdLet, error) =>
            {
                CommandRuntimeWriteEvent.AddWriteEvent(writeEvents, CommandRuntimeWriteEvent.WriteKind.Error, error);
                ShimsContext.ExecuteWithoutShims(() => cmdLet.WriteError(error));
            };

            return writeEvents;
        }

        public static List<string> RestartServiceWatcher()
        {
            List<string> servicesRestarted = new List<string>(2);

            // Watch calls to restart the services and make sure they all happen but don't really do it.
            StoreLicensing.Powershell.Fakes.ShimServicesHelper.RestartServiceString = (serviceName) =>
            {
                servicesRestarted.Add(serviceName);
            };

            return servicesRestarted;
        }

        public static void RegistryReadReturnsSandboxName(string sandboxName)
        {
            // Mock calls to the registry looking for the sandbox value to return the given sandbox name.
            Microsoft.Win32.Fakes.ShimRegistry.GetValueStringStringObject = (keyName, valueName, defaultValue) =>
            {
                object registryValue;

                if (keyName == Constants.SandboxRegistryPath &&
                    valueName == Constants.SandboxRegistryName)
                {
                    registryValue = sandboxName;
                }
                else
                {
                    registryValue = ShimsContext.ExecuteWithoutShims<object>(() =>
                    {
                        return Microsoft.Win32.Registry.GetValue(keyName, valueName, defaultValue);
                    });
                }
                return registryValue;
            };
        }

        public delegate void OnSandboxSetInRegistry(object newValue);
        public static void RegistrySetSandboxWatcher(OnSandboxSetInRegistry watcher)
        { 
            // Mock calls to set the registry key for the sandbox to inform the delegate of the value but not really set it.
            Microsoft.Win32.Fakes.ShimRegistry.SetValueStringStringObject = (keyName, valueName, value) =>
            {
                if (keyName == Constants.SandboxRegistryPath &&
                    valueName == Constants.SandboxRegistryName)
                {
                    watcher(value);
                }
                else
                {
                    ShimsContext.ExecuteWithoutShims(() =>
                    {
                        Microsoft.Win32.Registry.SetValue(keyName, valueName, value);
                    });
                }
            };            
        }
    }
}
