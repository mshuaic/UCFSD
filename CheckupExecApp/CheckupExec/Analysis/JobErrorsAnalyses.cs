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

        public JobErrorsAnalyses()
        {
            var jobHistoryController = new JobHistoryController();

            jobHistories.AddRange(jobHistoryController.GetJobHistories());

            foreach(var jobHistory in jobHistories)
            {
                if (Convert.ToInt32(jobHistory.JobStatus) == JobHistory.SuccessfulFinalStatus)
                    jobHistories.Remove(jobHistory);
            }
        }

        public JobErrorsAnalyses(DateTime? start, DateTime? end)
        {
            var jobHistoryController = new JobHistoryController();

            var jobPipeline = new Dictionary<string, string>
            {
                ["FromStartTime"] = start.ToString() ?? DateTime.MinValue.ToString(),
                ["ToStartTime"] = end.ToString() ?? DateTime.Now.ToString()
            };

            jobHistories.AddRange(jobHistoryController.GetJobHistoriesBy(jobPipeline));

            foreach (var jobHistory in jobHistories)
            {
                if (Convert.ToInt32(jobHistory.JobStatus) == JobHistory.SuccessfulFinalStatus)
                    jobHistories.Remove(jobHistory);
            }
        }
    }
}
