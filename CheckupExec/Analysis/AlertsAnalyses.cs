using CheckupExec.Models;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckupExec.Analysis
{
    public class AlertsAnalyses
    {
        private List<Alert> _allAlerts { get; }

        public bool Successful { get; }

        //All this is doing right now is getting alerts depending on the params passed
        public AlertsAnalyses(DateTime? start = null, DateTime? end = null, List<string> jobNames = null, List<string> alertTypes = null)
        {
            Successful = true;

            start = start ?? DateTime.MinValue;
            end = end ?? DateTime.Now;
            jobNames = jobNames ?? new List<string>();
            alertTypes = alertTypes ?? new List<string>();

            var jobsPipeline = new Dictionary<string, string>();
            var alertsPipeline = new Dictionary<string, string>();

            _allAlerts = new List<Alert>();

            if (jobNames.Count > 0 && alertTypes.Count > 0)
            {
                string fullJobString = jobNames
                                        .Aggregate("", (current, job) => current + ("'" + job + "'" + ((jobNames
                                                                                                       .ElementAt(jobNames.Count - 1)
                                                                                                       .Equals(job)) ? "" : ", ")));

                jobsPipeline["name"] = fullJobString;

                var jobs = DataExtraction.JobController.GetJobs(jobsPipeline);

                try
                {
                    _allAlerts.AddRange(DataExtraction.AlertController.GetAlerts(alertsPipeline));
                    _allAlerts.AddRange(DataExtraction.AlertHistoryController.GetAlertHistories(alertsPipeline));
                }
                catch
                {
                    //log
                }

                var temp = _allAlerts.Where(alert => !alertTypes.Contains(alert.Category)).ToList();

                foreach (Alert alert in temp)
                {
                    _allAlerts.Remove(alert);
                }

                if (_allAlerts.Count > 0)
                {
                    temp = _allAlerts.Where(alert => !jobs.Exists(x => x.Id.Equals(alert.JobId))).ToList();

                    foreach (Alert alert in temp)
                    {
                        _allAlerts.Remove(alert);
                    }
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
                    _allAlerts.AddRange(DataExtraction.AlertController.GetAlerts(jobPipeline));
                    _allAlerts.AddRange(DataExtraction.AlertHistoryController.GetAlertHistories(jobPipeline));
                }
                catch
                {
                    //log
                }
            }
            else if (alertTypes.Count > 0)
            {
                try
                {
                    _allAlerts.AddRange(DataExtraction.AlertController.GetAlerts(alertsPipeline));
                    _allAlerts.AddRange(DataExtraction.AlertHistoryController.GetAlertHistories(alertsPipeline));
                }
                catch
                {
                    //log
                }

                var temp = _allAlerts.Where(alert => !alertTypes.Contains(alert.Category)).ToList();

                foreach (Alert alert in temp)
                {
                    _allAlerts.Remove(alert);
                }
            }
            else
            {
                try
                {
                    _allAlerts.AddRange(DataExtraction.AlertController.GetAlerts());
                    _allAlerts.AddRange(DataExtraction.AlertHistoryController.GetAlertHistories());
                }
                catch
                {
                    //log
                }
            }

            if (_allAlerts.Count > 0)
            {
                SortingUtility<Alert>.Sort(_allAlerts, 0, _allAlerts.Count - 1);

                var filteredAlerts = _allAlerts.Where(alert => alert.Date < start || alert.Date > end).ToList();

                foreach (Alert alert in filteredAlerts)
                {
                    _allAlerts.Remove(alert);
                }

            }
            else
            {
                Successful = false;
            }
        }

        public List<Alert> GetAlerts()
        {
            return _allAlerts;
        }
    }
}
