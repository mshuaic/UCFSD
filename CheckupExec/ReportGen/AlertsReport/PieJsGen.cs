  using Newtonsoft.Json.Linq;

namespace ReportGen.AlertsReport
{
    class PieJsGen : JsGen
    {
        private const string layout_json = @"{
          autosize: true,
          height: 300,
          width: 300,
          margin: {
            l: 0,
            r: 0,
            b: 0,
            t: 0
          },
          paper_bgcolor: 'rgba(0,0,0,0)',
          plot_bgcolor: 'rgba(0,0,0,0)'
        }";

        private const string data_json = @"{
            type: 'pie',
            showlegend: false,
            hoverinfo: 'label+percent'
        }";
        public PieJsGen(string name) : base(name)
        {
            base.layout = JObject.Parse(layout_json);
        }

        public void SetData(JArray values, JArray labels)
        {
            JObject newData = JObject.Parse(data_json);
            newData.Add(new JProperty("values", values));
            newData.Add(new JProperty("labels", labels));
            base.AddTrace(newData);
        }
    }
}
