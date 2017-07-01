using CheckupExec.Controllers;
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
    public class FrontEndUsedCapacity
    {
        public double TotalUsedCapacity { get; set; }

        public FrontEndForecast FrontEndForecast { get; }

        private List<FullBackupJobInstance> _fullBackupJobInstances { get; set; }

        public FrontEndUsedCapacity(List<Storage> storageDevices)
        {
            _fullBackupJobInstances = new List<FullBackupJobInstance>();

            var jobHistoryPipeline = new Dictionary<string, Dictionary<string, string>>
            {
                [Constants.GetStorages] = new Dictionary<string, string>
                {
                    ["Id"] = ""
                },
                [Constants.GetJobs] = new Dictionary<string, string>
                {
                    ["TaskType"] = "Full"
                }
            };

            var storagesAccountedFor      = new List<string>();
            var lastFullBackupJobInstance = new JobHistory();

            //if we have storages, and they are not a pool of devices, set up the list _fullBackupJobInstances with { storage: List<JobHistory> }'s
            //then pass this list to FrontendForecast
            if (storageDevices != null && storageDevices.Count > 0)
            {
                foreach (Storage storageDevice in storageDevices)
                {
                    // || true for testing with our sets
                    if (!storageDevice.StorageType.Equals("0") || true)
                    {
                        jobHistoryPipeline[Constants.GetStorages]["Id"] = "'" + storageDevice.Id + "'";

                        var temp = DataExtraction.JobHistoryController.GetJobHistories(jobHistoryPipeline);

                        if (temp.Count > 0)
                        {
                            var fullBackupJobInstance = new FullBackupJobInstance { Storage = storageDevice };

                            fullBackupJobInstance.JobHistories = new List<JobHistory>();

                            foreach (JobHistory jobHistory in temp)
                            {
                                if (Convert.ToInt32(jobHistory.JobStatus) == JobHistory.SuccessfulFinalStatus && jobHistory.PercentComplete == 100)
                                {
                                    fullBackupJobInstance.JobHistories.Add(jobHistory);
                                    lastFullBackupJobInstance = jobHistory;
                                }
                            }

                            SortingUtility<JobHistory>.sort(fullBackupJobInstance.JobHistories, 0, fullBackupJobInstance.JobHistories.Count - 1);

                            _fullBackupJobInstances.Add(fullBackupJobInstance);

                            if (!storagesAccountedFor.Contains(storageDevice.Name))
                            {
                                TotalUsedCapacity += (double)((lastFullBackupJobInstance?.TotalDataSizeBytes) >> 20) / 1024;
                                storagesAccountedFor.Add(storageDevice.Name);
                            }
                        }

                        lastFullBackupJobInstance = null;
                    }
                }
            }

            FrontEndForecast = new FrontEndForecast(_fullBackupJobInstances);
        }
    }
}
