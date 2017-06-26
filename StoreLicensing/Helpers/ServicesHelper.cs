// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace StoreLicensing.Powershell
{
    /// <summary>
    /// Helper methods for dealing with services.
    /// </summary>
    internal static class ServicesHelper
    {
        private static CmdletsCaller cmdletsCaller = CmdletsCaller.GetInstance();

        /// <summary>Restarts a given service by just calling Restart-Service cmdlet</summary>
        /// <param name="serviceName">The name of the service</param>
        public static void RestartService(string serviceName)
        {
            cmdletsCaller.ExecuteCmdlet($"Restart-Service -Name {serviceName} -Force");
        }
    }
}
