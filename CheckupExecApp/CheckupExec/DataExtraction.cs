using CheckupExec.Analysis;
using CheckupExec.Controllers;
using CheckupExec.Models;
using CheckupExec.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec
{
    //to-do: log utility, get-* validation(?), job-data-footprint+, jobestimate, frontendcapacityuse,  
    class DataExtraction
    {
        static void Main(string[] args)
        {
            bool remoteAccess = true;
            string password = "Veritas4935";
            string serverName = "WIN-GAEP2HNEO1O";
            string userName = "Administrator";

            var AlertHistories = new AlertHistoryController();

            new BEMCLIHelper(remoteAccess, password, serverName, userName);

            if (BEMCLIHelper.powershell != null)
            {
                //example of how you would pretty much run get-bealert -severity "warning" | convertto-json
                var parameters = new Dictionary<string, string>();
                parameters.Add("severity", "warning");

                var alertsBySeverity = AlertHistories.GetAlertHistoriesBy(parameters);

                foreach (var alert in alertsBySeverity)
                    Console.WriteLine(JsonHelper.JsonSerializer<Alert>(alert));
                Console.ReadLine();
            }

            //var jobController = new JobController();
            //var jobs = jobController.GetJobs();
            //var job = jobs.First();

            //var bje = new BackupJobEstimate(job.Id);
            //Console.WriteLine("Next Start Date: " + bje.NextStartDate);
            //Console.WriteLine("Job rate estimate: " + bje.EstimateOfJobRateMBMin);
            //Console.WriteLine("Elapsed time estimate (sec): " + bje.EstimateOfElapsedTimeSec);
            //Console.WriteLine("Data Size estimate: " + bje.EstimateDataSizeMB);
            //Console.WriteLine("=============================");

            //var forecast = new Forecast(job.Id);
            //Console.WriteLine("Slope: " + forecast.FinalSlope);
            //Console.WriteLine("Intercept: " + forecast.FinalIntercept);
            //Console.ReadLine();

            //var frontEnd = new FrontEndUsedCapacity();
            //Console.ReadLine();

            BEMCLIHelper.cleanUp();
        }

    }
}
