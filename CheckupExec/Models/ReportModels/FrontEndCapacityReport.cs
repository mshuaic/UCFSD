using CheckupExec.Models.AnalysisModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Models.ReportModels
{
    //todo: add license analysis props (w/e those are going to be)
    public class FrontEndCapacityReport
    {
        public List<PlotPoint> HistoricalPoints { get; set; }

        public List<PlotPoint> ForecastPoints { get; set; }

        public List<Storage> StorageDevices { get; set; }

        public double MaxCapacity { get; set; }

        public double UsedCapacity { get; set; }

        public double DaysTo50 { get; set; }

        public double DaysTo75 { get; set; }

        public double DaysTo90 { get; set; }

        public double DaysToFull { get; set; }

        //license information if applicable
    }
}
