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

        public AlertsAnalyses(DateTime? start, DateTime? end, List<string> jobNames = null, List<string> alertTypes = null)
        {
            var alertController = new AlertController();
            var alertHistoryController = new AlertHistoryController();
            var jobController = new JobController();

            start = start ?? DateTime.MinValue;
            end = end ?? DateTime.Now;

            var jobsPipeline = new Dictionary<string, string>();
            var alertsPipeline = new Dictionary<string, string>();

            if (jobNames.Count > 0 && alertTypes.Count > 0)
            {
                string fullJobString = "";

                foreach (var job in jobNames)
                {
                    fullJobString += job + ((jobNames.ElementAt(jobNames.Count - 1).Equals(job)) ? "" : ", ");
                }

                jobsPipeline["name"] = fullJobString;

                var jobs = jobController.GetJobs(jobsPipeline);

                string fullTypeString = "";

                foreach (var type in alertTypes)
                {
                    fullTypeString += type + ((alertTypes.ElementAt(alertTypes.Count - 1).Equals(type)) ? "" : ", ");
                }

                alertsPipeline["category"] = fullTypeString;

                _allAlerts.AddRange(alertController.GetAlerts(alertsPipeline));
                _allAlerts.AddRange(alertHistoryController.GetAlertHistories(alertsPipeline));

                foreach (var alert in _allAlerts)
                {
                    if (!jobs.Exists(x => x.Id.Equals(alert.JobId)))
                    {
                        _allAlerts.Remove(alert);
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
                    fullString += name + ((jobNames.ElementAt(jobNames.Count - 1).Equals(name)) ? "" : ", ");
                }

                jobInnerPipeline["name"] = fullString;
                jobPipeline["get-bejob"] = jobInnerPipeline;

                _allAlerts.AddRange(alertController.GetAlerts(jobPipeline));
                _allAlerts.AddRange(alertHistoryController.GetAlertHistories(jobPipeline));
            }
            else if (alertTypes.Count > 0)
            {
                string fullTypeString = "";

                foreach (var type in alertTypes)
                {
                    fullTypeString += type + ((alertTypes.ElementAt(alertTypes.Count - 1).Equals(type)) ? "" : ", ");
                }

                alertsPipeline["category"] = fullTypeString;

                _allAlerts.AddRange(alertController.GetAlerts(alertsPipeline));
                _allAlerts.AddRange(alertHistoryController.GetAlertHistories(alertsPipeline));
            }
            else
            {
                _allAlerts.AddRange(alertController.GetAlerts());
                _allAlerts.AddRange(alertHistoryController.GetAlertHistories());
            }

            if (!SortingUtility<Alert>.isSorted(_allAlerts))
            {
                SortingUtility<Alert>.sort(_allAlerts, 0, _allAlerts.Count - 1);
            }

            foreach(var alert in _allAlerts)
            {
                if (alert.Date < start || alert.Date > end)
                {
                    _allAlerts.Remove(alert);
                }
            }
        }
    }
}
