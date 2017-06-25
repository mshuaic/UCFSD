using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Timers;


namespace CheckupService
{
    class logger
    {
        DriveInfo[] allDrives = DriveInfo.GetDrives();
        string path;

        public logger(string path)
        {
            this.path = path;
        }
        public void _logger(object source, ElapsedEventArgs e)
        {
            _logger();
        }
        public void _logger()
        {

            if (File.Exists(path) == false)
            {
                using (XmlWriter writer = XmlWriter.Create(path))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("DiskCapacities");
                    writer.WriteAttributeString("count", "1");
                    writer.WriteAttributeString("ServerName", System.Environment.MachineName);
                    logDirveInfo(writer);
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Close();
                }
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode node = doc.SelectSingleNode("/DiskCapacities");
                node.Attributes["count"].Value = (Int32.Parse(node.Attributes["count"].Value)+1).ToString();

                XPathNavigator navigator = doc.CreateNavigator();
                navigator.MoveToChild("DiskCapacities","");
                using  (XmlWriter writer = navigator.AppendChild())
                {
                    logDirveInfo(writer);
                    writer.Close();
                }               
                doc.Save(path);
            }

        }

        private void logDirveInfo(XmlWriter writer)
        {
            writer.WriteStartElement("DiskCapacityInstance");
            writer.WriteAttributeString("Date", DateTime.Now.ToString("yyyy-MM-dd hh:mm:sszzz"));

            foreach (DriveInfo d in allDrives)
            {
                writer.WriteStartElement("Disk");
                writer.WriteAttributeString("Name", d.Name);
                writer.WriteAttributeString("VolumeLabel", d.VolumeLabel);
                writer.WriteElementString("AvailableFreeSpace", d.AvailableFreeSpace.ToString());
                writer.WriteElementString("TotalFreeSpace", d.TotalFreeSpace.ToString());
                writer.WriteElementString("TotalSize", d.TotalSize.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}
