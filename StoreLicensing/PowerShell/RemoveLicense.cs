// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Management.Automation;

namespace StoreLicensing.Powershell.Cmdlets
{
    /// <summary>
    ///     <para type="synopsis">Removes the license for the specified product</para>
    ///     <para type="description">
    ///         The Remove-License cmdlet will completely remove the license from machine for the specified product.
    ///     </para>
    ///     <para type="link">Show-License</para>
    /// </summary>
    /// <example>
    ///     <code>Remove-License Microsoft.BingWeather_8wekyb3d8bbwe</code>
    ///     <para>This command will remove the license for MSN Weather app</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "License", SupportsShouldProcess = true)]
    public class RemoveLicenseCommand : Cmdlet
    {
        /// <summary>
        /// <para type="description">The Package Family Name of the licensed product for which the license is removed</para> 
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string PackageFamilyName { get; set; }

        /// <summary>
        /// <para type="description">Force the removal of the license without a confirmation prompt.</para> 
        /// </summary>
        [Parameter()]
        public SwitchParameter Force { get; set; }
        
        /// <summary>Process Remove-License command</summary>
        protected override void ProcessRecord()
        {
            if (Force || ShouldProcess(PackageFamilyName))
            {
                try
                {
                    CLiP clip = new CLiP();
                    if (!clip.IsLicenseInstalled(PackageFamilyName))
                    {
                        WriteWarning(string.Format(Globals.Resources.GetString("LicenseNotFound"), PackageFamilyName));
                        return;
                    }

                    clip.UninstallLicense(PackageFamilyName);
                    WriteObject(Globals.Resources.GetString("SuccessMessage"));                    
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
            }
        }
    }
}
