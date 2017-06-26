// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using StoreLicensing.Powershell.License;
using System;
using System.Collections.Generic;

namespace StoreLicensing.UnitTests
{
    internal class MockKey : IKey
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public Categories Category { get; set; }
        public string PackageFamilyName { get; set; }
        public string ContentId { get; set; }
        public uint Validity { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool RequiresLease { get; set; }
        public IEnumerable<ILease> Leases { get; set; }
    }
}
