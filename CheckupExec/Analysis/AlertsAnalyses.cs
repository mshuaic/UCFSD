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
        public List<Alert> allAlerts { get; }

        public AlertsAnalyses(DateTime? start, DateTime? end, string[] jobs = null, string[] alertTypes = null)
        {
            var alertController = new AlertController();
            var alertHistoryController = new AlertHistoryController();

            start = start ?? DateTime.MinValue;
            end = end ?? DateTime.Now;

            var alertsPipeline = new Dictionary<string, string>();

            if (jobs.Length > 0 && alertTypes.Length > 0)
            {
                string fullJobString = "";

                foreach (var job in jobs)
                {
                    fullJobString += job + ((jobs.ElementAt(jobs.Length - 1).Equals(job)) ? "" : ", ");
                }

                alertsPipeline["job"] = fullJobString;

                string fullTypeString = "";

                foreach (var type in alertTypes)
                {
                    fullTypeString += type + ((alertTypes.ElementAt(alertTypes.Length - 1).Equals(type)) ? "" : ", ");
                }

                alertsPipeline["category"] = fullTypeString;

                allAlerts.AddRange((IEnumerable<Alert>)alertController.GetAlerts(alertsPipeline));
                allAlerts.AddRange(alertHistoryController.GetAlertHistories(alertsPipeline));
            }
            else if (jobs.Length > 0)
            {
                string fullJobString = "";

                foreach (var job in jobs)
                {
                    fullJobString += job + ((jobs.ElementAt(jobs.Length - 1).Equals(job)) ? "" : ", ");
                }

                alertsPipeline["job"] = fullJobString;

                allAlerts.AddRange((IEnumerable<Alert>)alertController.GetAlerts(alertsPipeline));
                allAlerts.AddRange(alertHistoryController.GetAlertHistories(alertsPipeline));
            }
            else if (alertTypes.Length > 0)
            {
                string fullTypeString = "";

                foreach (var type in alertTypes)
                {
                    fullTypeString += type + ((alertTypes.ElementAt(alertTypes.Length - 1).Equals(type)) ? "" : ", ");
                }

                alertsPipeline["category"] = fullTypeString;

                allAlerts.AddRange((IEnumerable<Alert>)alertController.GetAlerts(alertsPipeline));
                allAlerts.AddRange(alertHistoryController.GetAlertHistories(alertsPipeline));
            }
            else
            {
                allAlerts.AddRange((IEnumerable<Alert>)alertController.GetAlerts());
                allAlerts.AddRange(alertHistoryController.GetAlertHistories());
            }

            if (!SortingUtility<Alert>.isSorted(allAlerts))
            {
                SortingUtility<Alert>.sort(allAlerts, 0, allAlerts.Count - 1);
            }

            foreach(var alert in allAlerts)
            {
                if (alert.Date < start || alert.Date > end)
                {
                    allAlerts.Remove(alert);
                }
            }
        }
    }
}
