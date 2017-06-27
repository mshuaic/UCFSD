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

        public double MaxCapacity { get; }

        public List<FE_Forecast> Forecasts { get; set; }

        public FrontEndForecast(List<FullBackupJobInstance> fullBackupJobInstances)
        {
            _forecastsSuccessful = true;

            Forecasts    = new List<FE_Forecast>();
            var fc = new Forecast<JobHistory>();

            //for each element, create an element in Forecasts like { storage: ForecastResults } and compute maxcapacity available between all storage devices
            //being backed up fully
            if (fullBackupJobInstances != null && fullBackupJobInstances.Count > 0)
            {
                foreach (FullBackupJobInstance fullBackupJobInstance in fullBackupJobInstances)
                {
                    var forecast = new FE_Forecast
                    {
                        Storage         = fullBackupJobInstance.Storage,
                        ForecastResults = fc.doForecast(fullBackupJobInstance.JobHistories)
                    };
                    
                    if (!forecast.ForecastResults.ForecastSuccessful)
                    {
                        _forecastsSuccessful = false;
                        break;
                    }

                    Forecasts.Add(forecast);
                    MaxCapacity += (double)(fullBackupJobInstance.Storage.TotalCapacityBytes >> 20) / 1024;
                }
            }
        }
    }
}
