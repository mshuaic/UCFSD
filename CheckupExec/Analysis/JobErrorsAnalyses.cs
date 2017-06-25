using CheckupExec.Controllers;
using CheckupExec.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Analysis
{
    public class JobErrorsAnalyses
    {
        private List<JobHistory> _jobHistories { get; }

        public bool Successful { get; }

        public JobErrorsAnalyses(DateTime? start, DateTime? end, List<string> jobErrorStatuses = null, List<string> jobNames = null)
        {
            var jobHistoryController = new JobHistoryController();

            var jobHistoryPipeline = new Dictionary<string, string>
            {
                ["FromStartTime"] = start.ToString() ?? DateTime.MinValue.ToString(),
                ["ToStartTime"] = end.ToString() ?? DateTime.Now.ToString()
            };

            if (jobErrorStatuses.Count > 0 && jobNames.Count > 0)
            {
                var jobPipeline = new Dictionary<string, Dictionary<string, string>>();
                var jobInnerPipeline = new Dictionary<string, string>();

                string fullString = "";

                foreach (var name in jobNames)
                {
                    fullString += name + ((jobNames.ElementAt(jobNames.Count - 1).Equals(name)) ? "" : ", ");
                }

                jobInnerPipeline["name"] = fullString;
                jobPipeline["get-bejob"] = jobInnerPipeline;

                fullString = "";

                foreach (var errorstatus in jobErrorStatuses)
                {
                    fullString += errorstatus + ((jobErrorStatuses.ElementAt(jobErrorStatuses.Count - 1).Equals(errorstatus)) ? "" : ", ");
                }

                jobHistoryPipeline["jobstatus"] = fullString;

                _jobHistories.AddRange(jobHistoryController.GetJobHistories(jobPipeline, jobHistoryPipeline));
            }
            else if (jobNames.Count > 0)
            {
                var jobPipeline = new Dictionary<string, Dictionary<string, string>>();
                var jobInnerPipeline = new Dictionary<string, string>();

                string fullString = "";

                foreach (var name in jobNames)
                {
                    fullString += name + ((jobNames.ElementAt(jobNames.Count - 1).Equals(name)) ? "" : ", ");
                }

                jobInnerPipeline["name"] = fullString;
                jobPipeline["get-bejob"] = jobInnerPipeline;

                _jobHistories.AddRange(jobHistoryController.GetJobHistories(jobPipeline, jobHistoryPipeline));
            }
            else if (jobErrorStatuses.Count > 0)
            {
                string fullString = "";

                foreach (var errorstatus in jobErrorStatuses)
                {
                    fullString += errorstatus + ((jobErrorStatuses.ElementAt(jobErrorStatuses.Count - 1).Equals(errorstatus)) ? "" : ", ");
                }

                jobHistoryPipeline["jobstatus"] = fullString;

                _jobHistories.AddRange(jobHistoryController.GetJobHistories(jobHistoryPipeline));
            }
            else
            {
                _jobHistories.AddRange(jobHistoryController.GetJobHistories(jobHistoryPipeline));
            }

            foreach (var jobHistory in _jobHistories)
            {
                if (Convert.ToInt32(jobHistory.JobStatus) == JobHistory.SuccessfulFinalStatus)
                    _jobHistories.Remove(jobHistory);
            }
        }
    }
}
