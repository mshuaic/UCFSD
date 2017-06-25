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
            jobErrorStatuses = jobErrorStatuses ?? new List<string>();
            jobNames = jobNames ?? new List<string>();

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
                jobPipeline[Constants.GetJobs] = jobInnerPipeline;

                fullString = "";

                foreach (var errorstatus in jobErrorStatuses)
                {
                    fullString += errorstatus + ((jobErrorStatuses.ElementAt(jobErrorStatuses.Count - 1).Equals(errorstatus)) ? "" : ", ");
                }

                jobHistoryPipeline["jobstatus"] = fullString;

                try
                {
                    _jobHistories.AddRange(jobHistoryController.GetJobHistories(jobPipeline, jobHistoryPipeline));
                }
                catch
                {
                    //log
                }
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
                jobPipeline[Constants.GetJobs] = jobInnerPipeline;

                try
                {
                    _jobHistories.AddRange(jobHistoryController.GetJobHistories(jobPipeline, jobHistoryPipeline));
                }
                catch
                {
                    //log
                }
            }
            else if (jobErrorStatuses.Count > 0)
            {
                string fullString = "";

                foreach (var errorstatus in jobErrorStatuses)
                {
                    fullString += errorstatus + ((jobErrorStatuses.ElementAt(jobErrorStatuses.Count - 1).Equals(errorstatus)) ? "" : ", ");
                }

                jobHistoryPipeline["jobstatus"] = fullString;

                try
                {
                    _jobHistories.AddRange(jobHistoryController.GetJobHistories(jobHistoryPipeline));
                }
                catch
                {
                    //log
                }
            }
            else
            {
                try
                {
                    _jobHistories.AddRange(jobHistoryController.GetJobHistories(jobHistoryPipeline));
                }
                catch
                {
                    //log
                }
            }

            if (_jobHistories.Count > 0)
            {
                foreach (var jobHistory in _jobHistories)
                {
                    if (Convert.ToInt32(jobHistory.JobStatus) == JobHistory.SuccessfulFinalStatus)
                        _jobHistories.Remove(jobHistory);
                }
            }
        }
    }
}
