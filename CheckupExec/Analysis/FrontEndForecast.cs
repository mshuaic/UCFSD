using CheckupExec.Models;
using CheckupExec.Models.AnalysisModels;
using CheckupExec.Utilities;
using System.Collections.Generic;

namespace CheckupExec.Analysis
{
    public class FrontEndForecast
    {
        public bool ForecastsSuccessful { get; }

        public FrontEndForecast(IReadOnlyCollection<FeForecast> feForecasts)
        {
            ForecastsSuccessful = true;

            var fc = new Forecast<JobHistory>();

            //for each element, create an element in Forecasts like { storage: ForecastResults } and compute maxcapacity available between all storage devices
            //being backed up fully
            if (feForecasts == null || feForecasts.Count <= 0) return;

            foreach (FeForecast feForecast in feForecasts)
            {
                feForecast.ForecastResults = fc.doForecast(feForecast.JobHistories);

                if (!feForecast.ForecastResults.ForecastSuccessful)
                {
                    ForecastsSuccessful = false;
                    break;
                }

                //max capacity that can be stored on device
                feForecast.MaxCapacity += (double)(feForecast.Storage.TotalCapacityBytes >> 20) / 1024;
            }
        }
    }
}
