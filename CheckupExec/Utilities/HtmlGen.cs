using System;
using System.IO;
using System.Collections.Generic;
using CheckupExec.Models.AnalysisModels;
using CheckupExec.Models.ReportModels;

namespace CheckupExec.Utilities
{
    class HtmlGen
    {
        

        public void readTemplate(string path)
        {
            string html = File.ReadAllText(path);
        }

        public Boolean makeReportFore(List<FrontEndCapacityReport> reps, string loc)
        {
            string html = File.ReadAllText("templateFrontEnd.html");
            foreach(FrontEndCapacityReport sinRep in reps){

            }
            SectionGen sc = new SectionGen();
            string newHtml = html.Insert(html.IndexOf("<!--START REPORT-->") + "<!--START REPORT-->".Length, sc.middleFore(reps));
            newHtml = newHtml.Insert(newHtml.IndexOf("<!--CHARTS AREA-->") + "<!--CHARTS AREA-->".Length, sc.chartFore(reps));
            System.IO.File.WriteAllText(loc,newHtml);
            return true;
        }
        public Boolean makeReportDisk(List<DiskCapacityReport> reps, string loc)
        {
            string html = File.ReadAllText("templateDisk.html");
            SectionGen sc = new SectionGen();
            string newHtml = html.Insert(html.IndexOf("<!--START REPORT-->") + "<!--START REPORT-->".Length, sc.middleDisk(reps));
            newHtml = newHtml.Insert(newHtml.IndexOf("<!--CHARTS AREA-->") + "<!--CHARTS AREA-->".Length, sc.chartDisk(reps));
            System.IO.File.WriteAllText(loc, newHtml);
            return true;
        }
        public Boolean makeReportJob(List<BackupJobReport> reps, string loc) {

            string html = File.ReadAllText("templateJob.html");
            SectionGen sc = new SectionGen();
            string newHtml = html.Insert(html.IndexOf("<!--START REPORT-->") + "<!--START REPORT-->".Length, sc.middleJob(reps));
            newHtml = newHtml.Insert(newHtml.IndexOf("<!--CHARTS AREA-->") + "<!--CHARTS AREA-->".Length, sc.chartJob(reps));
            System.IO.File.WriteAllText(loc, newHtml);
            return true;
        }
    }
}
