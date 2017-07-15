using System;
using System.Collections.Generic;
using CheckupExec.Models;
using CheckupExec.Utilities;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ReportGen.ErrorsReport
{
    class ErrorsReportGen
    {
        private const string START_JS = "/**START MY JAVASCRIPT**/";

        public ErrorsReportGen(string output, List<JobHistory> jobHistory, int numOfTrunk = 10)
        {
            ErrorsReportInfo info = new ErrorsReportInfo(jobHistory, numOfTrunk);
            BarJsGen barJs = new BarJsGen("bar");

            JObject[] traces = new JObject[info.Bars.Count];
            int i = 0;
            foreach(var errorStatus in info.Bars.Keys)
            {
                traces[i++] = barJs.GetNewTrace(new JArray(info.Labels), new JArray(info.Bars[errorStatus]), Constants.JobErrorStatuses[errorStatus]);
            }
            barJs.SetData(traces);

            PieJsGen pieJs = new PieJsGen("pie");
            JArray values = new JArray();
            JArray labels = new JArray();
            foreach(var errorStatus in info.Pie.Keys)
            {
                labels.Add(Constants.JobErrorStatuses[errorStatus]);
                values.Add(info.Pie[errorStatus]);
            }
            pieJs.SetData(values, labels);

            //Console.WriteLine(barJs.Gen());
            //Console.WriteLine(pieJs.Gen());

            //Generate HTML
            try
            {
                string template = CheckupExec.Properties.Resources.template_errors;               
                string html = template.Insert(template.IndexOf(START_JS) + START_JS.Length, barJs.Gen() + pieJs.Gen());
                //Console.WriteLine(html);
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

