using System;
using System.Linq;
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

        private const string MODEL = @"
            var myPlot = document.getElementById('bar');
            var modal = document.getElementById('myModal');
            var span = document.getElementsByClassName('close')[0];

            myPlot.on('plotly_click',function(data){
              var pn='', error = [];
              for(var i=0; i < data.points.length; i++){
                pn = data.points[i].pointNumber;
                error = data.points[i].data.errorMessage;
              };
              modal.style.display = 'block';
              document.getElementById('p1').innerHTML = error[pn];
              span.onclick = function() {
                  modal.style.display = 'none';
              };
            });";

        public ErrorsReportGen(string output, List<JobHistory> jobHistory, int numOfTrunk = 10)
        {
            ErrorsReportInfo info = new ErrorsReportInfo(jobHistory, numOfTrunk);
            BarJsGen barJs = new BarJsGen("bar");

            JObject[] traces = new JObject[info.Bars.Count];
            string[] errorStatusArr = info.Bars.Keys.ToArray();
            for (int i=0;i<info.Bars.Count;i++)
            {
                var errorStatus = errorStatusArr[i];
                if(i != info.Bars.Count-1)
                traces[i] = barJs.GetNewTrace(new JArray(info.Labels), new JArray(info.Bars[errorStatus].Count), new JArray(info.Bars[errorStatus].BuildErrorMessages()), Constants.JobErrorStatuses[errorStatus],"none",null);
                else
                    traces[i] = barJs.GetNewTrace(new JArray(info.Labels), new JArray(info.Bars[errorStatus].Count), new JArray(info.Bars[errorStatus].BuildErrorMessages()), Constants.JobErrorStatuses[errorStatus], "text", new JArray(info.Hovertext));
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
                string html = template.Insert(template.IndexOf(START_JS) + START_JS.Length,"\n" + barJs.Gen() + pieJs.Gen() + MODEL);
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

