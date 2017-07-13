using System;
using Newtonsoft.Json.Linq;

namespace ReportGen.ErrorsReport
{
    public class BarJsGen : JsGen
    {
        private const string layout_json = @"{
            autosize: true,
            title: 'Error Report',
            xaxis: {
            tickangle: -45
            },
            barmode: 'stack',
            paper_bgcolor: 'rgba(0,0,0,0)',
            plot_bgcolor: 'rgba(0,0,0,0)'
        }";

        private const string trace_json = @"{  
            type: 'bar'
        }";

        public BarJsGen(string name) : base(name)
        {
            base.layout = JObject.Parse(layout_json);

        }

        public JObject GetNewTrace(JArray x, JArray y, string name)
        {
            JObject newTrace = JObject.Parse(trace_json);
            newTrace.Add(new JProperty("x", x));
            newTrace.Add(new JProperty("y", y));
            newTrace.Add(new JProperty("name", name));
            return newTrace;
        }
    }
}
