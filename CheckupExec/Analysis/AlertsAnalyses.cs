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
    //I think we need to filter by date manually for alerts
    public class AlertsAnalyses
    {
        private List<Alert> _allAlerts { get; }

        public bool Successful { get; }

        public AlertsAnalyses(DateTime? start = null, DateTime? end = null, List<string> jobNames = null, List<string> alertTypes = null)
        {
            start = start ?? DateTime.MinValue;
            end = end ?? DateTime.Now;
            jobNames = jobNames ?? new List<string>();
            alertTypes = alertTypes ?? new List<string>();

            var jobsPipeline = new Dictionary<string, string>();
            var alertsPipeline = new Dictionary<string, string>();

            _allAlerts = new List<Alert>();

            if (jobNames.Count > 0 && alertTypes.Count > 0)
            {
                string fullJobString = "";

                foreach (var job in jobNames)
                {
                    fullJobString += "'" + job + "'" + ((jobNames.ElementAt(jobNames.Count - 1).Equals(job)) ? "" : ", ");
                }

                jobsPipeline["name"] = fullJobString;

                var jobs = DataExtraction.JobController.GetJobs(jobsPipeline);

                string fullTypeString = "";

                foreach (var type in alertTypes)
                {
                    fullTypeString += "'" + type + "'" + ((alertTypes.ElementAt(alertTypes.Count - 1).Equals(type)) ? "" : ", ");
                }

                alertsPipeline["category"] = fullTypeString;

                try
                {
                    _allAlerts.AddRange(DataExtraction.AlertController.GetAlerts(alertsPipeline));
                    _allAlerts.AddRange(DataExtraction.AlertHistoryController.GetAlertHistories(alertsPipeline));
                }
                catch
                {
                    //log
                }

                if (_allAlerts.Count > 0)
                {
                    foreach (var alert in _allAlerts)
                    {
                        if (!jobs.Exists(x => x.Id.Equals(alert.JobId)))
                        {
                            _allAlerts.Remove(alert);
                        }
                    }
                }
            }
            else if (jobNames.Count > 0)
            {
                var jobPipeline = new Dictionary<string, Dictionary<string, string>>();
                var jobInnerPipeline = new Dictionary<string, string>();

                string fullString = "";

                foreach (var name in jobNames)
                {
                    fullString += "'" + name + "'" + ((jobNames.ElementAt(jobNames.Count - 1).Equals(name)) ? "" : ", ");
                }

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
                string fullTypeString = "";

                foreach (var type in alertTypes)
                {
                    fullTypeString += "'" + type + "'" + ((alertTypes.ElementAt(alertTypes.Count - 1).Equals(type)) ? "" : ", ");
                }

                alertsPipeline["category"] = fullTypeString;

                try
                {
                    _allAlerts.AddRange(DataExtraction.AlertController.GetAlerts(alertsPipeline));
                    _allAlerts.AddRange(DataExtraction.AlertHistoryController.GetAlertHistories(alertsPipeline));
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
                SortingUtility<Alert>.sort(_allAlerts, 0, _allAlerts.Count - 1);   

                foreach (var alert in _allAlerts)
                {
                    if (alert.Date < start || alert.Date > end)
                    {
                        _allAlerts.Remove(alert);
                    }
                }
            }
        }
    }
}
