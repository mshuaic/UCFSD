﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ErrorReport
{
   public abstract class JsGen
    {
        public JArray data { set; get; }

        public JObject layout { set; get; }

        private const string PLOTLY = "Plotly.newPlot(";

        public string name { set; get; }
        
        public JsGen(string name)
        {
            this.name = name;
            data = new JArray();
        }

        public void AddTrace(JObject trace)
        {
            data.Add(trace);
        }

        public string Gen()
        {
            return PLOTLY + name +","+ data.ToString(Formatting.None)+","+layout.ToString(Formatting.None)+");";
        }
    }
}
