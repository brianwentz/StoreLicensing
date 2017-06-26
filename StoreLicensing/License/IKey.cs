// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace StoreLicensing.Powershell.License
{
    /// <summary>
    /// The list of categories that a license key may belong to.
    /// </summary>
    public enum Categories
    {
        /// <summary>
        /// The category is not known.
        /// </summary>
        Unknown,

        /// <summary>
        /// The key is from the retail store.
        /// </summary>
        Retail,

        /// <summary>
        /// The key is from the enterprise/business store
        /// </summary>
        Enterprise,

        /// <summary>
        /// The key is from an OEM preinstall.
        /// </summary>
        OEM,

        /// <summary>
        /// The key is from a developer package.
        /// </summary>
        Developer,
    };

    /// <summary>
    /// A license key.  A key represents the item being licensed and should be unique and there is a 1-1 mapping between the 
    /// item and its key.  Examples of keys are applications (Microsoft Word, Google Chrome, etc).
    /// </summary>     
    public interface IKey
    {
        /// <summary>
        /// The internal license key identifier.  
        /// </summary>
        string Id { get; }

        /// <summary>
        /// The package family name for the item the key represents
        /// </summary>
        string PackageFamilyName { get; }

        /// <summary>
        /// The Content Id for the item the key represents
        /// </summary>
        string ContentId { get; }

        /// <summary>
        /// The Product Id (aka catalog id) for the item the key represents
        /// </summary>
        string ProductId { get; }

        /// <summary>
        /// The key category
        /// </summary>
        Categories Category { get; }

        /// <summary>
        /// The validity of the key.  Keys may be invalid due to expiration, corruption, change in 
        /// the binding data, etc.  The value is an HRESULT, where 0 means the key is valid, non-0
        /// indicates the HRESULT value for the reason the key is invalid.
        /// </summary>
        uint Validity { get; }

        /// <summary>
        /// The issue date for the key.
        /// </summary>
        DateTime IssueDate { get; }

        /// <summary>
        /// The expiration date for the key.
        /// </summary>
        DateTime ExpirationDate { get; }

        /// <summary>
        /// Indicates if the key requires a lease for usage.
        /// </summary>
        bool RequiresLease { get; }

        /// <summary>
        /// The leases for the key, if present.
        /// </summary>
        IEnumerable<ILease> Leases { get; }
    }
}