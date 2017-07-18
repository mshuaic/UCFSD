using CheckupExec.Models.AnalysisModels;
using System;
using System.Collections.Generic;

namespace CheckupExec.Models.ReportModels
{
    public class BackupJobReport
    {
        public List<PlotPoint> HistoricalPoints { get; set; }

        public List<PlotPoint> ForecastPoints { get; set; }

        public string StorageName { get; set; }

        public string StorageType { get; set; }

        public double MaxCapacity { get; set; }

        public double UsedCapacity { get; set; }

        public double DaysTo50 { get; set; }

        public double DaysTo75 { get; set; }

        public double DaysTo90 { get; set; }

        public double DaysToFull { get; set; }

        public string JobName { get; set; }

        public DateTime NextJobDate { get; set; }

        public double NextJobRate { get; set; }

        public double NextElapsedTimeSeconds { get; set; }

        public double NextDataSize { get; set; }
    }
}
