using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Models
{
    public class ForecastResults
    {
        public bool isDiskForecast { get; set; }

        public bool ForecastSuccessful { get; set; }

        public Dictionary<double, double> plot;

        public double FinalSlope { get; set; }

        public double FinalIntercept { get; set; }
    }
}
