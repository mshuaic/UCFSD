using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using CheckupExec.Models;
using CheckupExec.Utilities;

namespace ReportGen.ErrorsReport
{
    class ErrorsReportInfo
    {
        public Dictionary<string, BarInfo> Bars { get; }

        public Dictionary<string, int> Pie { get; }

        public List<string> Labels { get; }

        public int TotalErrors { get; }

        public ErrorsReportInfo(List<JobHistory> jobHistory, int numOfTrunk)
        {
            Bars = new Dictionary<string, BarInfo>();
            Pie = new Dictionary<string, int>();
            Labels = new List<string>();
            TotalErrors = jobHistory.Count;

            int elapsedTime = (jobHistory[jobHistory.Count - 1].EndTime - jobHistory[0].EndTime).Days + 1;
            DateTime tempDate = jobHistory[0].EndTime;
            double interval = (double)elapsedTime / numOfTrunk;

            if (interval < 1)
                numOfTrunk = elapsedTime;
            
            int currentTrunk = 0;

            foreach (var job in jobHistory)
            {
                while (job.EndTime > tempDate.AddDays(interval))
                {
                    currentTrunk++;
                    tempDate = tempDate.AddDays(interval);
                }
                if (!Bars.ContainsKey(job.JobStatus))
                {
                    Bars.Add(job.JobStatus, new BarInfo(numOfTrunk));
                }
                Bars[job.JobStatus].Count[currentTrunk]++;

                // if jobstatus == "Error"
                if (Constants.JobErrorStatuses[job.JobStatus] == "Error")
                {
                    Bars[job.JobStatus].ErrorMessages[currentTrunk].Add(new ErrorMessageBuilder(job).ToString());
                }


                if (!Pie.ContainsKey(job.JobStatus))
                    Pie.Add(job.JobStatus, 1);
                Pie[job.JobStatus]++;


            }

            // generate labels for bar graph
            DateTime startTime = jobHistory[0].EndTime;
            DateTime endTime = startTime.AddDays(interval);
            for(int i=0;i<numOfTrunk;i++)
            {
                Labels.Add(startTime.ToString("MM/dd") + "-" + endTime.ToString("MM/dd"));
                startTime = endTime;
                endTime = startTime.AddDays(interval);
            }
        }

        public class BarInfo
        {
            public int[] Count { get; }

            public List<string>[] ErrorMessages { get; }

            public BarInfo(int numberOfBar)
            {
                Count = new int[numberOfBar];
                ErrorMessages = new List<string>[numberOfBar];
                for(int i=0;i<numberOfBar;i++)
                {
                    ErrorMessages[i] = new List<string>();
                }
            }

            private string ToString(List<string> errorMessages)
            {
                StringBuilder sb = new StringBuilder();
                string newline = "";
                foreach (var errorMessage in errorMessages)
                {
                    sb.Append(newline);
                    sb.Append(errorMessage.ToString());
                    newline = "<br>";
                }
                return sb.ToString();
            }

            public string[] BuildErrorMessages()
            {
                return ErrorMessages.Select(x => ToString(x)).ToArray();
            }
        }

        public class ErrorMessageBuilder
        {
            public DateTime Date { get; set; }

            public int ErrorCode { get; set; }

            public string ErrorMessage { get; set; }

            public ErrorMessageBuilder(JobHistory job)
            {
                this.Date = job.StartTime;
                this.ErrorCode = job.ErrorCode;
                this.ErrorMessage = job.ErrorMessage;
            }

            public ErrorMessageBuilder(DateTime Date = new DateTime(),int ErrorCode = 0,string ErrorMessage = "")
            {
                this.Date = Date;
                this.ErrorCode = ErrorCode;
                this.ErrorMessage = ErrorMessage;
            }

            public void SetMessage(DateTime Date, int ErrorCode, string ErrorMessage)
            {
                this.Date = Date;
                this.ErrorCode = ErrorCode;
                this.ErrorMessage = ErrorMessage;
            }

            public override string ToString()
            {
                if (ErrorCode == 0)
                    return null;
                return string.Format("{0}  {1:X}   {2}",Date.ToString("MM/dd"),ErrorCode,ErrorMessage);
            }
        }
    }
}
