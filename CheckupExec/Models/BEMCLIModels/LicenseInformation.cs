namespace CheckupExec.Models
{
    public class LicenseInformation
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public bool IsInstalled { get; set; }

        public bool IsLicensed { get; set; }

        //The assumption is that license count is >= licenseusedcount. A user could be licensed for more than what they are using (or need?).
        //Also, if cap/cap lite, each eligible option is per front end TB, therefore, our forecast should determine: 1) when their cumm. licenseusedcount will
        //exceed their cumm. licensecount and 2) suggest a licensing model purchase count based on growth and tier

        public int LicenseCount { get; set; }

        public int LicenseUsedCount { get; set; }

        //not needed
        public bool IsEvaluation { get; set; }

        public int EvaluationDaysRemaining { get; set; }

        public int TotalEvaluationDays { get; set; }

        public bool IsMaintenanceFeature { get; set; }

        public int FeatureId { get; set; }
        //end not needed
    }
}
