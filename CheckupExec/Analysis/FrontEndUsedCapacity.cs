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

            var jobHistoryPipelineLast30Days = new Dictionary<string, Dictionary<string, string>>
            {
                [Constants.GetStorages] = new Dictionary<string, string>
                {
                    ["Id"] = ""
                },
                [Constants.GetJobs] = new Dictionary<string, string>
                {
                    ["TaskType"] = "'Full'"
                }
            };

            var jobHistoryParamsLast30Days = new Dictionary<string, string>
            {
                ["FromStartTime"] = "'" + DateTime.Now.AddDays(-60).ToString() + "'"
            };

            var jobHistoryPipeline = new Dictionary<string, Dictionary<string, string>>
            {
                [Constants.GetJobs] = new Dictionary<string, string>
                {
                    ["Id"] = ""
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
                    //if storage device is not a pool of devices (we want to look at jobs of individual resources)
                    if (!storageDevice.StorageType.Equals("0") || true)
                    {
                        jobHistoryPipelineLast30Days[Constants.GetStorages]["Id"] = "'" + storageDevice.Id + "'";

                        //get instances of full backup jobs that ran in the last 30 days of current device
                        var temp = DataExtraction.JobHistoryController.GetJobHistories(jobHistoryPipelineLast30Days, jobHistoryParamsLast30Days);

                        JobHistory largestFullBackupLast30Days = new JobHistory();

                        foreach (JobHistory jobHistory in temp)
                        {
                            if (Convert.ToInt32(jobHistory.JobStatus) == JobHistory.SuccessfulFinalStatus
                                && jobHistory.PercentComplete == 100
                                && jobHistory.TotalDataSizeBytes > largestFullBackupLast30Days.TotalDataSizeBytes)
                            {
                                largestFullBackupLast30Days = jobHistory;
                            }
                        }

                        jobHistoryPipeline[Constants.GetJobs]["Id"] = "'" + largestFullBackupLast30Days.JobId + "'";

                        //get all instances of the largest full backup job found in the last 30 days
                        temp = DataExtraction.JobHistoryController.GetJobHistories(jobHistoryPipeline);

                        if (temp.Count > 0)
                        {
                            var fullBackupJobInstance = new FullBackupJobInstance { Storage = storageDevice };

                            fullBackupJobInstance.JobHistories = new List<JobHistory>();

                            SortingUtility<JobHistory>.sort(temp, 0, temp.Count - 1);

                            foreach (JobHistory jobHistory in temp)
                            {
                                if (Convert.ToInt32(jobHistory.JobStatus) == JobHistory.SuccessfulFinalStatus && jobHistory.PercentComplete == 100)
                                {
                                    fullBackupJobInstance.JobHistories.Add(jobHistory);
                                    lastFullBackupJobInstance = jobHistory;
                                }
                            }

                            _fullBackupJobInstances.Add(fullBackupJobInstance);

                            //presumably each storage device will have 1 largest job instance found. This was used before when we were looking at all jobs
                            //indiscriminately, however, to ensure that we were not exaggerating the current capacity usage. Current capacity usage is being
                            //calculated with the last full, successfully completed backup jobs of each storage device
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
