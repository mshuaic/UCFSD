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

        //All this is doing right now is getting job histories depending on the params passed
        public JobErrorsAnalyses(DateTime? start = null, DateTime? end = null, List<string> jobErrorStatuses = null, List<string> jobNames = null)
        {
            Successful = true;

            _jobHistories = new List<JobHistory>();
            
            //***************************************
            var jobHistoryPipeline = new Dictionary<string, string>
            {
                ["FromStartTime"] = (start == null) ? "'" + DateTime.MinValue.Date.ToString() + "'" : "'" + start.ToString() + "'",
                ["ToStartTime"] = (end == null) ? "'" + DateTime.Now.Date.ToUniversalTime().ToString() + "'" : "'" + end.ToString() + "'"
            };
            jobErrorStatuses = jobErrorStatuses ?? new List<string>();
            jobNames         = jobNames ?? new List<string>();

            if (jobErrorStatuses.Count > 0 && jobNames.Count > 0)
            {
                var jobPipeline      = new Dictionary<string, Dictionary<string, string>>();
                var jobInnerPipeline = new Dictionary<string, string>();

                string fullString = "";

                foreach (string name in jobNames)
                {
                    fullString += "'" + name + "'" + ((jobNames.ElementAt(jobNames.Count - 1).Equals(name)) ? "" : ", ");
                }

                jobInnerPipeline["name"]       = fullString;
                jobPipeline[Constants.GetJobs] = jobInnerPipeline;

                fullString = "";

                try
                {
                    _jobHistories.AddRange(DataExtraction.JobHistoryController.GetJobHistories(jobPipeline, jobHistoryPipeline));
                }
                catch
                {
                    //log
                }

                var temp = new List<JobHistory>();

                foreach (JobHistory JobHistory in _jobHistories)
                {
                    if (!jobErrorStatuses.Contains(Constants.JobErrorStatuses[JobHistory.JobStatus]))
                    {
                        temp.Add(JobHistory);
                    }
                }

                foreach (JobHistory JobHistory in temp)
                {
                    _jobHistories.Remove(JobHistory);
                }
            }
            else if (jobNames.Count > 0)
            {
                var jobPipeline      = new Dictionary<string, Dictionary<string, string>>();
                var jobInnerPipeline = new Dictionary<string, string>();

                string fullString = "";

                foreach (string name in jobNames)
                {
                    fullString += "'" + name + "'" + ((jobNames.ElementAt(jobNames.Count - 1).Equals(name)) ? "" : ", ");
                }

                jobInnerPipeline["name"]       = fullString;
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

                var temp = new List<JobHistory>();

                foreach (JobHistory JobHistory in _jobHistories)
                {
                    if (!jobErrorStatuses.Contains(Constants.JobErrorStatuses[JobHistory.JobStatus]))
                    {
                        temp.Add(JobHistory);
                    }
                }

                foreach (JobHistory JobHistory in temp)
                {
                    _jobHistories.Remove(JobHistory);
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
                foreach (JobHistory jobHistory in _jobHistories)
                {
                    //bemcli returns an int -> get the corresponding string
                    //Might be accounted for in report generation rather than here
                    //jobHistory.JobStatus = Constants.JobErrorStatuses[jobHistory.JobStatus];

                    if (Convert.ToInt32(jobHistory.JobStatus) == Constants.SUCCESSFUL_JOB_STATUS)
                    {
                        filteredJobHistories.Add(jobHistory);
                    }
                }

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
