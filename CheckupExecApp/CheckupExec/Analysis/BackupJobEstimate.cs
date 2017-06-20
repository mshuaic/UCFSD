using CheckupExec.Controllers;
using CheckupExec.Models;
using CheckupExec.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Analysis
{
    public class BackupJobEstimate
    {
        public string JobName { get; }

        public DateTime NextStartDate { get; }

        public double EstimateOfJobRateMBMin { get; }

        public double EstimateOfElapsedTimeSec { get; }

        public double EstimateDataSizeMB { get; set; }

        private string _jobId;

        public BackupJobEstimate(string jobId)
        {
            _jobId = jobId;

            JobController jobController = new JobController();
            JobHistoryController jobHistoryController = new JobHistoryController();

            var jobPipeline = new Dictionary<string, string>
            {
                ["Id"] = jobId
            };
            var jobHistoryPipeline = new Dictionary<string, Dictionary<string, string>>
            {
                ["get-bejob"] = new Dictionary<string, string>
                                {
                                    ["Id"] = jobId
                                }
            };

            var jobAsList = jobController.GetJobsBy(jobPipeline);
            var job = jobAsList.First();

            var jobHistories = jobHistoryController.GetJobHistoriesPipeline(jobHistoryPipeline);
            var filteredJobHistories = new List<JobHistory>();

            foreach (var jobHistory in jobHistories)
            {
                if (Convert.ToInt32(jobHistory.JobStatus) == JobHistory.SuccessfulFinalStatus && jobHistory.PercentComplete == 100)
                {
                    filteredJobHistories.Add(jobHistory);
                }
            }

            JobName = job.Name;
            NextStartDate = job.NextStartDate;
            EstimateOfJobRateMBMin = estimateJobRate(filteredJobHistories);
            EstimateOfElapsedTimeSec = estimateElapsedTime(filteredJobHistories);
        }

        private double estimateJobRate(List<JobHistory> jobHistories)
        {
            int count = 0;
            double sum = 0;

            foreach (var jobHistory in jobHistories)
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

        private double estimateElapsedTime(List<JobHistory> jobHistories)
        {
            var _fc = new Forecast(this._jobId);

            if (_fc.ForecastSuccessful)
            {
                Console.WriteLine("Slope: " + _fc.FinalSlope);
                Console.WriteLine("Intercept: " + _fc.FinalIntercept);
                EstimateDataSizeMB = _fc.FinalIntercept + _fc.FinalSlope * this.NextStartDate.Subtract(DateTime.Now).TotalDays;
            }
            else
            {
                int count = 0;
                double sum = 0;
                double currentSizeGB = jobHistories.First().TotalDataSizeBytes >> 20;

                jobHistories.Remove(jobHistories.First());

                foreach (var jobHistory in jobHistories)
                {
                    sum += (jobHistory.TotalDataSizeBytes >> 20) - currentSizeGB;
                    currentSizeGB = jobHistory.TotalDataSizeBytes >> 20;
                    count++;
                }

                EstimateDataSizeMB = ((jobHistories[count - 1].TotalDataSizeBytes >> 20) + sum / count) / 1024;
            }

            try
            {
                return 60 * (EstimateDataSizeMB * 1024)  / EstimateOfJobRateMBMin;
            }
            catch (DivideByZeroException e)
            {
                //log utility divide by zero encountered because the job has no successful job histories
                return -1;
            }
        }
    }
}
