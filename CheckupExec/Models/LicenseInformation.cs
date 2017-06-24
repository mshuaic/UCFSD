using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Models
{
    class LicenseInformation
    {
        //props of license information json string from bemcli...
        public string Name { get; set; }

        public string Id { get; set; }

        public bool IsInstalled { get; set; }

        public bool IsLicensed { get; set; }

        public int LicenseCount { get; set; }

        public int LicenseUsedCount { get; set; }

        public bool IsEvaluation { get; set; }

        public int EvaluuationDaysRemaining { get; set; }

        public int TotalEvaluationDays { get; set; }

        public bool IsMaintenanceFeature { get; set; }

        public int FeatureId { get; set; }
    }
}
