using System;
using System.Xml;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CheckupExec;
using CheckupExec.Models;


namespace DiskSpaceTest
{
    class Program
    {
        static string Output = Path.Combine(Directory.GetCurrentDirectory(), "DiskSpaceTest.xml");

        static int Max = 30;
        
        static int GrowthRange = 1024 * MB2B;

        const int MB2B = 1024;
        static void Main(string[] args)
        {
            if(args.Length == 1)
            {
                GrowthRange = int.Parse(args[0]) * MB2B;
            }

            if(args.Length == 3)
            {
                Output = args[0];
                Max = int.Parse(args[1]);
                GrowthRange = int.Parse(args[2]) * MB2B;
            }
            
            // remote
            //var de = new BEStorage(true, "mshuaic", "VM", "mshuaic");

            // local
             var de = new BEStorage(false);
            List<Storage> storages =  de.GetStorages();

            using (XmlWriter writer = XmlWriter.Create(Output))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("DiskCapacities");
                writer.WriteAttributeString("count", Max.ToString());
                writer.WriteAttributeString("ServerName", System.Environment.MachineName);
                DateTime date = DateTime.Now;
                Random rnd = new Random();
                List<long> usedSpace = storages.Select(x => x.UsedCapacityBytes).ToList();

                for (int i=0;i<Max;i++)
                {
                    
                    writer.WriteStartElement("DiskCapacityInstance");
                    writer.WriteAttributeString("Date", date.ToString("yyyy-MM-dd hh:mm:sszzz"));
                    for(int j=0;j<storages.Count;j++)
                    {
                        writer.WriteStartElement("Disk");
                        writer.WriteAttributeString("Name", storages[j].Name);
                        writer.WriteElementString("TotalCapacityBytes", storages[j].TotalCapacityBytes.ToString());
                        writer.WriteElementString("UsedCapacityBytes", usedSpace[j].ToString());
                        writer.WriteEndElement();
                        if (storages[j].Name == "Any disk storage")
                            continue;
                        usedSpace[j] += rnd.Next(0, GrowthRange);
                    }
                    writer.WriteEndElement();
                    date = date.AddDays(1);
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }

        }

        private class BEStorage : DataExtraction
        {
            public BEStorage(bool isRemoteUser, string password = null, string serverName = null, string userName = null)
            : base(isRemoteUser, password, serverName, userName)
            { }

            public List<Storage> GetStorages()
            {
                return StorageController.GetStorages();
            }
        }
    }


}
