// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoreLicensing.Powershell.License
{
    internal class Key : IKey
    {       
        public string Id { get; set; }

        public string PackageFamilyName { get; set; }
        public string ContentId { get; set; }
        public string ProductId { get; set; }
        public Categories Category { get; set; }

        public uint Validity { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool RequiresLease { get; set; }
        public IEnumerable<ILease> Leases { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(Globals.Resources.GetString("PackageFamilyName"), PackageFamilyName);
            sb.AppendLine();

            sb.AppendFormat(Globals.Resources.GetString("ContentId"), ContentId);
            sb.AppendLine();

            sb.AppendFormat(Globals.Resources.GetString("LicenseId"), Id);
            sb.AppendLine();

            string status = (Validity == 0) ? Globals.Resources.GetString("Valid") : String.Format(Globals.Resources.GetString("Invalid"), Validity);
            sb.AppendFormat(Globals.Resources.GetString("Validity"), status);
            sb.AppendLine();

            sb.AppendFormat(Globals.Resources.GetString("IssueDate"),
                (IssueDate.Year == 1601) ? String.Format(Globals.Resources.GetString("NoIssueDate")) : IssueDate.ToString());
            sb.AppendLine();

            sb.AppendFormat(Globals.Resources.GetString("ExpirationDate"),
                (ExpirationDate.Year == 1601) ? String.Format(Globals.Resources.GetString("NoExpiration")) : ExpirationDate.ToString());
            sb.AppendLine();

            sb.AppendFormat(Globals.Resources.GetString("RequiresLease"), RequiresLease.ToString());
            sb.AppendLine();

            sb.AppendFormat(Globals.Resources.GetString("Category"), Category.ToString());
            sb.AppendLine();

            if (Leases != null && Leases.Count() > 0)
            {
                sb.AppendLine(Globals.Resources.GetString("Leases"));

                foreach (var lease in Leases)
                {
                    sb.Append(lease.ToString());
                    sb.AppendLine();
                }
            }

            sb.AppendLine();

            return sb.ToString();
        }

    }
}
