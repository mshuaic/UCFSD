using CheckupExec.Controllers;
using CheckupExec.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Analysis
{
    public class FrontEndUsedCapacity
    {
        public double TotalUsedCapacity { get; set; }

        public bool _forecastSuccessful { get; set; }

        public FrontEndForecast Forecast { get; }

        private Dictionary<Storage, List<JobHistory>> _fullBackupJobInstances { get; set; }


        public FrontEndUsedCapacity()
        {
            var storageController = new StorageController();
            var jobHistoryController = new JobHistoryController();

            var storageDevices = storageController.GetStorages();

            var jobHistoryPipeline = new Dictionary<string, Dictionary<string, string>>
            {
                ["get-bestorage"] = new Dictionary<string, string>
                {
                    ["Id"] = ""
                },
                ["get-bejob"] = new Dictionary<string, string>
                {
                    ["TaskType"] = "Full"
                }
            };

            foreach (var storageDevice in storageDevices)
            {
                if (!storageDevice.StorageType.Equals("0"))
                {
                    jobHistoryPipeline["get-bestorage"]["Id"] = storageDevice.Id;
                    var temp = jobHistoryController.GetJobHistoriesPipeline(jobHistoryPipeline);
                    var lastFullBuJobInstance = new JobHistory();

                    if (temp != null)
                    { 
                        foreach (var jobHistory in temp)
                        {
                            if (Convert.ToInt32(jobHistory.JobStatus) == JobHistory.SuccessfulFinalStatus && jobHistory.PercentComplete == 100)
                            {
                                _fullBackupJobInstances[storageDevice].Add(jobHistory);
                                lastFullBuJobInstance = jobHistory;
                            }
                        }
                        TotalUsedCapacity += (double)lastFullBuJobInstance?.TotalDataSizeBytes;
                        lastFullBuJobInstance = null;
                    }
                }
            }

            Forecast = new FrontEndForecast(_fullBackupJobInstances);
        }
    }
}
