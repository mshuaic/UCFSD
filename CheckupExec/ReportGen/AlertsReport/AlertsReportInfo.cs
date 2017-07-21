using System;
using System.Text;
using System.Collections.Generic;
using CheckupExec.Models;

namespace ReportGen.AlertsReport
{
    class AlertsReportInfo
    {
        public int[] Bars { get; }

        public string[] Hovertext { get; }

        public string[] AlertsDetail { get; }

        public Dictionary<string, int> Pie { get; } = new Dictionary<string, int>();

        public string[] Labels { get; }

        public int TotalAlters { get; }

        public AlertsReportInfo(List<Alert> alerts, int numOfTrunk)
        {
            TotalAlters = alerts.Count;

            int elapsedTime = (alerts[alerts.Count - 1].Date - alerts[0].Date).Days + 1;
            double interval = (double)elapsedTime / numOfTrunk;

            if (interval < 1)
                numOfTrunk = elapsedTime;

            Bars = new int[numOfTrunk];
            Hovertext = new string[numOfTrunk];
            AlertsDetail = new string[numOfTrunk];
            Labels = new string[numOfTrunk];

            int numOfAlters = 0;
            AlertInfo hovertext = new AlertInfo();
            int currentTrunk = 0;
            DateTime tempDate = alerts[0].Date;

            foreach (var alert in alerts)
            {
                if(alert.Equals(alerts[alerts.Count-1]))
                {
                    Bars[currentTrunk] = numOfAlters;
                    Hovertext[currentTrunk] = hovertext.ToString();
                    break;
                }
                if (alert.Date > tempDate.AddDays(interval))
                {
                    Bars[currentTrunk] = numOfAlters;
                    Hovertext[currentTrunk] = hovertext.ToString();
                    AlertsDetail[currentTrunk] = hovertext.AlertsDetail;
                    hovertext = new AlertInfo();
                    numOfAlters = 0;
                    while (alert.Date > tempDate.AddDays(interval))
                    {
                        currentTrunk++;
                        tempDate = tempDate.AddDays(interval);
                    }
                }

                numOfAlters++;
                hovertext.Add(alert);

                if (!Pie.ContainsKey(alert.Name))
                    Pie.Add(alert.Name, 1);
                Pie[alert.Name]++;
            }

            // generate labels for bar graph
            DateTime startTime = alerts[0].Date;
            DateTime endTime = startTime.AddDays(interval);
            for (int i = 0; i < numOfTrunk; i++)
            {
                Labels[i] = startTime.ToString("MM/dd") + "-" + endTime.ToString("MM/dd");
                startTime = endTime;
                endTime = startTime.AddDays(interval);
            }

        }

        private class AlertInfo
        {
            private Dictionary<string, int> AlertAndNum { get; set; } = new Dictionary<string, int>();

            public string AlertsDetail { get; set; } = "";

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                string newline = "";
                foreach(var alert in AlertAndNum.Keys)
                {
                    sb.Append(newline);
                    sb.AppendFormat("({0}) {1}", AlertAndNum[alert], alert);
                    newline = "<br>";
                }
                return sb.ToString();
            }

            public void Add(Alert alert)
            {
                string newline = "";
                if(AlertsDetail != "")
                {
                    newline = "<br>";
                }
                AlertsDetail += newline + string.Format("{0}  {1}", alert.Date.ToString("MM/dd"), alert.Name);

                if (AlertAndNum.ContainsKey(alert.Name))
                {
                    AlertAndNum[alert.Name]++;
                }
                else
                {
                    AlertAndNum.Add(alert.Name, 1);
                }
            }
        }
    }
}
