using System.Collections.Generic;

namespace CheckupExec.Models.AnalysisModels
{
    public class ForecastResults
    {
        public bool IsDiskForecast { get; set; }

        public bool ForecastSuccessful { get; set; }

        public List<PlotPoint> Plot;

        public double FinalSlope { get; set; }

        public double FinalIntercept { get; set; }
    }
}
