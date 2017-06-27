using CheckupExec.Analysis;
using CheckupExec.Controllers;
using CheckupExec.Models;
using CheckupExec.Models.AnalysisModels;
using CheckupExec.Models.ReportModels;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec
{
    public class DataExtraction
    {
        /// <summary>
        /// True if powershell and runspace instances were successfully created.
        /// </summary>
        public bool PowershellInstanceCreated { get; }

        public static AlertController AlertController { get; set; }

        public static AlertHistoryController AlertHistoryController { get; set; }

        public static JobController JobController { get; set; }

        public static JobHistoryController JobHistoryController { get; set; }

        public static LicenseInformationController LicenseInformationController { get; set; }

        public static StorageController StorageController { get; set; }

        private List<Storage> _storageDevices { get; set; }

        private List<Job> _jobs { get; set; }

        /// <summary>
        /// Creates the instance of powershell and runspace. If user is remote, pass in (true, x, x, x,) where x's are not null. Otherwise,
        /// just ensure you pass in (false).
        /// </summary>
        /// <param name="isRemoteUser"></param>
        /// <param name="serverName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public DataExtraction(bool isRemoteUser, string serverName = null, string userName = null, string password = null)
        {
            try
            {
                new BEMCLIHelper(isRemoteUser, serverName, userName, password);
            }
            catch
            {

            }

            if (BEMCLIHelper.powershell != null)
            {
                PowershellInstanceCreated    = true;

                AlertController              = new AlertController();

                AlertHistoryController       = new AlertHistoryController();

                JobController                = new JobController();

                JobHistoryController         = new JobHistoryController();

                LicenseInformationController = new LicenseInformationController();

                StorageController            = new StorageController();
            }
            else
            {
                PowershellInstanceCreated = false;
            }  
        }

        /// <summary>
        /// Indiscriminate fetch of all storage devices on the BE server.
        /// </summary>
        /// <returns>List of strings representing storage device names.</returns>
        public List<string> GetStorageDeviceNames()
        {
            var names = new List<string>();

            //retrieve storage devices and return their names
            _storageDevices = StorageController.GetStorages();

            if (_storageDevices != null && _storageDevices.Count > 0)
            {
                foreach (Storage storageDevice in _storageDevices)
                {
                    names.Add(storageDevice.Name);
                }
            }

            return names;
        }

        /// <summary>
        /// Fetches Jobs which are tied to storage devices whose names are specified.
        /// </summary>
        /// <param name="storageDeviceNames">if empty, searches all jobs.</param>
        /// <returns>List of strings representing job names.</returns>
        public List<string> GetJobNames(List<string> storageDeviceNames)
        {
            var names = new List<string>();

            //if we have storagedevices, retrieve the full backup jobs of those devices and return the names of those retrieved
            if (storageDeviceNames != null && storageDeviceNames.Count > 0)
            {
                var jobParams = new Dictionary<string, string>
                {
                    ["tasktype"] = "full"
                };

                var fullNamesString = "";

                foreach (string name in storageDeviceNames)
                {
                    fullNamesString += "'" + name + "'" + ((storageDeviceNames.ElementAt(storageDeviceNames.Count - 1).Equals(name)) ? "" : ", ");
                }

                jobParams["storage"] = fullNamesString;

                _jobs = JobController.GetJobs(jobParams);

                if (_jobs != null && _jobs.Count > 0)
                {
                    foreach (Job job in _jobs)
                    {
                        names.Add(job.Name);
                    }
                }
            }
            else
            {
                _jobs = JobController.GetJobs();

                if (_jobs != null && _jobs.Count > 0)
                {
                    foreach (Job job in _jobs)
                    {
                        names.Add(job.Name);
                    }
                }
            }

            return names;
        }

        /// <summary>
        /// Runs FrontendUsedCapacity and FrontendForecast. No parameters are needed since this is a full analysis of the server.
        /// </summary>
        /// <returns>True if successful, false if not.</returns>
        public bool FrontEndAnalysis(string reportPath)
        {
            //run frontendanalysis
            var feuc = new FrontEndUsedCapacity(_storageDevices);

            var maxCapacity = feuc.FrontEndForecast.MaxCapacity;
            var usedCapacity = feuc.TotalUsedCapacity;

            // || true to test with our sets
            //front end analysis has already been run, so here we check if it was successful
            //if it was, organize the data so it can be concisely passed to report generator
            if (feuc.FrontEndForecast.ForecastsSuccessful || true)
            {
                var fullPlot         = new List<PlotPoint>();
                double fullSlope     = 0;
                double fullIntercept = 0;

                //
                foreach (FE_Forecast forecast in feuc.FrontEndForecast.FE_Forecasts)
                {
                    foreach (PlotPoint point in forecast.ForecastResults.plot)
                    {
                        fullPlot.Add( new PlotPoint
                        {
                            Days = point.Days,
                            GB = point.GB
                        });
                    }

                    fullSlope     += forecast.ForecastResults.FinalSlope;
                    fullIntercept += forecast.ForecastResults.FinalIntercept;
                }

                //call report generator method for frontend and pass in ^

                //foreach (var pointList in fullPlot)
                //{
                //    foreach (var point in pointList.Value)
                //    {
                //        Console.WriteLine("(" + pointList.Key + ", " + point + ")");
                //    }
                //}

                //Console.WriteLine("Slope: " + fullSlope);
                //Console.WriteLine("Intercept: " + fullIntercept);
                var report = new FrontEndCapacityReport();

                report.HistoricalPoints = fullPlot;

                report.ForecastPoints = new List<PlotPoint>();

                report.DaysTo50 = ((maxCapacity * .5) - fullIntercept) / fullSlope;
                report.ForecastPoints.Add(new PlotPoint
                {
                    Days = report.DaysTo50,
                    GB = (maxCapacity * .5)
                });

                report.DaysTo75 = ((maxCapacity * .75) - fullIntercept) / fullSlope;
                report.ForecastPoints.Add(new PlotPoint
                {
                    Days = report.DaysTo75,
                    GB = (maxCapacity * .75)
                });

                report.DaysTo90 = ((maxCapacity * .9) - fullIntercept) / fullSlope;
                report.ForecastPoints.Add(new PlotPoint
                {
                    Days = report.DaysTo90,
                    GB = (maxCapacity * .9)
                });

                report.DaysToFull = (maxCapacity - fullIntercept) / fullSlope;
                report.ForecastPoints.Add(new PlotPoint
                {
                    Days = report.DaysToFull,
                    GB = (maxCapacity)
                });

                foreach (FE_Forecast fe_forecast in feuc.FrontEndForecast.FE_Forecasts)
                {
                    report.StorageDevices.Add(fe_forecast.Storage);
                }

                report.MaxCapacity = maxCapacity;
                report.UsedCapacity = usedCapacity;

                //pass to report

                return true;
            }
            else
            {
                //error message on why it failed? Logging utility would be useful here
                return false;
            }
        }

        /// <summary>
        /// Runs BackupJobEstimate and Forecast on each job specified. Runs on all if list is empty.
        /// </summary>
        /// <param name="jobNames">List of jobNames which have been chosen by the user.</param>
        /// <returns>True if successful, false if not.</returns>
        public bool BackupJobsAnalysis(List<string> jobNames, string reportPath)
        {
            var buJobEstimates = new List<BackupJobEstimate>();

            //if we have a subset of total jobs passed to us, for each of these run backupjobestimate on it and add to a list for report generator
            if (jobNames!= null && jobNames.Count > 0)
            {
                var jobIds = new List<string>();

                foreach (string jobName in jobNames)
                {
                    jobIds.Add(_jobs.ElementAt(_jobs.FindIndex(x => x.Name.Equals(jobName))).Id);
                }

                if (jobIds.Count > 0)
                {
                    foreach (string jobId in jobIds)
                    {
                        var buje = new BackupJobEstimate(jobId);

                        buJobEstimates.Add(buje);
                    }
                }

                //pass to report generator

                //foreach (var fr in forecastResults)
                //{
                //    Console.WriteLine(fr.Key);

                //    foreach (var pointList in fr.Value.plot)
                //    {
                //        foreach (var point in pointList.Value)
                //        {
                //            Console.WriteLine("(" + pointList.Key + ", " + point + ")");
                //        }
                //    }

                //    Console.WriteLine("Slope: " + fr.Value.FinalSlope);
                //    Console.WriteLine("Intercept: " + fr.Value.FinalIntercept);
                //}

                return true;
            }
            //if no subset is passed, do the same thing but for every job
            else if (jobNames != null)
            {
                if (_jobs != null && _jobs.Count > 0)
                {
                    foreach (Job job in _jobs)
                    {
                        var buje = new BackupJobEstimate(job.Id);

                        buJobEstimates.Add(buje);
                    }
                }

                //pass to report generator

                return true;
            }

            return false;
        }

        /// <summary>
        /// Runs DiskForecast and DiskUsedCapacity on the list of disks.
        /// </summary>
        /// <param name="diskNames">List containing the names of the disks to run the report on.</param>
        /// <returns>True if successful, false if not.</returns>
        public bool DiskAnalysis(List<string> diskNames, string diskPath, string reportPath)
        {
            //if we have disks passed to us, run a DiskAnalysis on each of them and add each to a list for passing to report generator
            if (diskNames != null && diskNames.Count >= 0)
            {
                var diskAnalyses = new DiskForecast(diskNames, diskPath).DiskForecastModels;

                //pass to report generator

                return true;
            }

            return false;
        }

        /// <summary>
        /// Runs Alert analysis on the alerts corresponding to the passed parameters.
        /// </summary>
        /// <param name="jobNames">Optional. List of job names whose alerts we should check.</param>
        /// <param name="start">Optional. Start date of time window.</param>
        /// <param name="end">Optional. End date of time window.</param>
        /// <param name="types">Optional. Alert categories to check for.</param>
        /// <returns>True if successful, false if not.</returns>
        public bool AlertsAnalysis(string reportPath, List<string> jobNames, List<string> types, DateTime? start = null, DateTime? end = null)
        {
            //run AlertsAnalysis with given params
            var alertsAnalysis = new AlertsAnalyses(start, end, jobNames, types);

            //pass to report generator

            if (alertsAnalysis.Successful)
                return true;
            return false;
        }

        /// <summary>
        /// Runs JobErrorAnalysis on the jobs corresponding to the passed parameters.
        /// </summary>
        /// <param name="jobName">Optional. List of job names whose errors we should check for. If empty, all are checked.</param>
        /// <param name="start">Optional. Start date of time window.</param>
        /// <param name="end">Optional. End date of time window.</param>
        /// <param name="errorStatuses">Optional. Final error statuses of job instances we should look for. If empty, all are checked.</param>
        /// <returns>True if successful, false if not.</returns>
        public bool JobErrorsAnalysis(string reportPath, List<string> jobNames, List<string> errorStatuses, DateTime? start = null, DateTime? end = null)
        {
            //run JobErrorsAnalysis with given params
            var jobErrorsAnalysis = new JobErrorsAnalyses(start, end, errorStatuses, jobNames);

            //pass to report generator

            if (jobErrorsAnalysis.Successful)
                return true;
            return false;
        }

        public bool CleanUp()
        {
            return BEMCLIHelper.CleanUp();
        }
    }
}
