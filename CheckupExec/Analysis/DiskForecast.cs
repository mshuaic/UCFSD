using CheckupExec.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CheckupExec.Analysis
{
    public class DiskForecast
    {
        public ForecastResults ForecastResults { get; set; }

        public DiskForecast(string diskName)
        {

        }

        //same as forecast except the source of data will be the service's file and max=30, min=10
        public void doForecast()
        {
            var diskCapacities = from root in XDocument.Load("../../xml.txt").Elements()
                                 select root;

            foreach (var dc in diskCapacities)
            {
                using (XmlReader reader = dc.CreateReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.Name + ": ");
                        Console.WriteLine("\tValue: " + reader.Value);
                        if (reader.HasAttributes) reader.MoveToAttribute(0);
                        Console.WriteLine("\t" + reader.Name + ": " + reader.GetAttribute(reader.Name));
                    }
                }
            }
        }
    }
}
