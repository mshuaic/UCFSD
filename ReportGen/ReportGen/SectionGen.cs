using System;
using System.IO;
using System.Web.UI;


namespace ReportGen
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

                writer.AddAttribute(HtmlTextWriterAttribute.Id, "bar");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();

            }

            return stringWriter.ToString();
        }


    }
}
