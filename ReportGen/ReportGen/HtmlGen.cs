using System;
using System.IO;

namespace ReportGen
{
    class HtmlGen
    {
        public void readTemplate(string path)
        {
            string html = File.ReadAllText(path);

        }

        static void Main()
        {
            string html = File.ReadAllText("template.html");
            Driver d = new Driver("C", 20, 20, 100);
            SectionGen sc = new SectionGen();
            string newHtml = html.Insert(html.IndexOf("<!--START REPORT-->") + "<!--START REPORT-->".Length, sc.GetSection(d));
            Console.WriteLine(newHtml);
            Console.ReadKey();

        }
    }
}
