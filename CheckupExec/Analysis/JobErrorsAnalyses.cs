using CheckupExec.Controllers;
using CheckupExec.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Analysis
{
    class JobErrorsAnalyses
    {
        public List<JobHistory> jobHistories { get; }

        public JobErrorsAnalyses(DateTime? start, DateTime? end, string[] jobErrorStatuses = null)
        {
            var jobHistoryController = new JobHistoryController();

            var jobPipeline = new Dictionary<string, string>
            {
                ["FromStartTime"] = start.ToString() ?? DateTime.MinValue.ToString(),
                ["ToStartTime"] = end.ToString() ?? DateTime.Now.ToString()
            };

            if (jobErrorStatuses.Length > 0)
            {
                string fullString = "";

                foreach (var errorstatus in jobErrorStatuses)
                {
                    fullString += errorstatus + ((jobErrorStatuses.ElementAt(jobErrorStatuses.Length - 1).Equals(errorstatus)) ? "" : ", ");
                }

                jobPipeline["jobstatus"] = fullString;

                jobHistories.AddRange(jobHistoryController.GetJobHistories(jobPipeline));
            }
            else
            {
                jobHistories.AddRange(jobHistoryController.GetJobHistories(jobPipeline));
            }

            foreach (var jobHistory in jobHistories)
            {
                if (Convert.ToInt32(jobHistory.JobStatus) == JobHistory.SuccessfulFinalStatus)
                    jobHistories.Remove(jobHistory);
            }
        }
    }
}
