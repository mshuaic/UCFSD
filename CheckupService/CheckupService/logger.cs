using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Timers;
using CheckupExec.Models;
using System.Collections.Generic;


namespace CheckupService
{
    public class logger
    {
        protected string path; // log path

        protected int max; // max log entries

        protected List<Storage> storages;

        protected ServiceDataExtraction de;

        public logger(string path, int max)
        {
            this.path = path;
            this.max = max;
            de = new ServiceDataExtraction(false);
            // for remote testing only
            // de = new ServiceDataExtraction(true, "VM", "mshuaic", "mshuaic");
            storages = de.GetStorage();
        }
        public void _logger(object source, ElapsedEventArgs e)
        {
            _logger();
        }
        public void _logger()
        {
            // if xml file does not exit, crate xml file frist
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
            // append new log at the end
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode node = doc.SelectSingleNode("/DiskCapacities");
                XPathNavigator navigator = doc.CreateNavigator();

                int count = Int32.Parse(node.Attributes["count"].Value);
                if (count < max)                
                    node.Attributes["count"].Value = (count + 1).ToString();
                else
                {
                    navigator.MoveToChild("DiskCapacities", "");
                    navigator.MoveToFirstChild();
                    navigator.DeleteSelf();
                    navigator.MoveToParent();
                }
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

            foreach (Storage s in storages)
            {
                writer.WriteStartElement("Disk");
                writer.WriteAttributeString("Name", s.Name);
                writer.WriteElementString("TotalCapacityBytes", s.TotalCapacityBytes.ToString());
                writer.WriteElementString("UsedCapacityBytes", s.UsedCapacityBytes.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}
