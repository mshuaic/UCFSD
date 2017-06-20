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

        public AlertsAnalyses()
        {
            var alertController = new AlertController();
            var alertHistoryController = new AlertHistoryController();

            allAlerts.AddRange((IEnumerable<Alert>)alertController.GetAlerts());
            allAlerts.AddRange((IEnumerable<Alert>)alertHistoryController.GetAlertHistories());

            if (!SortingUtility<Alert>.isSorted(allAlerts))
            {
                SortingUtility<Alert>.sort(allAlerts, 0, allAlerts.Count - 1);
            }
        }

        public AlertsAnalyses(DateTime? start, DateTime? end)
        {
            var alertController = new AlertController();
            var alertHistoryController = new AlertHistoryController();

            start = start ?? DateTime.MinValue;
            end = end ?? DateTime.Now;

            allAlerts.AddRange((IEnumerable<Alert>)alertController.GetAlerts());
            allAlerts.AddRange((IEnumerable<Alert>)alertHistoryController.GetAlertHistories());

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
