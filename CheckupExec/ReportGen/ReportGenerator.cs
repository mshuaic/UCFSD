using System;
using System.IO;
using System.Collections.Generic;
using CheckupExec.Models;

namespace ReportGen
{
    class ReportGenerator
    {
        public bool Successful { get; } = false;
        public ReportGenerator(string reportPath, List<Alert> alerts, int numOfTrunk = 10, string fileName = "AlertsReport.html")
        {
            string outputFile = Path.Combine(reportPath, fileName);
            try
            {
                new AlertsReport.AlertsReportGen(outputFile, alerts, numOfTrunk);
            }
            finally
            {
                Successful = true;
            }
            
        }

        public ReportGenerator(string reportPath, List<JobHistory> jobHistory, int numOfTrunk = 10, string fileName = "ErrorsReport.html")
        {
            string outputFile = Path.Combine(reportPath, fileName);
            try
            {
                new ErrorsReport.ErrorsReportGen(outputFile, jobHistory, numOfTrunk);
            }
            finally
            {
                Successful = true;
            }
        }
    }
}
