// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using StoreLicensing.Powershell.License;
using System;

namespace StoreLicensing.UnitTests
{
    internal class MockLease : ILease
    {
        public string Id { get; set; }
        public string PackageFamilyNames { get; set; }
        public string SID { get; set; }
        public string MSA { get; set; }
        public uint Validity { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
