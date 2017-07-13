using System.IO;
using System.Web.UI;
using CheckupExec.Models.ReportModels;
using CheckupExec.Models.AnalysisModels;
using System.Collections.Generic;


namespace CheckupExec.Utilities
{
    class SectionGen
    {
        public string GetSection(List<FrontEndCapacityReport> reps)
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
                //writer.Write("Disk " + d.Name + ": ");
                //writer.RenderEndTag();

                writer.RenderBeginTag(HtmlTextWriterTag.P);
                //writer.Write("Free Space: " + d.getFreePer("n2") + "%");
                //writer.RenderEndTag();

                writer.RenderBeginTag(HtmlTextWriterTag.P);
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

        public string middleDisk(List<DiskCapacityReport> reps)
        {
            StringWriter stringWriter = new StringWriter();
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                int i = 0;
                foreach (DiskCapacityReport rep in reps)
                {
                    i++;
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "margin: auto; width: 80 %; ");
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);

                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "rows");
                            writer.RenderBeginTag(HtmlTextWriterTag.Div);

                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "columns");
                                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                                writer.AddAttribute(HtmlTextWriterAttribute.Class, "item");
                                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                                writer.RenderBeginTag(HtmlTextWriterTag.P);
                                writer.Write("Diskname: " + rep.DiskName);
                                writer.RenderEndTag();

                                writer.RenderBeginTag(HtmlTextWriterTag.P);
                                writer.Write("Maximum Capacity: "+rep.MaxCapacity+"GB");
                                writer.RenderEndTag();

                                writer.RenderBeginTag(HtmlTextWriterTag.P);
                                writer.Write("Used Capacity: " + rep.UsedCapacity + "GB");
                                writer.RenderEndTag();

                                writer.RenderBeginTag(HtmlTextWriterTag.P);
                                writer.Write("50% Capacity: " + rep.DaysTo50 + " Days");
                                writer.RenderEndTag();

                                writer.RenderBeginTag(HtmlTextWriterTag.P);
                                writer.Write("75% Capacity: " + rep.DaysTo75 + " Days");
                                writer.RenderEndTag();

                                writer.RenderBeginTag(HtmlTextWriterTag.P);
                                writer.Write("90% Capacity: " + rep.DaysTo90 + " Days");
                                writer.RenderEndTag();

                                writer.RenderBeginTag(HtmlTextWriterTag.P);
                                writer.Write("100% Capacity: " + rep.DaysToFull + " Days");
                                writer.RenderEndTag();


                                writer.RenderBeginTag(HtmlTextWriterTag.P);
                                //writer.Write("Free Space: " + d.getFreePer("n2") + "%");
                                writer.RenderEndTag();

                                writer.RenderBeginTag(HtmlTextWriterTag.P);
                                //writer.Write("Used Space: " + d.getUsedPer("n2") + "%");
                                writer.RenderEndTag();
                            writer.RenderEndTag();

                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "item");
                                writer.AddAttribute(HtmlTextWriterAttribute.Id, "pie");
                                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                                writer.RenderEndTag();
                            writer.RenderEndTag();

                            writer.AddAttribute(HtmlTextWriterAttribute.Id, $"histTrace{i}");
                            writer.RenderBeginTag(HtmlTextWriterTag.Div);
                            writer.RenderEndTag();
                            writer.AddAttribute(HtmlTextWriterAttribute.Id, $"futTrace{i}");
                            writer.RenderBeginTag(HtmlTextWriterTag.Div);
                            writer.RenderEndTag();

                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }

            }

            return stringWriter.ToString();
        }
        public string chartDisk(List<DiskCapacityReport> reps)
        {
            StringWriter stringWriter = new StringWriter();
            int i = 0;
            foreach (DiskCapacityReport rep in reps)
            {
                i++;
                //List<PlotPoints> histPoints = rep.HistoricalPoints;
                //List<PlotPoint> futurePoints = rep.ForecastPoints;

                stringWriter.Write($"\nvar histTrace{i}");
                stringWriter.Write(" = {x: [");
                PlotPoint va;

                foreach (PlotPoint po in rep.HistoricalPoints)
                {
                    stringWriter.Write($"'{po.Days}',");
                }
                stringWriter.Write("], \ny: [");
                foreach (PlotPoint po in rep.HistoricalPoints)
                {
                    stringWriter.Write($"{po.GB},");
                }
                stringWriter.Write("]\n,mode: 'lines+markers',marker:{color: 'rgb(179,4,16)'}};var dataHist");
                stringWriter.Write($"{i}= [histTrace{i}];\nvar layout = ");
                stringWriter.Write("{autosize: true,title: 'Space Usage on Disk',height:400,width:400,\n xaxis:{title: 'Days',},yaxis:{title: 'Used space on disk(GB)'}};");
                stringWriter.Write($"\nPlotly.newPlot('histTrace{i}', dataHist{i}, layout);");


                stringWriter.Write($"\n\nvar futTrace{i}");
                stringWriter.Write(" = {x: [");
                //PlotPoint va;

                foreach (PlotPoint po in rep.ForecastPoints)
                {
                    stringWriter.Write($"'{po.Days}',");
                }
                stringWriter.Write("], \ny: [");
                foreach (PlotPoint po in rep.ForecastPoints)
                {
                    stringWriter.Write($"{po.GB},");
                }
                stringWriter.Write("]\n,mode: 'lines+markers',marker:{color: 'rgb(179,4,16)'}};var dataFut");
                stringWriter.Write($"{i}= [futTrace{i}];\nvar layout = ");
                stringWriter.Write("{autosize: true,title: 'Projected Space Usage on Disk',height:400,width:400,\n xaxis:{title: 'Days',},yaxis:{title: 'Used space on disk(GB)'}};");
                stringWriter.Write($"\nPlotly.newPlot('futTrace{i}', dataFut{i}, layout);");
            }
            return stringWriter.ToString();
        }

        public string middleFore(List<FrontEndCapacityReport> reps)
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
                //writer.Write("Disk " + d.Name + ": ");
                //writer.RenderEndTag();

                writer.RenderBeginTag(HtmlTextWriterTag.P);
                //writer.Write("Gigabytes of Data Protected: " + d.getFreePer("n2") + "Gb");
                //writer.RenderEndTag();

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
        public string chartFore(List<FrontEndCapacityReport> reps)
        {
            StringWriter stringWriter = new StringWriter();

            foreach (FrontEndCapacityReport rep in reps)
            {
                stringWriter.Write("\n var trace1 = {x: [");
                PlotPoint va;


                foreach (PlotPoint po in rep.HistoricalPoints)
                {
                    stringWriter.Write($"'{po.Days}',");
                }
                stringWriter.Write("],y: [");
                foreach (PlotPoint po in rep.HistoricalPoints)
                {
                    stringWriter.Write($"{po.GB},");
                }
                stringWriter.Write("],mode: 'lines+markers',};var data = [trace1];var layout = {autosize: true,title: 'Amount of Data under Protection',height:400,width:400,xaxis:{title: 'Days',},yaxis:{title: 'Gigabytes of Data Protected'}};");
                stringWriter.Write($"Plotly.newPlot('line', data, layout);");
            }
            return stringWriter.ToString();
        }

        public string middleJob(List<BackupJobReport> reps) {

            return "";
        }
        public string chartJob(List<BackupJobReport> reps)
        {

            return "";
        }

    }
}
