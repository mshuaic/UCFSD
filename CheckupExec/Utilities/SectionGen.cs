using System.IO;
using System.Web.UI;
using CheckupExec.Models.AnalysisModels;
using System.Collections.Generic;


namespace CheckupExec.Utilities
{
    class SectionGen
    {
        public string GetSection(Driver d)
        {
            StringWriter stringWriter = new StringWriter();
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "margin: auto; width: 80 %; ");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "rows");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "columns");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "item");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.Write("Disk " + d.Name + ": ");
                writer.RenderEndTag();

                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.Write("Free Space: " + d.getFreePer("n2") + "%");
                writer.RenderEndTag();

                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.Write("Used Space: " + d.getUsedPer("n2") + "%");
                writer.RenderEndTag();
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "item");
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "pie");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Id, "line");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();

            }

            return stringWriter.ToString();
        }

        public string middleDisk(Driver d)
        {
            StringWriter stringWriter = new StringWriter();
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "margin: auto; width: 80 %; ");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "rows");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "columns");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "item");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.Write("Disk " + d.Name + ": ");
                writer.RenderEndTag();

                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.Write("Free Space: " + d.getFreePer("n2") + "%");
                writer.RenderEndTag();

                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.Write("Used Space: " + d.getUsedPer("n2") + "%");
                writer.RenderEndTag();
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "item");
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "pie");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Id, "line");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();

            }

            return stringWriter.ToString();
        }

        public string chartDisk(Driver d, List<PlotPoint> points)
        {
            StringWriter stringWriter = new StringWriter();
            stringWriter.Write("\n var trace1 = {x: [");
            PlotPoint va;


            foreach (PlotPoint po in points)
            {
                stringWriter.Write($"'{po.Days}',");
            }
            stringWriter.Write("],y: [");
            foreach (PlotPoint po in points)
            {
                stringWriter.Write($"{po.GB},");
            }
            stringWriter.Write("],mode: 'lines+markers',};var data = [trace1];var layout = {autosize: true,title: 'Free Space on Disk',height:400,width:400,xaxis:{title: 'Days',},yaxis:{title: 'Percent of Disk Space Free'}};");
            stringWriter.Write($"Plotly.newPlot('line', data, layout);");
            return stringWriter.ToString();
        }

        public string middleFore(Driver d)
        {
            StringWriter stringWriter = new StringWriter();
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "margin: auto; width: 80 %; ");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "rows");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "columns");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "item");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.Write("Disk " + d.Name + ": ");
                writer.RenderEndTag();

                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.Write("Gigabytes of Data Protected: " + d.getFreePer("n2") + "Gb");
                writer.RenderEndTag();

                //writer.RenderBeginTag(HtmlTextWriterTag.P);
                //writer.Write("Used Space: " + d.getUsedPer("n2") + "%");
                //writer.RenderEndTag();
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "item");
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "pie");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Id, "line");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();

            }

            return stringWriter.ToString();
        }
        public string chartFore(Driver d, List<PlotPoint> points)
        {
            StringWriter stringWriter = new StringWriter();
            stringWriter.Write("\n var trace1 = {x: [");
            PlotPoint va;


            foreach(PlotPoint po in points)
            {
            stringWriter.Write($"'{po.Days}',");
            }
            stringWriter.Write("],y: [");
            foreach (PlotPoint po in points)
            {
                stringWriter.Write($"{po.GB},");
            }
            stringWriter.Write("],mode: 'lines+markers',};var data = [trace1];var layout = {autosize: true,title: 'Amount of Data under Protection',height:400,width:400,xaxis:{title: 'Days',},yaxis:{title: 'Gigabytes of Data Protected'}};");
            stringWriter.Write($"Plotly.newPlot('line', data, layout);");
            return stringWriter.ToString();
        }


    }
}
