using System;

namespace CheckupExec.Models.AnalysisModels
{
    public class BackupJobEstimateModel
    {
        public string JobName { get; set; }

        public DateTime NextStartDate { get; set; }

        public double EstimateOfJobRateMBMin { get; set; }

        public double EstimateOfElapsedTimeSec { get; set; }

        public double EstimateDataSizeMB { get; set; }

        public ForecastResults ForecastResults { get; set; }

        public bool IsPoolDevice { get; set; }

        public string StorageName { get; set; }

        public long UsedCapacityBytes { get; set; }

        public long MaxCapacityBytes { get; set; }

        public string StorageType { get; set; }

        public string JobId { get; set; }
    }
}
