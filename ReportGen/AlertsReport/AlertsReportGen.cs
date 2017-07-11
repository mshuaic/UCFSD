using System;
using System.Collections.Generic;
using CheckupExec.Models;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ReportGen.AlertsReport
{
    class AlertsReportGen
    {
        private const string START_JS = "/**START MY JAVASCRIPT**/";

        public AlertsReportGen(string output, List<Alert> alerts, int numOfTrunk = 10)
        {
            AlertsReportInfo info = new AlertsReportInfo(alerts, numOfTrunk);
            BarJsGen barJs = new BarJsGen("bar");

            barJs.SetData(new JArray(info.Labels), new JArray(info.Bars), new JArray(info.Hovertext));

            PieJsGen pieJs = new PieJsGen("pie");
            JArray values = new JArray();
            JArray labels = new JArray();
            foreach (var alertName in info.Pie.Keys)
            {
                labels.Add(alertName);
                values.Add(info.Pie[alertName]);
            }
            pieJs.SetData(values, labels);

            //Generate HTML
            try
            {
                string template = File.ReadAllText("template.html");
                string html = template.Insert(template.IndexOf(START_JS) + START_JS.Length, barJs.Gen() + pieJs.Gen());
                Console.WriteLine(html);
                File.WriteAllText(output, html);
            }
            catch(IOException e)
            {
                if (e.Source != null)
                    Console.WriteLine("IOException source: {0}", e.Source);
            }

        }
    }
}

