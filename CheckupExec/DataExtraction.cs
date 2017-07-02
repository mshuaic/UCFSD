using CheckupExec.Analysis;
using CheckupExec.Controllers;
using CheckupExec.Models;
using CheckupExec.Models.AnalysisModels;
using CheckupExec.Models.BEMCLIModels;
using CheckupExec.Models.ReportModels;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public static AlertCategoryController AlertCategoryController { get; set; }

        public static JobController JobController { get; set; }

        public static JobHistoryController JobHistoryController { get; set; }

        public static LicenseInformationController LicenseInformationController { get; set; }

        public static StorageController StorageController { get; set; }

        public static StorageDevicePoolController StorageDevicePoolController { get; set; }

        public static EditionInformationController EditionInformationController { get; set; }

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
        public DataExtraction(bool isRemoteUser, string password = null, string serverName = null, string userName = null)
        {
            try
            {
                new BEMCLIHelper(isRemoteUser, password, serverName, userName);
            }
            catch
            {

            }

            if (BEMCLIHelper.powershell != null)
            {
                PowershellInstanceCreated    = true;

                AlertController              = new AlertController();

                AlertHistoryController       = new AlertHistoryController();

                AlertCategoryController      = new AlertCategoryController();

                JobController                = new JobController();

                JobHistoryController         = new JobHistoryController();

                LicenseInformationController = new LicenseInformationController();

                StorageController            = new StorageController();

                StorageDevicePoolController  = new StorageDevicePoolController();

                EditionInformationController = new EditionInformationController();
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
            var poolDevices = StorageDevicePoolController.GetStoragePools();

            if (_storageDevices != null && _storageDevices.Count > 0
                && poolDevices != null && poolDevices.Count > 0)
            {
                Storage temp = null;
                foreach (var device in poolDevices)
                {
                    if ((temp = _storageDevices.Find(x => x.Name.Equals(device.Name))) != null)
                    {
                        _storageDevices.Remove(temp);
                    }
                }
            }

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
            var jobNames = new List<string>();

            var jobHistoryPipeline = new Dictionary<string, Dictionary<string, string>>
            {
                [Constants.GetStorages] = new Dictionary<string, string>
                {
                    ["Name"] = ""
                }
            };

            var jobParams = new Dictionary<string, string>
            {
                ["tasktype"] = "'full'"
            };

            var fullNamesString = "";

            if (storageDeviceNames != null && storageDeviceNames.Count > 0)
            {
                foreach (string name in storageDeviceNames)
                {
                    fullNamesString += "'" + name + "'" + ((storageDeviceNames.ElementAt(storageDeviceNames.Count - 1).Equals(name)) ? "" : ", ");
                }
            }
            else
            {
                foreach (Storage storage in _storageDevices)
                {
                    fullNamesString += "'" + storage.Name + "'" + ((_storageDevices.ElementAt(_storageDevices.Count - 1).Equals(storage)) ? "" : ", ");
                }
            }

            jobHistoryPipeline[Constants.GetStorages]["Name"] = fullNamesString;

            var temp = JobHistoryController.GetJobHistories(jobHistoryPipeline) ?? new List<JobHistory>();
            var names = new List<string>();

            foreach (JobHistory jobHistory in temp)
            {
                if (Convert.ToInt32(jobHistory.JobStatus) == JobHistory.SuccessfulFinalStatus 
                    && jobHistory.PercentComplete == 100
                    && jobHistory.JobType == Constants.BACKUP_JOB_TYPE
                    && !names.Exists(x => x.Equals(jobHistory.JobName)))
                {
                    names.Add(jobHistory.JobName);
                }
            }

            List<Job> jobTemp = null;
            _jobs = new List<Job>();
            foreach (string name in names)
            {
                jobParams["Name"] = "'" + name + "'";

                if ((jobTemp = JobController.GetJobs(jobParams) ?? new List<Job>()).Count > 0)
                {
                    _jobs.AddRange(jobTemp);
                }
            }
            
            foreach (Job job in _jobs)
            {
                jobNames.Add(job.Name);
            }
            
            return jobNames;
        }

        public List<string> GetAlertCategoryNames()
        {
            var alertCategories = AlertCategoryController.GetAlertCategories();
            var alertCategoryNames = new List<string>();

            if (alertCategories != null && alertCategories.Count > 0)
            {
                foreach (AlertCategory category in alertCategories)
                {
                    alertCategoryNames.Add(category.Name);
                }
            }

            return alertCategoryNames;
        }

        public List<string> GetJobErrorStatuses()
        {
            var errorStatuses = new List<string>();

            foreach (KeyValuePair<string, string> errorStatus in Constants.JobErrorStatuses)
            {
                errorStatuses.Add(errorStatus.Value);
            }

            return errorStatuses;
        }

        /// <summary>
        /// Runs FrontendUsedCapacity and FrontendForecast. No parameters are needed since this is a full analysis of the server.
        /// </summary>
        /// <returns>True if successful, false if not.</returns>
        public bool FrontEndAnalysis(string reportPath)
        {
            //run frontendanalysis
            var feuc = new FrontEndUsedCapacity(_storageDevices);

            //used is total data being processed by be. max is total max storage on all resources that be currently backs up.
            double maxCapacity = 0;
            double usedCapacity = 0;

            //get editioninformation here, then licenseinformation (count of used licenses that correspond to each tier * TB and compare with usedcap.)

            foreach (FE_Forecast FE_Forecast in feuc.Fe_Forecasts)
            {
                maxCapacity += FE_Forecast.MaxCapacity;
                usedCapacity += FE_Forecast.UsedCapacity;
            };

            // || true to test with our sets
            //front end analysis has already been run, so here we check if it was successful
            //if it was, organize the data so it can be concisely passed to report generator
            if (feuc.FrontEndForecast.ForecastsSuccessful || true)
            {
                var fullPlot         = new List<PlotPoint>();
                double fullSlope     = 0;
                double fullIntercept = 0;

                foreach (FE_Forecast forecast in feuc.Fe_Forecasts)
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

                //call license analysis and pass used cap.
                //brings back analysis and max cap. support by current licensing

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

                report.StorageDevices = new List<Storage>();

                foreach (FE_Forecast fe_forecast in feuc.Fe_Forecasts)
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
            var buJobEstimates = new List<BackupJobEstimateModel>();

            //if we have a subset of total jobs passed to us, for each of these run backupjobestimate on it and add to a list for report generator
            if (jobNames != null && jobNames.Count > 0)
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
                        var buje = new BackupJobEstimate(jobId).BackupJobEstimateModel;

                        buJobEstimates.Add(buje);
                    }
                }
            }
            //if no subset is passed, do the same thing but for every job
            else
            {
                if (_jobs != null && _jobs.Count > 0)
                {
                    foreach (Job job in _jobs)
                    {
                        var buje = new BackupJobEstimate(job.Id).BackupJobEstimateModel;

                        buJobEstimates.Add(buje);
                    }
                }
            }

            var reports = new List<BackupJobReport>();

            foreach (BackupJobEstimateModel buje in buJobEstimates)
            {
                var report = new BackupJobReport();

                report.HistoricalPoints = buje.ForecastResults.plot;

                report.ForecastPoints = new List<PlotPoint>();

                double maxCapacity = (double)(buje.MaxCapacityBytes >> 20) / 1024;

                report.DaysTo50 = (maxCapacity * .5 - buje.ForecastResults.FinalIntercept) 
                    / buje.ForecastResults.FinalSlope;

                report.ForecastPoints.Add(new PlotPoint
                {
                    Days = report.DaysTo50,
                    GB = (maxCapacity * .5)
                });

                report.DaysTo75 = (maxCapacity * .75 - buje.ForecastResults.FinalIntercept) 
                    / buje.ForecastResults.FinalSlope;

                report.ForecastPoints.Add(new PlotPoint
                {
                    Days = report.DaysTo75,
                    GB = (maxCapacity * .75)
                });

                report.DaysTo90 = (maxCapacity * .9 - buje.ForecastResults.FinalIntercept) 
                    / buje.ForecastResults.FinalSlope;

                report.ForecastPoints.Add(new PlotPoint
                {
                    Days = report.DaysTo90,
                    GB = (maxCapacity * .9)
                });

                report.DaysToFull = (maxCapacity - buje.ForecastResults.FinalIntercept) 
                    / buje.ForecastResults.FinalSlope;

                report.ForecastPoints.Add(new PlotPoint
                {
                    Days = report.DaysToFull,
                    GB = (maxCapacity)
                });

                report.MaxCapacity = maxCapacity;
                report.UsedCapacity = (double)(buje.UsedCapacityBytes >> 20) / 1024;
                report.JobName = buje.JobName;
                report.NextDataSize = buje.EstimateDataSizeMB;
                report.NextElapsedTimeSeconds = buje.EstimateOfElapsedTimeSec;
                report.NextJobDate = buje.NextStartDate;
                report.NextJobRate = buje.EstimateOfJobRateMBMin;

                reports.Add(report);

                //pass list to report generator
            }

            return (reports.Count > 0);
        }

        /// <summary>
        /// Runs DiskForecast and DiskUsedCapacity on the list of disks.
        /// </summary>
        /// <param name="diskNames">List containing the names of the disks to run the report on.</param>
        /// <returns>True if successful, false if not.</returns>
        public bool DiskAnalysis(List<string> diskNames, string diskPath, string reportPath)
        {
            var diskAnalyses = new DiskForecast(diskNames, diskPath).UsedCapacityForecastModels;

            var reports = new List<DiskCapacityReport>();

            foreach (UsedCapacityForecastModel ucm in diskAnalyses)
            {
                var report = new DiskCapacityReport();

                report.HistoricalPoints = ucm.ForecastResults.plot;

                report.ForecastPoints = new List<PlotPoint>();

                double maxCapacity = (double)(ucm.TotalCapacity >> 20) / 1024;

                report.DaysTo50 = (maxCapacity * .5 - ucm.ForecastResults.FinalIntercept)
                    / ucm.ForecastResults.FinalSlope;

                report.ForecastPoints.Add(new PlotPoint
                {
                    Days = report.DaysTo50,
                    GB = (maxCapacity * .5)
                });

                report.DaysTo75 = (maxCapacity * .75 - ucm.ForecastResults.FinalIntercept)
                    / ucm.ForecastResults.FinalSlope;

                report.ForecastPoints.Add(new PlotPoint
                {
                    Days = report.DaysTo75,
                    GB = (maxCapacity * .75)
                });

                report.DaysTo90 = (maxCapacity * .9 - ucm.ForecastResults.FinalIntercept)
                    / ucm.ForecastResults.FinalSlope;

                report.ForecastPoints.Add(new PlotPoint
                {
                    Days = report.DaysTo90,
                    GB = (maxCapacity * .9)
                });

                report.DaysToFull = (maxCapacity - ucm.ForecastResults.FinalIntercept)
                    / ucm.ForecastResults.FinalSlope;

                report.ForecastPoints.Add(new PlotPoint
                {
                    Days = report.DaysToFull,
                    GB = (maxCapacity)
                });

                report.MaxCapacity = maxCapacity;
                report.UsedCapacity = (double)(ucm.UsedCapacityInstances.Last().Bytes >> 20) / 1024;
                report.DiskName = ucm.StorageName;
                
                reports.Add(report);

                //pass list to report generator
            }

            return (reports.Count > 0);
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

        //public bool DemoTest()
        //{
        //    var instances = new List<JobHistory>();
        //    Random j = new Random();
        //    long bytes = 10000000000;

        //    for (int i = 0, k = 100; i <= 100; i++, k--)
        //    {
        //        bytes = bytes + j.Next(-100000, 10000000);

        //        instances.Add(new JobHistory
        //        {
        //            TotalDataSizeBytes = bytes,
        //            StartTime = DateTime.Now.Date.AddDays(-k)
        //        });
        //    }

        //    double maxCapacity = 20;

        //    var fc = new Forecast<JobHistory>();

        //    var fr = fc.doForecast(instances);

        //    var report = new BackupJobReport();

        //    report.HistoricalPoints = fr.plot;

        //    report.ForecastPoints = new List<PlotPoint>();

        //    report.DaysTo50 = ((maxCapacity * .5) - fr.FinalIntercept) / fr.FinalSlope;
        //    report.ForecastPoints.Add(new PlotPoint
        //    {
        //        Days = report.DaysTo50,
        //        GB = (maxCapacity * .5)
        //    });

        //    report.DaysTo75 = ((maxCapacity * .75) - fr.FinalIntercept) / fr.FinalSlope;
        //    report.ForecastPoints.Add(new PlotPoint
        //    {
        //        Days = report.DaysTo75,
        //        GB = (maxCapacity * .75)
        //    });

        //    report.DaysTo90 = ((maxCapacity * .9) - fr.FinalIntercept) / fr.FinalSlope;
        //    report.ForecastPoints.Add(new PlotPoint
        //    {
        //        Days = report.DaysTo90,
        //        GB = (maxCapacity * .9)
        //    });

        //    report.DaysToFull = (maxCapacity - fr.FinalIntercept) / fr.FinalSlope;
        //    report.ForecastPoints.Add(new PlotPoint
        //    {
        //        Days = report.DaysToFull,
        //        GB = (maxCapacity)
        //    });

        //    report.MaxCapacity = maxCapacity;
        //    report.UsedCapacity = (double) (bytes >> 20) / 1024;
        //    report.JobName = "Demo Test Job";

        //    return true;
        //}
    }
}
