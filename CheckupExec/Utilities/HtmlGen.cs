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

        public void makeReportFore(List<FrontEndCapacityReport> reps, string loc)
        {
            //dir = "B:/report.html";
            string html = File.ReadAllText("templateFrontEnd.html");

            foreach(FrontEndCapacityReport sinRep in reps){

            }
            //double space = sinRep.HistoricalPoints[HistoricalPoints.Count - 1].GB;
            //int free = space - 100;
            //Driver d = new Driver("C", space, (space-100), 100);
            SectionGen sc = new SectionGen();
            string newHtml = html.Insert(html.IndexOf("<!--START REPORT-->") + "<!--START REPORT-->".Length, sc.middleFore(reps));
            newHtml = newHtml.Insert(newHtml.IndexOf("<!--CHARTS AREA-->") + "<!--CHARTS AREA-->".Length, sc.chartFore(reps));
            System.IO.File.WriteAllText(loc,newHtml);


            //Console.WriteLine(newHtml);

            Console.ReadKey();

        }
        public void makeReportDisk(List<DiskCapacityReport> reps, string loc)
        {

            //dir = "B:/report.html";
            string html = File.ReadAllText("template.html");
            //double space = points[points.Count - 1].GB;
            //int free = space - 100;
            //Driver d = new Driver("C", space, (space-100), 100);
            SectionGen sc = new SectionGen();
            string newHtml = html.Insert(html.IndexOf("<!--START REPORT-->") + "<!--START REPORT-->".Length, sc.middleDisk(reps));
            newHtml = newHtml.Insert(newHtml.IndexOf("<!--CHARTS AREA-->") + "<!--CHARTS AREA-->".Length, sc.chartDisk(reps));
            System.IO.File.WriteAllText(loc, newHtml);


            //Console.WriteLine(newHtml);

            Console.ReadKey();

        }
        public void makeReportJob(List<BackupJobReport> reps, string loc) {

            string html = File.ReadAllText("template.html");
            SectionGen sc = new SectionGen();
            string newHtml = html.Insert(html.IndexOf("<!--START REPORT-->") + "<!--START REPORT-->".Length, sc.middleDisk(reps));
            newHtml = newHtml.Insert(newHtml.IndexOf("<!--CHARTS AREA-->") + "<!--CHARTS AREA-->".Length, sc.chartDisk(reps));
            System.IO.File.WriteAllText(loc, newHtml);

            Console.ReadKey();



        }
    }
}
