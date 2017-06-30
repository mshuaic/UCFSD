using CheckupExec.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Models.AnalysisModels
{
    public class DiskForecastModel
    {
        public string DiskName { get; set; }

        public double TotalCapacity { get; set; }

        public List<DiskCapacity> DiskInstances { get; set; }

        public ForecastResults ForecastResults { get; set; }
    }
}
