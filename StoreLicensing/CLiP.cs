// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using StoreLicensing.Powershell.License;

namespace StoreLicensing.Powershell
{
    /// <summary>
    /// Client Licensing Platform helper class
    /// </summary>
    internal class CLiP : IDisposable
    {       
        private IntPtr _hClip = IntPtr.Zero;
        
        public CLiP()
        {
            NativeMethods.ClipOpen(out _hClip);
        }

        ~CLiP()
        {
            InternalDispose();
        }

        public void Dispose()
        {
            InternalDispose();
            GC.SuppressFinalize(this);
        }

        protected virtual void InternalDispose()
        {
            if (_hClip != IntPtr.Zero)
            {
                NativeMethods.ClipClose(_hClip);
                _hClip = IntPtr.Zero;                
            }
        }
        /// <summary>Verify if there is a license installed for given Package Family Name</summary>
        /// <param name="packageFamilyName">The package family name</param>
        /// <returns>True if a license was found or false otherwise</returns>
        public bool IsLicenseInstalled(string packageFamilyName)
        {
            string query = CreateQueryForKeyId(packageFamilyName);
            uint count;
            Guid[] ids;

            NativeMethods.ClipQueryAssociateId(_hClip, query, out count, out ids);

            return count > 0;
        }

        /// <summary>
        /// Queries CLiP for license ids and then removes them.
        /// If no license is installed for the given Package Family Name, this function doesn't do anything and doesn't fail
        /// </summary>
        /// <param name="packageFamilyName">The product family name</param>
        public void UninstallLicense(string packageFamilyName)
        {
            string query = CreateQueryForLeaseId(packageFamilyName);
            uint count;
            Guid[] ids;
            int lastFailedResult = 0;

            NativeMethods.ClipQueryAssociateId(_hClip, query, out count, out ids);

            for (uint i = 0; i < count; ++i)
            {
                Guid fileId;
                NativeMethods.ClipGetFileIdFromAssociateId(_hClip, ids[i], out fileId);

                int result = NativeMethods.ClipUninstallLicense(_hClip, fileId);
                if (result != 0)
                {
                    lastFailedResult = result;
                }
            }
                
            if (lastFailedResult != 0)
            {
                Marshal.ThrowExceptionForHR(lastFailedResult);
            }
        }

        /// <summary>
        /// Gets the license details for the specified Package Family Name
        /// If no license is installed for the given Package Family Name, this function returns an empty list and doesn't fail.
        /// </summary>
        /// <param name="packageFamilyName">The package family name</param>
        /// <returns>An <see cref="System.Collections.Generic.IList{T}"/> of stringified licenses</returns>
        public IKey GetLicenseDetails(string packageFamilyName)
        {
            string query = CreateQueryForLeaseId(packageFamilyName);
            uint count;
            Guid[] ids;
            NativeMethods.ClipQueryAssociateId(_hClip, query, out count, out ids);

            Key key = GetKeyForPackage(packageFamilyName);
            IList<ILease> leases = new List<ILease>();
            for (uint i = 0; i < count; ++i)
            {
                // Get license data
                ClipLicenseResultsArray licenseData;
                NativeMethods.ClipGetAssociatedResults(_hClip, ids[i], 1 /*nAssociatedIds*/, 1 /*E_LICENSE_RESULT_CHECK_LICENSE*/, out licenseData);

                if (licenseData != null && licenseData.results != null)
                {
                    Lease lease = new Lease();
                    lease.Id = licenseData.results.LicenseId.ToString();                   
                    
                    string userSid = Marshal.PtrToStringUni(licenseData.results.UserSID);
                    if (!string.IsNullOrWhiteSpace(userSid))
                    {
                        lease.SID = userSid;
                    }

                    string userId = Marshal.PtrToStringUni(licenseData.results.UserId);
                    if (!string.IsNullOrWhiteSpace(userId))
                    {
                        lease.MSA = userId;
                    }

                    string packageFamilyNames = Marshal.PtrToStringUni(licenseData.results.AssociatedPackageFamilyNames);
                    if (!string.IsNullOrWhiteSpace(packageFamilyNames))
                    {
                        lease.PackageFamilyNames = packageFamilyNames;
                    }

                    lease.Validity = licenseData.results.ActiveState;
                    lease.IssueDate = System.DateTime.FromFileTimeUtc(((long)licenseData.results.IssueDate.dwHighDateTime << 32) + licenseData.results.IssueDate.dwLowDateTime);
                    lease.ExpirationDate = System.DateTime.FromFileTimeUtc(((long)licenseData.results.ExpirationDate.dwHighDateTime << 32) + licenseData.results.ExpirationDate.dwLowDateTime);

                    leases.Add(lease);
                }
                key.Leases = leases;
            }            
            return key;
        }

        private string CreateQueryForKeyId(string packageFamilyName)
        {
            return $"PFM={packageFamilyName};LicenseType=1"; 
        }
        private string CreateQueryForLeaseId(string packageFamilyName)
        {
            return $"AssociatePFM={packageFamilyName};LicenseType=3";
        }

        private Key GetKeyForPackage(string packageFamilyName)
        {
            string query = CreateQueryForKeyId(packageFamilyName);
            uint count;
            Guid[] ids;
            NativeMethods.ClipQueryAssociateId(_hClip, query, out count, out ids);

            Key key = new Key();
            if (count > 0)
            {
                ClipLicenseResultsArray licenseData;
                NativeMethods.ClipGetAssociatedResults(_hClip, ids[0], 1 /*nAssociatedIds*/, 1 /*E_LICENSE_RESULT_CHECK_LICENSE*/, out licenseData);

                if (licenseData != null && licenseData.results != null)
                {
                    key.Id = licenseData.results.LicenseId.ToString();
                    key.IssueDate = System.DateTime.FromFileTimeUtc(((long)licenseData.results.IssueDate.dwHighDateTime << 32) + licenseData.results.IssueDate.dwLowDateTime);
                    key.ExpirationDate = System.DateTime.FromFileTimeUtc(((long)licenseData.results.ExpirationDate.dwHighDateTime << 32) + licenseData.results.ExpirationDate.dwLowDateTime);
                    key.RequiresLease = licenseData.results.LeaseRequired;
                    key.Validity = licenseData.results.ActiveState;

                    string productId = Marshal.PtrToStringUni(licenseData.results.ProductId);
                    if (!string.IsNullOrWhiteSpace(productId))
                    {
                        key.ProductId = productId;
                    }

                    string packageFamilyNameFromKey = Marshal.PtrToStringUni(licenseData.results.PackageFamilyName);
                    if (!string.IsNullOrWhiteSpace(packageFamilyNameFromKey))
                    {
                        key.PackageFamilyName = packageFamilyNameFromKey;
                    }

                    key.ContentId = licenseData.results.ContentId.ToString();

                    switch (licenseData.results.Category)
                    {                     
                        case 1: key.Category = Categories.Retail; break;
                        case 2: key.Category = Categories.Enterprise; break;
                        case 3: key.Category = Categories.OEM; break;
                        case 4: key.Category = Categories.Developer; break;

                        default:
                        case 0: key.Category = Categories.Unknown; break;
                    }
                }
            }
            return key;
        }
    }
 }
