using CheckupExec.Controllers;
using CheckupExec.Models;
using CheckupExec.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckupExec.Utilities;

namespace CheckupExec.Analysis
{
    public class BackupJobEstimate
    {
        public string JobName { get; }

        public DateTime NextStartDate { get; }

        public double EstimateOfJobRateMBMin { get; }

        public double EstimateOfElapsedTimeSec { get; }

        public double EstimateDataSizeMB { get; set; }

        public ForecastResults ForecastResults { get; set; }

        public bool isPoolDevice { get; set; }

        public string StorageName { get; }

        public long UsedCapacityBytes { get; }

        public long MaxCapacityBytes { get; }

        public string StorageType { get; }

        private string _jobId;

        //get all job instances and filter by 1) successful and 2) fully completed, then run analysis on those job instances 
        public BackupJobEstimate(string jobId)
        {
            _jobId = jobId;

            var jobPipeline = new Dictionary<string, string>
            {
                ["Id"] = jobId
            };
            var jobHistoryPipeline = new Dictionary<string, Dictionary<string, string>>
            {
                [Constants.GetJobs] = new Dictionary<string, string>
                                      {
                                          ["Id"] = jobId
                                      }
            };

            var jobAsList = DataExtraction.JobController.GetJobs(jobPipeline);
            Job job       = (jobAsList.Count > 0) ? jobAsList.First() : null;

            var jobHistories         = DataExtraction.JobHistoryController.GetJobHistories(jobHistoryPipeline);
            var filteredJobHistories = new List<JobHistory>();

            if (jobHistories.Count > 0)
            {
                foreach (JobHistory jobHistory in jobHistories)
                {
                    if (Convert.ToInt32(jobHistory.JobStatus) == JobHistory.SuccessfulFinalStatus && jobHistory.PercentComplete == 100)
                    {
                        filteredJobHistories.Add(jobHistory);
                    }
                }
            }

            if (filteredJobHistories.Count > 0)
            {
                JobName                  = job.Name;
                NextStartDate            = job.NextStartDate;
                EstimateOfJobRateMBMin   = estimateJobRate(filteredJobHistories);
                EstimateOfElapsedTimeSec = estimateElapsedTime(filteredJobHistories);
            }
        }

        //job estimate is the average rate of all previous filtered job instances
        private double estimateJobRate(List<JobHistory> jobHistories)
        {
            if (jobHistories.Count > 0)
            {
                int count  = 0;
                double sum = 0;

                foreach (JobHistory jobHistory in jobHistories)
                {
                    sum += jobHistory.JobRateMBPerMinute;
                    count++;
                }

                try
                {
                    return sum / count;
                }
                catch (DivideByZeroException e)
                {
                    //log utility divide by zero encountered because the job has no successful job histories
                    return -1;
                }
            }

            return -1;
        }

        //elapsedtime is estimated by applying estimaterate to estimatedatasize. data size is estimated by attempting to forecast the job; however
        //if this fails, it is estimated by adding on the average size increase between previous instances to the most recent instance's data size
        private double estimateElapsedTime(List<JobHistory> jobHistories)
        {
            if (jobHistories.Count > 0)
            {
                var _fc = new Forecast<JobHistory>();
    
                SortingUtility<JobHistory>.sort(jobHistories, 0, jobHistories.Count - 1);

                ForecastResults = _fc.doForecast(jobHistories);

                if (ForecastResults.ForecastSuccessful)
                {
                    //Console.WriteLine("Slope: " + ForecastResults.FinalSlope);
                    //Console.WriteLine("Intercept: " + ForecastResults.FinalIntercept);
                    EstimateDataSizeMB = ForecastResults.FinalIntercept + ForecastResults.FinalSlope * NextStartDate.Subtract(DateTime.Now).TotalDays;
                }
                else
                {
                    int count  = 0;
                    double sum = 0;
                    double currentSizeGB = jobHistories.First().TotalDataSizeBytes >> 20;

                    jobHistories.Remove(jobHistories.First());

                    if (jobHistories.Count > 0)
                    {
                        foreach (JobHistory jobHistory in jobHistories)
                        {
                            sum += (jobHistory.TotalDataSizeBytes >> 20) - currentSizeGB;
                            currentSizeGB = jobHistory.TotalDataSizeBytes >> 20;
                            count++;
                        }
                    }

                    try
                    {
                        EstimateDataSizeMB = ((jobHistories[count - 1].TotalDataSizeBytes >> 20) + sum / count) / 1024;
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        //index out of range
                    }
                }

                try
                {
                    return 60 * (EstimateDataSizeMB * 1024) / EstimateOfJobRateMBMin;
                }
                catch (DivideByZeroException e)
                {
                    //log utility divide by zero encountered because the job has no successful job histories
                    return -1;
                }
            }

            return -1;
        }
    }
}
