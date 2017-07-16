using System;
using System.Xml;
using System.IO;

namespace DiskSpaceTest
{
    class Program
    {
        static string Output = Path.Combine(Directory.GetCurrentDirectory(), "DiskSpaceTest.xml");

        static int Max = 30;

        static int NumOfDisk = 1;

        static int InitTotalSpace = 1024 * 10 * MB2B;

        static int InitUsedSpace = (int)(InitTotalSpace * 0.2); 
        
        static int GrowthRange = 512 * MB2B;

        const int MB2B = 1024;
        static void Main(string[] args)
        {
            if(args.Length != 0)
            {
                Output = args[0];
                Max = int.Parse(args[1]);
                NumOfDisk = int.Parse(args[2]);
                InitTotalSpace = int.Parse(args[3]) * MB2B;
                InitUsedSpace = int.Parse(args[4]) * MB2B;
                GrowthRange = int.Parse(args[5]) * MB2B;
            }

            using (XmlWriter writer = XmlWriter.Create(Output))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("DiskCapacities");
                writer.WriteAttributeString("count", Max.ToString());
                writer.WriteAttributeString("ServerName", System.Environment.MachineName);
                int usedSpace = InitUsedSpace;
                DateTime date = DateTime.Now;
                Random rnd = new Random();   
                for (int i=0;i<Max;i++)
                {
                    logDirveInfo(writer,usedSpace,date);
                    usedSpace += rnd.Next(0, GrowthRange);
                    date = date.AddDays(1);
                }               
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }

        }

        static private void logDirveInfo(XmlWriter writer, int usedSpace, DateTime date)
        {
            writer.WriteStartElement("DiskCapacityInstance");
            writer.WriteAttributeString("Date", date.ToString("yyyy-MM-dd hh:mm:sszzz"));

            for(int i=1;i<=NumOfDisk;i++)
            {
                writer.WriteStartElement("Disk");
                writer.WriteAttributeString("Name", String.Format("Disk storage {0:D4}", i));
                writer.WriteElementString("TotalCapacityBytes", InitTotalSpace.ToString());
                writer.WriteElementString("UsedCapacityBytes", usedSpace.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}
