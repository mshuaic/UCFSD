using System.Collections.Generic;

namespace CheckupExec.Models.AnalysisModels
{
    public class FeForecast
    {
        public Storage Storage { get; set; }

        public string JobName { get; set; }

        public List<JobHistory> JobHistories { get; set; }

        public ForecastResults ForecastResults { get; set; }

        public double UsedCapacity { get; set; }

        public double MaxCapacity { get; set; }
    }
}
