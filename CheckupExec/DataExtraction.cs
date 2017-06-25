using CheckupExec.Analysis;
using CheckupExec.Controllers;
using CheckupExec.Models;
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
                PowershellInstanceCreated = true;
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
            var sc = new StorageController();
            var names = new List<string>();

            _storageDevices = sc.GetStorages();

            foreach (var storageDevice in _storageDevices)
            {
                names.Add(storageDevice.Name);
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
            var jc = new JobController();
            var names = new List<string>();

            if (storageDeviceNames.Count > 0)
            {
                var jobParams = new Dictionary<string, string>
                {
                    ["tasktype"] = "full"
                };

                var fullNamesString = "";

                foreach (var name in storageDeviceNames)
                {
                    fullNamesString += name + ((storageDeviceNames.ElementAt(name.Length - 1).Equals(name)) ? "" : ", ");
                }

                jobParams["storage"] = fullNamesString;

                _jobs = jc.GetJobs(jobParams);

                foreach (var job in _jobs)
                {
                    names.Add(job.Name);
                }
            }
            else
            {
                _jobs = jc.GetJobs();

                foreach (var job in _jobs)
                {
                    names.Add(job.Name);
                }
            }

            return names;
        }

        /// <summary>
        /// Runs FrontendUsedCapacity and FrontendForecast. No parameters are needed since this is a full analysis of the server.
        /// </summary>
        /// <returns>True if successful, false if not.</returns>
        public bool FrontEndAnalysis()
        {
            //usedcapacity, plots into one dict, slopes into one, intercepts into one, 

            var feuc = new FrontEndUsedCapacity();

            var usedCapacity = feuc.TotalUsedCapacity;

            if (feuc.FrontEndForecast.ForecastsSuccessful)
            {
                var fullPlot = new Dictionary<double, double>();
                double fullSlope = 0;
                double fullIntercept = 0;

                //feuc.frontendforecast.forecasts are Dictionary<storage, forecastresults>
                foreach (var forecast in feuc.FrontEndForecast.Forecasts)
                {
                    foreach (var point in forecast.Value.plot)
                    {
                        fullPlot.Add(point.Key, point.Value);
                    }

                    fullSlope += forecast.Value.FinalSlope;
                    fullIntercept += forecast.Value.FinalIntercept;
                }

                //call report generator method for frontend and pass in ^

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
        public bool BackupJobsAnalysis(List<string> jobNames)
        {
            var buJobEstimates = new List<BackupJobEstimate>();

            if (jobNames.Count > 0)
            {
                var jobIds = new List<string>();

                foreach (var jobName in jobNames)
                {
                    jobIds.Add(_jobs.ElementAt(_jobs.FindIndex(x => x.Name.Equals(jobName))).Id);
                }

                var forecastResults = new Dictionary<BackupJobEstimate, ForecastResults>();

                foreach (var jobId in jobIds)
                {
                    var buje = new BackupJobEstimate(jobId);

                    buJobEstimates.Add(buje);

                    forecastResults[buje] = buje.ForecastResults;
                }

                //pass to report generator

                return true;
            }
            else if (jobNames != null)
            {
                var forecastResults = new Dictionary<BackupJobEstimate, ForecastResults>();

                foreach (var job in _jobs)
                {
                    var buje = new BackupJobEstimate(job.Id);

                    buJobEstimates.Add(buje);

                    forecastResults[buje] = buje.ForecastResults;
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
        public bool DiskAnalysis(List<string> diskNames)
        {
            if (diskNames.Count >= 0)
            {
                var forecastResults = new Dictionary<string, DiskForecast>();

                foreach (var diskName in diskNames)
                {
                    var dc = new DiskForecast(diskName);

                    forecastResults[diskName] = dc;
                }

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
        public bool AlertsAnalysis(List<string> jobNames = null, List<string> types = null, DateTime? start = null, DateTime? end = null)
        {
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
        public bool JobErrorsAnaylysis(List<string> jobNames = null, List<string> errorStatuses = null, DateTime? start = null, DateTime? end = null)
        {
            var jobErrorsAnalysis = new JobErrorsAnalyses(start, end, errorStatuses, jobNames);

            //pass to report generator

            if (jobErrorsAnalysis.Successful)
                return true;
            return false;
        }
    }
}
