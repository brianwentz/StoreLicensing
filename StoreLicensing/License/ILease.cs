// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace StoreLicensing.Powershell.License
{
    /// <summary>
    /// A license lease.  A lease contains policy for a Key indicating how and when it can be used.  There may be multiple
    /// leases for a given Key.
    /// </summary>
    public interface ILease
    {
        /// <summary>
        /// The internal license lease identifier.  
        /// </summary>
        string Id { get; }

        /// <summary>
        /// The package family names the lease applies to
        /// </summary>
        string PackageFamilyNames { get; }

        /// <summary>
        /// The SID of the user associated with the lease.  This property is only present for leases bound
        /// to a specific user SID.
        /// </summary>
        string SID { get; }

        /// <summary>
        /// The MSA of the user associated with the lease.  This property is only present for leases bound
        /// to a specific MSA.
        /// </summary>
        string MSA { get; }

        /// <summary>
        /// The validity of the lease.  Leases may be invalid due to expiration, corruption, change in 
        /// the binding data, etc.  The value is an HRESULT, where 0 means the lease is valid, non-0
        /// indicates the HRESULT value for the reason the lease is invalid.
        /// </summary>
        uint Validity { get; }

        /// <summary>
        /// The issue date for the lease.
        /// </summary>
        DateTime IssueDate { get; }

        /// <summary>
        /// The expiration date for the lease.
        /// </summary>
        DateTime ExpirationDate { get; }
    }
}
