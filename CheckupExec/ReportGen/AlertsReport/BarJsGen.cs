using Newtonsoft.Json.Linq;

namespace ReportGen.AlertsReport
{
    public class BarJsGen : JsGen
    {
        private const string layout_json = @"{
            autosize: true,
            title: 'Alerts Report',
            xaxis: {
            tickangle: -45
            },
            barmode: 'stack',
            paper_bgcolor: 'rgba(0,0,0,0)',
            plot_bgcolor: 'rgba(0,0,0,0)'
        }";

        private const string trace_json = @"{  
            type: 'bar',
            hoverinfo: 'text'
        }";

        public BarJsGen(string name) : base(name)
        {
            base.layout = JObject.Parse(layout_json);
        }

        public void SetData(JArray x, JArray y, JArray hovertext, JArray alertsDetail)
        {
            JObject newTrace = JObject.Parse(trace_json);
            newTrace.Add(new JProperty("x", x));
            newTrace.Add(new JProperty("y", y));
            newTrace.Add(new JProperty("hovertext", hovertext));
            newTrace.Add(new JProperty("alertsDetail", alertsDetail));
            AddTrace(newTrace);
        }
    }
}
