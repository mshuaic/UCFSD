using CheckupExec.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Models.AnalysisModels
{
    public class UsedCapacityForecastModel
    {
        public string StorageName { get; set; }

        public long TotalCapacity { get; set; }

        public List<UsedCapacity> UsedCapacityInstances { get; set; }

        public ForecastResults ForecastResults { get; set; }
    }
}
