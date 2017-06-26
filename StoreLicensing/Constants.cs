// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace StoreLicensing.Powershell
{
    /// <summary>
    /// Various constants used in the powershell cmdlet.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// The name of the default sandbox that indicates there is not a special sandbox set.
        /// </summary>
        public const string NoSandbox = "RETAIL";

        /// <summary>
        /// The path in the registry to where the current sandbox name is stored.
        /// </summary>
        public const string SandboxRegistryPath = "HKEY_LOCAL_MACHINE\\software\\microsoft\\XboxLive";

        /// <summary>
        /// The value name for the sandbox in the registry key specified by SandboxRegistryPath
        /// </summary>
        public const string SandboxRegistryName = "Sandbox";
    }
}
