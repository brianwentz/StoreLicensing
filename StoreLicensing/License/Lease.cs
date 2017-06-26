// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;

namespace StoreLicensing.Powershell.License
{
    internal class Lease : ILease
    {
        public string Id { get; set; }
        public string PackageFamilyNames { get; set; }
        public string SID { get; set; }
        public string MSA { get; set; }
        public uint Validity { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();

            sb.AppendFormat(Globals.Resources.GetString("LicenseId"), Id);
            sb.AppendLine();
             
            if (!string.IsNullOrWhiteSpace(SID))
            {
                sb.AppendFormat(Globals.Resources.GetString("BoundToSID"), SID);
                sb.AppendLine();
            }

            if (!string.IsNullOrWhiteSpace(MSA))
            {
                sb.AppendFormat(Globals.Resources.GetString("BoundToMSA"), MSA);
                sb.AppendLine();
            }

            string status = (Validity == 0) ? Globals.Resources.GetString("Valid") : String.Format(Globals.Resources.GetString("Invalid"), Validity);
            sb.AppendFormat(Globals.Resources.GetString("Validity"), status);
            sb.AppendLine();

            sb.AppendFormat(Globals.Resources.GetString("IssueDate"),
                (IssueDate.Year == 1601) ? String.Format(Globals.Resources.GetString("NoIssueDate")) : IssueDate.ToString());
            sb.AppendLine();

            sb.AppendFormat(Globals.Resources.GetString("ExpirationDate"),
                (ExpirationDate.Year == 1601) ? String.Format(Globals.Resources.GetString("NoExpiration")) : ExpirationDate.ToString());
            sb.AppendLine();

            sb.AppendFormat(Globals.Resources.GetString("PackageFamilyNames"), PackageFamilyNames);
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
