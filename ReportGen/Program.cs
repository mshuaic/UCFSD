using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CheckupExec.Models;
using ReportGen.ErrorsReport;
using ReportGen.AlertsReport;

namespace ReportGen
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = System.IO.File.ReadAllText(@"e:\example.json");

            List<JobHistory> jobhistory = JsonConvert.DeserializeObject<List<JobHistory>>(file);

            var foo = new ErrorsReportGen(@"e:\example_output.html", jobhistory);

            //string file = System.IO.File.ReadAllText(@"e:\alerts.json");

            //List<Alert> alerts = JsonConvert.DeserializeObject<List<Alert>>(file);

            //var foo = new AlertsReportGen(@"e:\alerts_output.html", alerts);

            Console.ReadKey();
        }
    }
}
