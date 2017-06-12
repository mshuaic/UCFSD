using CheckupExec.Controllers;
using CheckupExec.Models;
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

        public BackupJobEstimate(string jobId)
        {
            JobController jobController = new JobController();
            JobHistoryController jobHistoryController = new JobHistoryController();

            var jobPipeline = new Dictionary<string, string>
            {
                ["Id"] = jobId
            };
            var jobHistoryPipeline = new Dictionary<string, Dictionary<string, string>>
            {
                ["get-bejob"] = new Dictionary<string, string>()
                                {
                                    ["Id"] = jobId
                                }
            };

            var jobAsList = jobController.GetJobsBy(jobPipeline);
            var job = jobAsList.First<Job>();

            var jobHistories = jobHistoryController.GetJobHistoriesPipeline(jobHistoryPipeline);

            JobName = job.Name;
            NextStartDate = job.NextStartDate;
            EstimateOfJobRateMBMin = estimateJobRate(jobHistories);
            EstimateOfElapsedTimeSec = estimateElapsedTime(jobHistories);
        }

        private double estimateJobRate(List<JobHistory> jobHistories)
        {
            int count = 0;
            double sum = 0;

            foreach (var jobHistory in jobHistories)
            {
                if (Convert.ToInt32(jobHistory.JobStatus) == 9 && jobHistory.PercentComplete == 100)
                {
                    sum += jobHistory.JobRateMBPerMinute;
                    count++;
                }
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
            int count = 0;
            double sum = 0;

            foreach (var jobHistory in jobHistories)
            {
                if (Convert.ToInt32(jobHistory.JobStatus) == 9 && jobHistory.PercentComplete == 100)
                {
                    sum += jobHistory.ElapsedTime.TotalSeconds;
                    count++;
                }
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
    }
}
