using CheckupExec.Controllers;
using CheckupExec.Models;
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

        private Dictionary<Storage, List<JobHistory>> _fullBackupJobInstances { get; set; }

        public FrontEndUsedCapacity(List<Storage> storageDevices)
        {
            _fullBackupJobInstances = new Dictionary<Storage, List<JobHistory>>();

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

            var storagesAccountedFor = new List<string>();
            var lastFullBackupJobInstance = new JobHistory();

            if (storageDevices.Count > 0)
            {
                foreach (var storageDevice in storageDevices)
                {
                    // || true for testing with our sets
                    if (!storageDevice.StorageType.Equals("0") || true)
                    {
                        jobHistoryPipeline[Constants.GetStorages]["Id"] = "'" + storageDevice.Id + "'";
                        var temp = DataExtraction.JobHistoryController.GetJobHistories(jobHistoryPipeline);

                        if (temp.Count > 0)
                        {
                            _fullBackupJobInstances[storageDevice] = new List<JobHistory>();

                            foreach (var jobHistory in temp)
                            {
                                if (Convert.ToInt32(jobHistory.JobStatus) == JobHistory.SuccessfulFinalStatus && jobHistory.PercentComplete == 100)
                                {
                                    _fullBackupJobInstances[storageDevice].Add(jobHistory);
                                    lastFullBackupJobInstance = jobHistory;
                                }
                            }

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
