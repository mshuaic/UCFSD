using System;
using System.Collections.Generic;
using CheckupExec.Models;

namespace ReportGen.ErrorsReport
{
    class ErrorsReportInfo
    {
        public Dictionary<string, int[]> Bars { get; }

        public Dictionary<string, int> Pie { get; }

        public List<string> Labels { get; }

        public int TotalErrors { get; }

        public ErrorsReportInfo(List<JobHistory> jobHistory, int numOfTrunk)
        {
            Bars = new Dictionary<string, int[]>();
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
                    Bars.Add(job.JobStatus, new int[numOfTrunk]);
                }
                Bars[job.JobStatus][currentTrunk]++;

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
    }
}
