using CheckupExec.Models;
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

        public long MaxCapacityBytes { get; }

        public Dictionary<Storage, ForecastResults> Forecasts { get; set; }

        public FrontEndForecast(Dictionary<Storage, List<JobHistory>> fullBackupJobInstances)
        {
            _forecastsSuccessful = true;
            var forecast = new Forecast<JobHistory>();


            if (fullBackupJobInstances != null)
            {
                foreach (var storageDevice in fullBackupJobInstances)
                {
                    Forecasts[storageDevice.Key] = forecast.doForecast(storageDevice.Value);

                    if (!Forecasts[storageDevice.Key].ForecastSuccessful)
                    {
                        _forecastsSuccessful = false;
                        break;
                    }

                    MaxCapacityBytes += storageDevice.Key.TotalCapacityBytes;
                }
            }
        }
    }
}
