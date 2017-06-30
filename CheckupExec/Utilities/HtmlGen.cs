using System;
using System.IO;
using System.Collections.Generic;
using CheckupExec.Models.AnalysisModels;

namespace CheckupExec.Utilities
{
    class HtmlGen
    {
        public List<PlotPoint> points { get; set; }

        public void readTemplate(string path)
        {
            string html = File.ReadAllText(path);

        }

        public void makeReportFore(Driver d, string ty)
        {
            
            string dir = ty;
            //dir = "B:/report.html";
            string html = File.ReadAllText("template.html");
            double space = points[points.Count - 1].GB;
            //int free = space - 100;
            //Driver d = new Driver("C", space, (space-100), 100);
            SectionGen sc = new SectionGen();
            string newHtml = html.Insert(html.IndexOf("<!--START REPORT-->") + "<!--START REPORT-->".Length, sc.middleFore(d));
            newHtml = newHtml.Insert(newHtml.IndexOf("<!--CHARTS AREA-->") + "<!--CHARTS AREA-->".Length, sc.chartFore(d,points));
            System.IO.File.WriteAllText(dir,newHtml);


            //Console.WriteLine(newHtml);

            Console.ReadKey();

        }
        public void makeReportDisk(Driver d, string ty)
        {

            string dir = ty;
            //dir = "B:/report.html";
            string html = File.ReadAllText("template.html");
            double space = points[points.Count - 1].GB;
            //int free = space - 100;
            //Driver d = new Driver("C", space, (space-100), 100);
            SectionGen sc = new SectionGen();
            string newHtml = html.Insert(html.IndexOf("<!--START REPORT-->") + "<!--START REPORT-->".Length, sc.middleDisk(d));
            newHtml = newHtml.Insert(newHtml.IndexOf("<!--CHARTS AREA-->") + "<!--CHARTS AREA-->".Length, sc.chartDisk(d, points));
            System.IO.File.WriteAllText(dir, newHtml);


            //Console.WriteLine(newHtml);

            Console.ReadKey();

        }
    }
}
