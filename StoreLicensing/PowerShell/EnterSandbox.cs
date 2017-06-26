// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Management.Automation;

namespace StoreLicensing.Powershell.Cmdlets
{
    /// <summary>
    ///     <para type="synopsis">Enter the specified sandbox</para>
    ///     <para type="description">
    ///         The Enter-Sandbox cmdlet will enter the specified sandbox and will restart all the necessary services.
    ///         If services cannot be restarted, a message indicating a machine reboot is required will be displayed.
    ///     </para>
    ///     <para type="link">Exit-Sandbox</para>
    ///     <para type="link">Show-Sandbox</para>
    /// </summary>
    /// <example>
    ///     <code>Enter-Sandbox EXAMPLE</code>
    ///     <para>Use this command to join "EXAMPLE" sandbox</para>
    /// </example>
    [Cmdlet(VerbsCommon.Enter, "Sandbox")]
    public class EnterSandboxCommand : Cmdlet
    {        
        /// <summary>
        /// <para type="description">The name of the sandbox to be joined</para> 
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string SandboxName { get; set; }
                
        /// <summary>Process Enter-Sandbox command</summary>
        protected override void ProcessRecord()
        {
            // Check if we are not in that sandbox already
            ShowSandboxCommand getCurrentSandbox = new ShowSandboxCommand();
                        
            bool alreadyInSandbox = false;
            foreach (var result in getCurrentSandbox.Invoke())
            {
                // If the machine is already in the sandbox then call it a success.
                if (SandboxName.Equals(result.ToString()))                   
                {                    
                    alreadyInSandbox = true;
                }
            }

            if (!alreadyInSandbox)
            {
                // Set registry entry
                try
                {
                    Microsoft.Win32.Registry.SetValue(Constants.SandboxRegistryPath, Constants.SandboxRegistryName, SandboxName);
                }
                catch (System.UnauthorizedAccessException)
                {
                    WriteWarning(Globals.Resources.GetString("NotRunningAsAdmin"));
                    return;
                }
                catch (System.Exception e)
                {
                    WriteWarning(string.Format(Globals.Resources.GetString("GeneralFailure"), e.ToString()));
                    return;
                }

                // Restart necessary processes
                try
                {
                    ServicesHelper.RestartService("XblAuthManager");
                    ServicesHelper.RestartService("DiagTrack");
                }
                catch
                {
                    WriteObject(Globals.Resources.GetString("SuccessWithRebootMessage"));
                    return;
                }
            }
            WriteObject(Globals.Resources.GetString("SuccessMessage"));
        }
    }
}
