using CheckupExec.Models;
using CheckupExec.Models.AnalysisModels;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CheckupExec.Analysis
{
    //to-do......
    //same as forecast except the source of data will be the service's file and max=30, min=10
    public class DiskForecast
    {
        public List<DiskForecastModel> DiskForecastModels { get; set; }

        public DiskForecast(List<string> diskNames, string path)
        {
            DiskForecastModels = new List<DiskForecastModel>();

            IEnumerable<XElement> diskCapacities = from root in XDocument.Load(path).Elements()
                                                   select root;

            int count = 0;

            using (XmlReader reader = diskCapacities.First().CreateReader())
            {
                reader.Read();

                reader.MoveToAttribute("count");

                count = Int32.Parse(reader.Value);
            }

            diskCapacities.First().Remove();

            foreach (var dc in diskCapacities)
            {
                using (XmlReader reader = dc.CreateReader())
                {
                    DateTime date = DateTime.Now.Date;
                    long availableBytes = 0;
                    long totalBytes = 0;
                    string diskName = "";

                    while (reader.Read())
                    {
                        switch (reader.Name)
                        {
                            case "DiskCapacityInstance":
                                reader.MoveToAttribute("Date");
                                date = DateTime.Parse(reader.Value);
                                break;
                            case "Disk":
                                reader.MoveToAttribute("Name");
                                diskName = reader.Value;
                                if (!DiskForecastModels.Exists(x => x.DiskName.Equals(diskName)))
                                {
                                    DiskForecastModels.Add(new DiskForecastModel
                                    {
                                        DiskName = diskName,
                                        DiskInstances = new List<DiskCapacity>()
                                    });

                                }
                                break;
                            case "AvailableFreeSpace":
                                availableBytes = long.Parse(reader.Value);
                                break;
                            case "TotalSize":
                                totalBytes = long.Parse(reader.Value);
                                var dfm = DiskForecastModels.ElementAt(DiskForecastModels.FindIndex(x => x.DiskName.Equals(diskName)));
                                dfm.DiskInstances.Add(new DiskCapacity
                                {
                                    Bytes = totalBytes - availableBytes,
                                    Date = date
                                });
                                dfm.TotalCapacity = totalBytes;
                                break;
                            default:
                                break;
                        }

                    }
                }
            }

            var fc = new Forecast<DiskCapacity>();

            foreach (DiskForecastModel model in DiskForecastModels)
            {
                model.ForecastResults = fc.doForecast(model.DiskInstances);
            }
        }
    }
}

