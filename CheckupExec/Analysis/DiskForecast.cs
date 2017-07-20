using CheckupExec.Models.AnalysisModels;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace CheckupExec.Analysis
{
    public class DiskForecast
    {
        public List<UsedCapacityForecastModel> UsedCapacityForecastModels { get; set; }

        public DiskForecast(List<string> diskNames, string path)
        {
            UsedCapacityForecastModels = new List<UsedCapacityForecastModel>();

            diskNames = diskNames ?? new List<string>();
            IEnumerable<XElement> diskCapacities = new List<XElement>();

            try
            {
                diskCapacities = from root in XDocument.Load(path).Elements()
                                 select root;
            }
            catch (Exception e)
            {
                //ignore for now
            }

            int count = 0;
            bool continueFlag = true;

            using (XmlReader reader = diskCapacities.First().CreateReader())
            {
                try
                {
                    reader.Read();

                    reader.MoveToAttribute("count");
                    count = Int32.Parse(reader.Value);

                    reader.MoveToAttribute("ServerName");
                }
                catch
                {
                    continueFlag = false;
                }
            }

            if (continueFlag)
            {
                //diskCapacities.First().Remove();

                foreach (XElement dc in diskCapacities)
                {
                    using (XmlReader reader = dc.CreateReader())
                    {
                        DateTime date = DateTime.Now.Date;
                        long totalBytes = 0;
                        long usedBytes = 0;
                        string storageName = "";

                        while (reader.Read())
                        {
                            switch (reader.Name)
                            {
                                case "DiskCapacityInstance":
                                    reader.MoveToAttribute("Date");
                                    try
                                    {
                                        date = DateTime.Parse(reader.Value);
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                    break;
                                //Called disk but really refers to storagedevices in backupexec
                                case "Disk":
                                    reader.MoveToAttribute("Name");
                                    storageName = reader.Value;
                                    if (!string.IsNullOrWhiteSpace(storageName) && !UsedCapacityForecastModels.Exists(x => x.StorageName.Equals(storageName)))
                                    {
                                        UsedCapacityForecastModels.Add(new UsedCapacityForecastModel
                                        {
                                            StorageName = storageName,
                                            UsedCapacityInstances = new List<UsedCapacity>()
                                        });
                                    }
                                    break;
                                case "TotalCapacityBytes":
                                    try
                                    {
                                        reader.MoveToContent();
                                        totalBytes = long.Parse(reader.ReadString());
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                    //totalBytes = reader.ReadElementContentAsLong();
                                    break;
                                case "UsedCapacityBytes":
                                    try
                                    {
                                        reader.MoveToContent();
                                        usedBytes = long.Parse(reader.ReadString());
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                    //var usedBytes = reader.ReadElementContentAsLong();
                                    var ucm = UsedCapacityForecastModels.Find(x => x.StorageName.Equals(storageName));
                                    if (usedBytes > 0)
                                    {
                                        ucm.UsedCapacityInstances.Add(new UsedCapacity
                                        {
                                            Bytes = usedBytes,
                                            Date = date
                                        });
                                        ucm.TotalCapacity = totalBytes;
                                    }
                                    break;
                            }

                        }
                    }
                }

                var fc = new Forecast<UsedCapacity>();

                foreach (UsedCapacityForecastModel model in UsedCapacityForecastModels)
                {
                    if (diskNames.Count == 0 || diskNames.Contains(model.StorageName))
                    {
                        model.ForecastResults = fc.doForecast(model.UsedCapacityInstances);
                    }
                }
            }
        }
    }
}

