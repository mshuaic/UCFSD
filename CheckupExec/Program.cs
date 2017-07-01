﻿using CheckupExec.Analysis;
using CheckupExec.Controllers;
using CheckupExec.Models;
using CheckupExec.Models.AnalysisModels;
using CheckupExec.Models.ReportModels;
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
    //to-do: make this the api class which will communicate with the front end  
    class Program
    {
        static void Main(string[] args)
        {
            //remote bool and server credentials [since remote]
            bool remoteAccess = true;
            string password = "Veritas4935";
            string serverName = "server";
            string userName = "Administrator";

            var de = new DataExtraction(remoteAccess, password, serverName, userName);

            if (de.PowershellInstanceCreated)
            {
                //get storage device names
                var storageNames = de.GetStorageDeviceNames();

                foreach (var name in storageNames)
                {
                    Console.WriteLine(name);
                }

                //get job names for all storage devices (passed in list would be whatever was checked)
                var jobNames = de.GetJobNames(storageNames);

                foreach (var name in jobNames)
                {
                    Console.WriteLine(name);
                }

                //whatever report is ran by the user
                de.AlertsAnalysis("", jobNames, null);
                //dispose our runspace and powershell instances
                de.CleanUp();
                Console.WriteLine("Done.");
                Console.ReadLine();
            }

            //foreach (var point in fr.plot)
            //{
            //    Console.WriteLine("(" + point.Days + ", " + point.GB + ")");
            //}
            //Console.WriteLine(fr.FinalSlope);
            //Console.WriteLine(fr.FinalIntercept);

            //    //var jobController = new JobController();
            //    //var jobs = jobController.GetJobs();
            //    //var job = jobs.First();

            //    //var bje = new BackupJobEstimate(job.Id);
            //    //Console.WriteLine("Next Start Date: " + bje.NextStartDate);
            //    //Console.WriteLine("Job rate estimate: " + bje.EstimateOfJobRateMBMin);
            //    //Console.WriteLine("Elapsed time estimate (sec): " + bje.EstimateOfElapsedTimeSec);
            //    //Console.WriteLine("Data Size estimate: " + bje.EstimateDataSizeMB);
            //    //Console.WriteLine("=============================");

            //    //var forecast = new Forecast(job.Id);
            //    //Console.WriteLine("Slope: " + forecast.FinalSlope);
            //    //Console.WriteLine("Intercept: " + forecast.FinalIntercept);
            //    //Console.ReadLine();

            //    //var frontEnd = new FrontEndUsedCapacity();
            //    //Console.ReadLine();

            //    //called on application close?
        }

    }
}

