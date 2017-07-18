using CheckupExec.Models.AnalysisModels;
using System.Collections.Generic;

namespace CheckupExec.Models.ReportModels
{
    public class DiskCapacityReport
    {
        public List<PlotPoint> HistoricalPoints { get; set; }

        public List<PlotPoint> ForecastPoints { get; set; }

        public string DiskName { get; set; }

        public double MaxCapacity { get; set; }

        public double UsedCapacity { get; set; }

        public double DaysTo50 { get; set; }

        public double DaysTo75 { get; set; }

        public double DaysTo90 { get; set; }

        public double DaysToFull { get; set; }
    }
}
