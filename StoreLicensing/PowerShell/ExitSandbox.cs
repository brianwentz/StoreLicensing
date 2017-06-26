// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Management.Automation;

namespace StoreLicensing.Powershell.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Exits the current sandbox</para>
    /// <para type="description">
    /// The Exit-Sandbox cmdlet will exit the current sandbox and restart all the necessary services.
    /// If services cannot be restarted, a machine reboot will be required in order for operation to come into effect.
    /// </para>
    /// <para type="link">Enter-Sandbox</para>
    /// <para type="link">Show-Sandbox</para>
    /// </summary>
    /// <example>
    ///     <code>Exit-Sandbox</code>
    ///     <para>This command will leave the current sandbox.</para>
    /// </example>
    [Cmdlet(VerbsCommon.Exit, "Sandbox")]
    public class ExitSandboxCommand : Cmdlet
    {
        /// <summary>Process Exit-Sandbox command</summary>
        protected override void ProcessRecord()
        {
            // Enter-Sandbox RETAIL
            EnterSandboxCommand enterRetailCmd = new EnterSandboxCommand { SandboxName = Constants.NoSandbox };

            foreach (var result in enterRetailCmd.Invoke())
            {
                WriteObject(result);
            }
        }
    }
}
