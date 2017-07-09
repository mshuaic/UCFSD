using Newtonsoft.Json.Linq;

namespace ErrorReport
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
            //base.layout = new JObject(
            //    new JProperty("auto", true),
            //    new JProperty("title", "Error Report"),
            //    new JProperty("xaxis", new JObject(new JProperty("tickangle", -45))),
            //    new JProperty("barmode", "group"),
            //    new JProperty("paper_bgcolor", "rgba(0,0,0,0)"),
            //    new JProperty("plot_bgcolor", "rgba(0,0,0,0)"));
            base.layout = JObject.Parse(layout_json);

        }
            
        public JObject GetNewTrace(JArray x, JArray y, string name)
        {
            //JObject newTrace = new JObject(new JProperty("type", "bar"),
            //    new JProperty("name", "Primary Product"),
            //    new JProperty("maker", new JObject(
            //        new JProperty("color", "rgb(49,130,189)"),
            //        new JProperty("opacity", 0.7))));
            JObject newTrace = JObject.Parse(trace_json);
            newTrace.Add(new JProperty("x", x));
            newTrace.Add(new JProperty("y", y));
            newTrace.Add(new JProperty("name", name));
            return newTrace;
        }
    }
}
