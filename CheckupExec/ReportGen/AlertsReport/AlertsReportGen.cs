using CheckupExec.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReportGen.AlertsReport
{
    class AlertsReportGen
    {
        private const string START_JS = "/**START MY JAVASCRIPT**/";

        private const string MODEL = @"
            var myPlot = document.getElementById('bar');
            var modal = document.getElementById('myModal');
            var span = document.getElementsByClassName('close')[0];

            myPlot.on('plotly_click',function(data){
              var pn='', alertsDetail = [];
              for(var i=0; i < data.points.length; i++){
                pn = data.points[i].pointNumber;
                alertsDetail = data.points[i].data.alertsDetail;
              };
              modal.style.display = 'block';
              document.getElementById('p1').innerHTML = alertsDetail[pn];
              span.onclick = function() {
                  modal.style.display = 'none';
              };
            });";

        public AlertsReportGen(string output, List<Alert> alerts, int numOfTrunk = 10)
        {
            AlertsReportInfo info = new AlertsReportInfo(alerts, numOfTrunk);
            BarJsGen barJs = new BarJsGen("bar");

            JObject[] traces = new JObject[info.Bars.Count];
            string[] alertArr = info.Bars.Keys.ToArray();
            for (int i = 0; i < info.Bars.Count; i++)
            {
                var alert= alertArr[i];
                if (i != info.Bars.Count - 1)
                    traces[i] = barJs.GetNewTrace(new JArray(info.Labels), new JArray(info.Bars[alert].Count), null, alert, "none", null);
                else
                    traces[i] = barJs.GetNewTrace(new JArray(info.Labels), new JArray(info.Bars[alert].Count), new JArray(info.AlertsDetail), alert, "text", new JArray(info.Hovertext));
            }

            barJs.SetData(traces);

            PieJsGen pieJs = new PieJsGen("pie");
            JArray values = new JArray();
            JArray labels = new JArray();
            foreach (var alertName in info.Pie.Keys)
            {
                labels.Add(alertName);
                values.Add(info.Pie[alertName]);
            }
            pieJs.SetData(values, labels);



            //barJs.SetData(new JArray(info.Labels), new JArray(info.Bars), new JArray(info.Hovertext), new JArray(info.AlertsDetail));

            //PieJsGen pieJs = new PieJsGen("pie");
            //JArray values = new JArray();
            //JArray labels = new JArray();
            //foreach (var alertName in info.Pie.Keys)
            //{
            //    labels.Add(alertName);
            //    values.Add(info.Pie[alertName]);
            //}
            //pieJs.SetData(values, labels);

            //Generate HTML
            try
            {
                string template = CheckupExec.Properties.Resources.template_alerts;
                string html = template.Insert(template.IndexOf(START_JS) + START_JS.Length, "\n" + barJs.Gen() + pieJs.Gen()+ MODEL);
                //Console.WriteLine(html);
                File.WriteAllText(output, html);
            }
            catch (IOException e)
            {
                if (e.Source != null)
                    Console.WriteLine("IOException source: {0}", e.Source);
            }

        }
    }
}

