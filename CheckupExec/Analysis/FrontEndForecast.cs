using CheckupExec.Models;
using CheckupExec.Models.AnalysisModels;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Analysis
{
    public class FrontEndForecast
    {
        private bool _forecastsSuccessful;
        public bool ForecastsSuccessful
        {
            get
            {
                return _forecastsSuccessful;
            }
        }

        public FrontEndForecast(List<FE_Forecast> fe_forecasts)
        {
            _forecastsSuccessful = true;

            var fc       = new Forecast<JobHistory>();

            //for each element, create an element in Forecasts like { storage: ForecastResults } and compute maxcapacity available between all storage devices
            //being backed up fully
            if (fe_forecasts != null && fe_forecasts.Count > 0)
            {
                foreach (FE_Forecast fe_forecast in fe_forecasts)
                {
                    fe_forecast.ForecastResults = fc.doForecast(fe_forecast.JobHistories);

                    if (!fe_forecast.ForecastResults.ForecastSuccessful)
                    {
                        _forecastsSuccessful = false;
                        break;
                    }

                    //max capacity that can be stored on device
                    fe_forecast.MaxCapacity += (double)(fe_forecast.Storage.TotalCapacityBytes >> 20) / 1024;
                }
            }
        }
    }
}
