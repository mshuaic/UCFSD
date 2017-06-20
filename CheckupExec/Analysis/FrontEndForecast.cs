using CheckupExec.Models;
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

        public Dictionary<Storage, Forecast> forecasts { get; set; }

        public FrontEndForecast(Dictionary<Storage, List<JobHistory>> fullBackupJobInstances)
        {
            _forecastsSuccessful = true;

            if (fullBackupJobInstances != null)
            {
                foreach (var storageDevice in fullBackupJobInstances)
                {
                    forecasts[storageDevice.Key] = new Forecast(storageDevice.Key, storageDevice.Value);

                    if (!forecasts[storageDevice.Key].ForecastSuccessful)
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
