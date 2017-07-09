using CheckupExec.Controllers;
using CheckupExec.Models;
using CheckupExec.Models.BEMCLIModels;
using CheckupExec.Models.ReportModels;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Analysis
{
    //license model

    public class LicenseAnalysis
    {
        public int TotalDataCoverageTB { get; }

        public int TotalDataCoverageUsedTB { get; }

        //likely will be called straight from frontend forecast to tie the forecast with the user's licensing setup
        public LicenseAnalysis(FrontEndCapacityReport report)
        {
            List<EditionInformation> editionInformationAsList = DataExtraction.EditionInformationController.GetEditionInformation();

            EditionInformation editionInformation = editionInformationAsList.First();

            List<LicenseInformation> licenses = DataExtraction.LicenseInformationController.GetLicenses();

            var serverEdition = (editionInformation == null) ? "" : editionInformation.Edition;

            int maxCoverage = 0;
            int maxCoverageUsed = 0;

            if (serverEdition.Equals("Capacity Edition"))
            {
                //if this remains true after enumeration, user should just use Lite since the options he uses are all supported in 
                //Lite and he uses none of those supported in normal
                report.NormalToLite = true;

                foreach (LicenseInformation license in licenses)
                {
                    if (Constants.CapacityEditionCoverage[license.Name])
                    {
                        if (report.NormalToLite && !Constants.CapacityEditionLiteCoverage[license.Name])
                        {
                            report.NormalToLite = false;
                        }

                        TotalDataCoverageTB = (maxCoverage > license.LicenseCount) ? maxCoverage : license.LicenseCount;
                        TotalDataCoverageUsedTB = (maxCoverageUsed > license.LicenseUsedCount) ? maxCoverageUsed : license.LicenseUsedCount;
                    }
                }
            }
            else if (serverEdition.Equals("Capacity Edition Lite"))
            {
                report.LiteToNormal = new List<string>();

                foreach (LicenseInformation license in licenses)
                {
                    if (Constants.CapacityEditionLiteCoverage[license.Name])
                    {
                        TotalDataCoverageTB = (maxCoverage > license.LicenseCount) ? maxCoverage : license.LicenseCount;
                        TotalDataCoverageUsedTB = (maxCoverageUsed > license.LicenseUsedCount) ? maxCoverageUsed : license.LicenseUsedCount;
                    }
                    else if (Constants.CapacityEditionCoverage[license.Name])
                    {
                        //recommended to switch because option is not covered in lite but is in normal
                        report.LiteToNormal.Add(license.Name);
                    }
                }
            }
            else
            {
                report.LicensingApplicable = false;
                return;
            }

            report.LicensingApplicable = true;
            report.TotalDataCoverageTB = TotalDataCoverageTB;
            report.TotalDataCoverageUsedTB = TotalDataCoverageUsedTB;

            if ((TotalDataCoverageTB << 10) < report.MaxCapacity)
            {
                report.DaysToLicenseExceeded = ((TotalDataCoverageTB << 10) - report.Intercept) / report.Slope;

                report.RecommendedEditionCountToPurchase = (int)(report.MaxCapacity / (1024 * 1024)) - TotalDataCoverageTB;
                report.RecommendedTier = Constants.TierDiscounts.ToList()
                                        .Find(x => x.Key.Contains(report.RecommendedEditionCountToPurchase) 
                                              || (x.Key[0] == 26 && report.RecommendedEditionCountToPurchase >= x.Key[0])).Value;
             }
            else
            {
                //they are over-licensed for their current maximum capacity (won't run out with current storage)
                report.DaysToLicenseExceeded = -1;
                report.RecommendedEditionCountToPurchase = -1;
                report.RecommendedTier = "";
            }
        }
    }
}
