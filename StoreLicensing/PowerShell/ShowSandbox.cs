// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Management.Automation;

namespace StoreLicensing.Powershell.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Displays the current sandbox</para>
    /// <para type="description">
    /// The Show-Sandbox cmdlet will display the current sandbox.
    /// If sandbox was recently changed and a machine reboot is required for operation to come into effect,
    /// this cmdlet will display the new sandbox which will be in effect after the reboot.
    /// If machine is not part of any sandbox, the default sandbox name will be displayed.
    /// </para>
    /// <para type="link">Enter-Sandbox</para>
    /// <para type="link">Exit-Sandbox</para>
    /// </summary>
    /// <example>
    ///     <code>Show-Sandbox</code>
    ///     <para>This command will display the current sandbox.</para>
    /// </example>
    [Cmdlet(VerbsCommon.Show, "Sandbox")]
    public class ShowSandboxCommand : Cmdlet
    {
        /// <summary>Process Show-Sandbox command</summary>
        protected override void ProcessRecord()
        {
            string sandbox = Constants.NoSandbox;
            try
            {                           
                // Read sandbox name from the registry
                object sandboxValue = Microsoft.Win32.Registry.GetValue(Constants.SandboxRegistryPath, Constants.SandboxRegistryName, string.Empty);
                if (sandboxValue != null)
                {
                    string sandboxString = sandboxValue.ToString();
                    if (!String.IsNullOrEmpty(sandboxString))
                    {
                        sandbox = sandboxString;
                    }
                }
            }
            catch (Exception)
            {
                // Failed to get the sandbox name, the default will be used.
            }
            WriteObject(sandbox);
        }
    }
}
