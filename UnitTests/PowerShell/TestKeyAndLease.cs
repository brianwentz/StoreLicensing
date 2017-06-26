// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoreLicensing.Powershell.License;
using System;
using System.Collections.Generic;

namespace StoreLicensing.UnitTests
{
    [TestClass]
    public class TestKeyAndLease
    {
        [TestMethod]
        public void LeaseToStringWorks()
        {
            // Create a lease with some reasonable data
            Lease lease = new Lease();
            lease.Id = "LeaseId";
            lease.SID = "S-1-5-21-23423423-2345345";
            lease.MSA = "storelicensing@hotmail.com";
            lease.Validity = 0;
            lease.IssueDate = new DateTime(0);
            lease.ExpirationDate = new DateTime(0);
            lease.PackageFamilyNames = "LeasePfn1;LeasePfn2";

            // Make sure it can be converted to a string
            // This is really just testing no exceptions are thrown.            
            lease.ToString();

            // Change up the data & keep trying.
            lease.Validity = 0x803f8001;
            lease.IssueDate = DateTime.Now;
            lease.ExpirationDate = DateTime.Now;
            lease.ToString();

            lease.SID = null;
            lease.MSA = null;
            lease.ToString();
        }

        [TestMethod]
        public void KeyToStringWorks()
        {
            // Create a key with some reasonable data
            Key key = new Key();
            key.Id = "KeyId";
            key.RequiresLease = false;
            key.Validity = 0;
            key.IssueDate = new DateTime(0);
            key.ExpirationDate = new DateTime(0);
            key.PackageFamilyName = "KeyPfn";
            key.ContentId = "ContentIdForKey";
            key.Category = Categories.Retail;

            // Make sure it can be converted to a string
            key.ToString();

            // Change some data and keep trying.
            key.RequiresLease = true;
            key.Validity = 0x80040005;
            key.IssueDate = DateTime.Now;
            key.ExpirationDate = DateTime.Now;
            key.ToString();

            // Add some leases now
            List<ILease> leases = new List<ILease>();
            Lease lease = new Lease();
            lease.Id = "LeaseId1";
            lease.Validity = 0;
            lease.ExpirationDate = new DateTime(0);
            lease.PackageFamilyNames = "KeyPfn;OtherKeyPfn";
            leases.Add(lease);

            lease.Id = "LeaseId2";
            leases.Add(lease);

            key.Leases = leases;
            key.ToString();            
        }
    }
}
