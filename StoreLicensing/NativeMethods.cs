// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace StoreLicensing.Powershell
{
    internal class NativeMethods
    {
        private const string CLiPModuleFilePath = "clipc.dll";

        [DllImport(CLiPModuleFilePath, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int ClipOpen(out IntPtr hClip);

        [DllImport(CLiPModuleFilePath, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int ClipClose(IntPtr hClip);

        [DllImport(CLiPModuleFilePath, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int ClipQueryAssociateId(
            IntPtr hClip,
            [MarshalAs(UnmanagedType.LPWStr)] string query,
            out uint count,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] out Guid[] licenseIds);

        [DllImport(CLiPModuleFilePath, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int ClipGetFileIdFromAssociateId(
            IntPtr hClip,
            [MarshalAs(UnmanagedType.LPStruct)] Guid licenseAssociatedId,
            out Guid licenseFileId);

        [DllImport(CLiPModuleFilePath, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int ClipGetAssociatedResults(
            IntPtr hClip,
            [MarshalAs(UnmanagedType.LPStruct)] Guid pAssociateId,
            uint nAssociateIds,
            ulong dwFlags,
            [MarshalAs(UnmanagedType.LPStruct)] out ClipLicenseResultsArray ppLicensingResults);

        [DllImport(CLiPModuleFilePath, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int ClipUninstallLicense(
            IntPtr hClip,
            [MarshalAs(UnmanagedType.LPStruct), In, Out] Guid licenseFileId);
    }

    [StructLayout(LayoutKind.Sequential), Serializable]
    internal class ClipLicenseResult
    {
        public int DataVersion;
        public ulong DataSize;
        public Guid FileId;
        public Guid LicenseId;
        public Guid ContentId;
        public Guid value1;
        public Guid value2;
        public int value3;
        public int value4;
        public int value5;
        public int Category;
        [NonSerialized] public System.Runtime.InteropServices.ComTypes.FILETIME IssueDate;
        [NonSerialized] public System.Runtime.InteropServices.ComTypes.FILETIME ExpirationDate;
        public bool LeaseRequired;
        public int value9;
        public IntPtr ProductId;
        public IntPtr PackageFamilyName;
        public IntPtr UserId;
        public IntPtr UserSID;
        public IntPtr value12;
        public IntPtr value13;
        public IntPtr AssociatedPackageFamilyNames;
        public uint ActiveState;
    }

    [StructLayout(LayoutKind.Sequential), Serializable]
    internal class ClipLicenseResultsArray
    {
        public uint nCount;

        public ClipLicenseResult results;
    }
}

