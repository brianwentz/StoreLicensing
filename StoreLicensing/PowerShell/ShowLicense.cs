// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Management.Automation;
using StoreLicensing.Powershell.License;

namespace StoreLicensing.Powershell.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Displays the license for the specified product</para>
    /// <para type="description">
    /// The Show-License cmdlet will display the license for the specified licensed product.
    /// If product is not licensed, an error message will be displayed.
    /// </para>
    /// <para type="link">Remove-License</para>
    /// </summary>
    /// <example>
    ///     <code>Show-License Microsoft.BingWeather_8wekyb3d8bbwe</code>
    ///     <para>This command will display license details for MSN Weather app</para>
    /// </example>
    [Cmdlet(VerbsCommon.Show, "License")]
    public class ShowLicenseCommand : Cmdlet
    {
        /// <summary>
        /// <para type="description">The Package Family Name of the licensed product</para> 
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string PackageFamilyName { get; set; }

        /// <summary>Process Show-License command</summary>
        protected override void ProcessRecord()
        {
            try
            {
                CLiP clip = new CLiP();
                if (!clip.IsLicenseInstalled(PackageFamilyName))
                {
                    WriteWarning(String.Format(Globals.Resources.GetString("LicenseNotFound"), PackageFamilyName));
                    return;
                }

                IKey key = clip.GetLicenseDetails(PackageFamilyName);
                WriteObject(key);                
            }
            catch (System.Exception ex)
            {
                WriteWarning(String.Format(Globals.Resources.GetString("UnableToGetLicense"), PackageFamilyName, ex.ToString()));                
            }
        }
    }
}
