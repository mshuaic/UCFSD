using CheckupExec.Models;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckupExec.Analysis
{
    public class JobErrorsAnalyses
    {
        private List<JobHistory> _jobHistories { get; }

        public bool Successful { get; }

        //All this is doing right now is getting job histories depending on the params passed
        public JobErrorsAnalyses(DateTime? start = null, DateTime? end = null, List<string> jobErrorStatuses = null, List<string> jobNames = null)
        {
            Successful = true;

            _jobHistories = new List<JobHistory>();

            //***************************************
            var jobHistoryPipeline = new Dictionary<string, string>
            {
                ["FromStartTime"] = (start == null) ? "'" + DateTime.MinValue.Date + "'" : "'" + start + "'",
                ["ToStartTime"] = (end == null) ? "'" + DateTime.Now.Date.ToUniversalTime() + "'" : "'" + end + "'"
            };
            jobErrorStatuses = jobErrorStatuses ?? new List<string>();
            jobNames = jobNames ?? new List<string>();

            if (jobErrorStatuses.Count > 0 && jobNames.Count > 0)
            {
                var jobPipeline = new Dictionary<string, Dictionary<string, string>>();
                var jobInnerPipeline = new Dictionary<string, string>();

                string fullString = jobNames
                                    .Aggregate("", (current, name) => current + ("'" + name + "'" + ((jobNames
                                                                                                     .ElementAt(jobNames.Count - 1)
                                                                                                     .Equals(name)) ? "" : ", ")));

                jobInnerPipeline["name"] = fullString;
                jobPipeline[Constants.GetJobs] = jobInnerPipeline;

                try
                {
                    _jobHistories.AddRange(DataExtraction.JobHistoryController.GetJobHistories(jobPipeline, jobHistoryPipeline));
                }
                catch
                {
                    //log
                }

                var temp = _jobHistories.Where(jobHistory => !jobErrorStatuses.Contains(Constants.JobErrorStatuses[jobHistory.JobStatus])).ToList();

                foreach (JobHistory jobHistory in temp)
                {
                    _jobHistories.Remove(jobHistory);
                }
            }
            else if (jobNames.Count > 0)
            {
                var jobPipeline = new Dictionary<string, Dictionary<string, string>>();
                var jobInnerPipeline = new Dictionary<string, string>();

                string fullString = jobNames
                                    .Aggregate("", (current, name) => current + ("'" + name + "'" + ((jobNames
                                                                                                      .ElementAt(jobNames.Count - 1)
                                                                                                      .Equals(name)) ? "" : ", ")));

                jobInnerPipeline["name"] = fullString;
                jobPipeline[Constants.GetJobs] = jobInnerPipeline;

                try
                {
                    _jobHistories.AddRange(DataExtraction.JobHistoryController.GetJobHistories(jobPipeline, jobHistoryPipeline));
                }
                catch
                {
                    //log
                }
            }
            else if (jobErrorStatuses.Count > 0)
            {
                try
                {
                    _jobHistories.AddRange(DataExtraction.JobHistoryController.GetJobHistories(jobHistoryPipeline));
                }
                catch
                {
                    //log
                }

                var temp = _jobHistories.Where(JobHistory => !jobErrorStatuses.Contains(Constants.JobErrorStatuses[JobHistory.JobStatus])).ToList();

                foreach (JobHistory jobHistory in temp)
                {
                    _jobHistories.Remove(jobHistory);
                }
            }
            else
            {
                try
                {
                    _jobHistories.AddRange(DataExtraction.JobHistoryController.GetJobHistories(jobHistoryPipeline));
                }
                catch
                {
                    //log
                }
            }

            var filteredJobHistories = new List<JobHistory>();

            if (_jobHistories != null && _jobHistories.Count > 0)
            {
                filteredJobHistories.AddRange(_jobHistories.Where(jobHistory => Convert.ToInt32(jobHistory.JobStatus) == Constants.SUCCESSFUL_JOB_STATUS));

                foreach (JobHistory jobHistory in filteredJobHistories)
                {
                    _jobHistories.Remove(jobHistory);
                }
            }
            else
            {
                Successful = false;
            }
        }

        public List<JobHistory> GetJobHistories()
        {
            return _jobHistories;
        }
    }
}
