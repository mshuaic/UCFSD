using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Models.AnalysisModels
{
    public class FE_Forecast
    {
        public Storage Storage { get; set; }

        public List<JobHistory> JobHistories { get; set; }

        public ForecastResults ForecastResults { get; set; }

        public double UsedCapacity { get; set; }

        public double MaxCapacity { get; set; }
    }
}
