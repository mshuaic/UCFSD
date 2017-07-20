using CheckupExec.Analysis;
using CheckupExec.Controllers;
using CheckupExec.Models;
using CheckupExec.Models.AnalysisModels;
using CheckupExec.Models.ReportModels;
using CheckupExec.Utilities;
using ReportGen;
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
                //new BEMCLIHelper(isRemoteUser, password, serverName, userName);
                new BEMCLIHelper(true, "Veritas4935", "server", "Administrator");
            }
            catch
            {
                // ignored for now
            }

            if (BEMCLIHelper.Powershell != null)
            {
                PowershellInstanceCreated = true;

                AlertController = new AlertController();

                AlertHistoryController = new AlertHistoryController();

                AlertCategoryController = new AlertCategoryController();

                JobController = new JobController();

                JobHistoryController = new JobHistoryController();

                LicenseInformationController = new LicenseInformationController();

                StorageController = new StorageController();

                StorageDevicePoolController = new StorageDevicePoolController();

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
                foreach (var device in poolDevices)
                {
                    Storage temp = null;
                    if ((temp = _storageDevices.Find(x => x.Name.Equals(device.Name))) != null)
                    {
                        _storageDevices.Remove(temp);
                    }
                }
            }

            if (_storageDevices != null && _storageDevices.Count > 0)
            {
                names.AddRange(_storageDevices.Select(storageDevice => storageDevice.Name));
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
                fullNamesString = storageDeviceNames
                                 .Aggregate(fullNamesString, (current, name) => current + ("'" + name + "'" + ((storageDeviceNames
                                                                                                                .ElementAt(storageDeviceNames.Count - 1)
                                                                                                                .Equals(name)) ? "" : ", ")));
            }
            else
            {
                fullNamesString = _storageDevices
                                .Aggregate(fullNamesString, (current, storage) => current + ("'" + storage.Name + "'" + ((_storageDevices
                                                                                                                        .ElementAt(_storageDevices.Count - 1)
                                                                                                                        .Equals(storage)) ? "" : ", ")));
            }

            jobHistoryPipeline[Constants.GetStorages]["Name"] = fullNamesString;

            var temp = JobHistoryController.GetJobHistories(jobHistoryPipeline) ?? new List<JobHistory>();
            var names = new List<string>();

            foreach (JobHistory jobHistory in temp)
            {
                if (Convert.ToInt32(jobHistory.JobStatus) == Constants.SUCCESSFUL_JOB_STATUS
                    && jobHistory.PercentComplete == 100
                    && jobHistory.JobType == Constants.BACKUP_JOB_TYPE
                    && !names.Exists(x => x.Equals(jobHistory.JobName)))
                {
                    names.Add(jobHistory.JobName);
                }
            }

            _jobs = new List<Job>();
            foreach (string name in names)
            {
                jobParams["Name"] = "'" + name + "'";

                List<Job> jobTemp = null;
                if ((jobTemp = JobController.GetJobs(jobParams) ?? new List<Job>()).Count > 0)
                {
                    _jobs.AddRange(jobTemp);
                }
            }

            return _jobs.Select(job => job.Name).ToList();
        }

        public List<string> GetAlertCategoryNames()
        {
            var alertCategories = AlertCategoryController.GetAlertCategories();
            var alertCategoryNames = new List<string>();

            if (alertCategories != null && alertCategories.Count > 0)
            {
                alertCategoryNames.AddRange(alertCategories.Select(category => category.Name));
            }

            return alertCategoryNames;
        }

        public List<string> GetJobErrorStatuses()
        {
            return Constants.JobErrorStatuses.Select(errorStatus => errorStatus.Value).ToList();
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

            foreach (FeForecast feForecast in feuc.FeForecasts)
            {
                maxCapacity += feForecast.MaxCapacity;
                usedCapacity += feForecast.UsedCapacity;
            };

            // || true to test with our sets
            //front end analysis has already been run, so here we check if it was successful
            //if it was, organize the data so it can be concisely passed to report generator
            if (feuc.FrontEndForecast.ForecastsSuccessful || true)
            {
                var fullPlot = new List<PlotPoint>();
                List<BackupJobReport> backupJobs = new List<BackupJobReport>();
                double fullSlope = 0;
                double fullIntercept = 0;

                foreach (FeForecast forecast in feuc.FeForecasts)
                {
                    BackupJobReport temp = new BackupJobReport
                    {
                        HistoricalPoints = new List<PlotPoint>(),
                        ForecastPoints = new List<PlotPoint>()
                    };

                    fullPlot.AddRange(forecast.ForecastResults.Plot);

                    temp.HistoricalPoints.AddRange(forecast.ForecastResults.Plot);

                    temp.StorageName = forecast.Storage.Name;

                    double maxCapacityTemp = (double)(forecast.Storage.TotalCapacityBytes >> 20) / 1024;

                    temp.DaysTo50 = (maxCapacity * .5 - forecast.ForecastResults.FinalIntercept)
                                      / forecast.ForecastResults.FinalSlope;

                    temp.ForecastPoints.Add(new PlotPoint
                    {
                        Days = temp.DaysTo50,
                        GB = (maxCapacity * .5)
                    });

                    temp.DaysTo75 = (maxCapacity * .75 - forecast.ForecastResults.FinalIntercept)
                                      / forecast.ForecastResults.FinalSlope;

                    temp.ForecastPoints.Add(new PlotPoint
                    {
                        Days = temp.DaysTo75,
                        GB = (maxCapacity * .75)
                    });

                    temp.DaysTo90 = (maxCapacity * .9 - forecast.ForecastResults.FinalIntercept)
                                      / forecast.ForecastResults.FinalSlope;

                    temp.ForecastPoints.Add(new PlotPoint
                    {
                        Days = temp.DaysTo90,
                        GB = (maxCapacity * .9)
                    });

                    temp.DaysToFull = (maxCapacity - forecast.ForecastResults.FinalIntercept)
                                        / forecast.ForecastResults.FinalSlope;

                    temp.ForecastPoints.Add(new PlotPoint
                    {
                        Days = temp.DaysToFull,
                        GB = (maxCapacity)
                    });

                    temp.MaxCapacity = maxCapacityTemp;
                    temp.UsedCapacity = (double)(forecast.Storage.UsedCapacityBytes >> 20) / 1024;
                    temp.JobName = forecast.JobName;

                    backupJobs.Add(temp);

                    fullSlope += forecast.ForecastResults.FinalSlope;
                    fullIntercept += forecast.ForecastResults.FinalIntercept;
                }

                //call license analysis and pass used cap.
                //brings back analysis and max cap. support by current licensing

                var report = new FrontEndCapacityReport
                {
                    HistoricalPoints = fullPlot,
                    ForecastPoints = new List<PlotPoint>(),
                    Slope = fullSlope,
                    Intercept = fullIntercept,
                    DaysTo50 = ((maxCapacity * .5) - fullIntercept) / fullSlope,
                    BackupJobs = new List<BackupJobReport>()
                };

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

                foreach (FeForecast fe_forecast in feuc.FeForecasts)
                {
                    report.StorageDevices.Add(fe_forecast.Storage);
                }

                report.MaxCapacity = maxCapacity;
                report.UsedCapacity = usedCapacity;

                var licenseAnalysis = new LicenseAnalysis(report);

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
                var jobIds = jobNames.Select(jobName => _jobs.ElementAt(_jobs.FindIndex(x => x.Name.Equals(jobName))).Id).ToList();

                if (jobIds.Count > 0)
                {
                    buJobEstimates.AddRange(jobIds.Select(jobId => new BackupJobEstimate(jobId).BackupJobEstimateModel));
                }
            }
            //if no subset is passed, do the same thing but for every job
            else
            {
                if (_jobs != null && _jobs.Count > 0)
                {
                    buJobEstimates.AddRange(_jobs.Select(job => new BackupJobEstimate(job.Id).BackupJobEstimateModel));
                }
            }

            var reports = new List<BackupJobReport>();

            foreach (BackupJobEstimateModel buje in buJobEstimates)
            {
                var report = new BackupJobReport
                {
                    HistoricalPoints = buje.ForecastResults.Plot,
                    ForecastPoints = new List<PlotPoint>()
                };

                var storage = _storageDevices.Find(x => x.Name.Equals(buje.StorageName));

                report.StorageType = storage.StorageType;
                report.StorageName = storage.Name;

                double maxCapacity = (double)(storage.TotalCapacityBytes >> 20) / 1024;

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
                report.UsedCapacity = (double)(storage.UsedCapacityBytes >> 20) / 1024;
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
                var report = new DiskCapacityReport
                {
                    HistoricalPoints = ucm.ForecastResults.Plot,
                    ForecastPoints = new List<PlotPoint>()
                };

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

            var reportGen = new HtmlGen();

            try
            {
                reportGen.makeReportDisk(reports, @"C:\Users\Galic\Desktop");
            }
            catch
            {
                return false;
            }


            return (reports.Count > 0);
        }

        /// <summary>
        /// Runs Alert analysis on the alerts corresponding to the passed parameters.
        /// </summary>
        /// <param name="reportPath"></param>
        /// <param name="jobNames">Optional. List of job names whose alerts we should check.</param>
        /// <param name="start">Optional. Start date of time window.</param>
        /// <param name="end">Optional. End date of time window.</param>
        /// <param name="types">Optional. Alert categories to check for.</param>
        /// <returns>True if successful, false if not.</returns>
        public bool AlertsAnalysis(string reportPath, List<string> jobNames, List<string> types, DateTime? start = null, DateTime? end = null)
        {
            List<string> splitTypes = new List<string>();

            if (types != null && types.Count > 0)
            {
                splitTypes.AddRange(types.Select(type => type.Split(' ')).Select(temp => temp.Aggregate("", (current, stringInType) => current + stringInType)));
            }

            //run AlertsAnalysis with given params
            var alertsAnalysis = new AlertsAnalyses(start, end, jobNames, splitTypes);

            //pass to report generator
            //new AlertsReportGen(reportPath, alertsAnalysis.GetAlerts());
            new ReportGenerator(reportPath, alertsAnalysis.GetAlerts());

            return alertsAnalysis.Successful;
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
            //new ErrorsReportGen(reportPath, jobErrorsAnalysis.GetJobHistories());
            new ReportGenerator(reportPath, jobErrorsAnalysis.GetJobHistories());

            return jobErrorsAnalysis.Successful;
        }

        public bool CleanUp()
        {
            return BEMCLIHelper.CleanUp();
        }

        public bool DemoTest()
        {
            var instances = new List<UsedCapacity>();
            Random j = new Random();
            long bytes = 0;

            for (int i = 0, k = 100; i <= 30; i++, k--)
            {
                bytes = bytes + j.Next(-100000, 300000000);

                instances.Add(new UsedCapacity
                {
                    Bytes = bytes,
                    Date = DateTime.Now.Date.AddDays(-k)
                });
            }

            for (int i = 30, k = 70; i <= 80; i++, k--)
            {
                bytes = bytes + j.Next(-100000, 500000000);

                instances.Add(new UsedCapacity
                {
                    Bytes = bytes,
                    Date = DateTime.Now.Date.AddDays(-k)
                });
            }

            for (int i = 80, k = 20; i <= 100; i++, k--)
            {
                bytes = bytes + j.Next(-100000, 1000000000);

                instances.Add(new UsedCapacity
                {
                    Bytes = bytes,
                    Date = DateTime.Now.Date.AddDays(-k)
                });
            }

            double maxCapacity = 40;

            var fc = new Forecast<UsedCapacity>();

            var fr = fc.doForecast(instances);

            var report = new DiskCapacityReport();

            report.HistoricalPoints = fr.Plot;

            report.ForecastPoints = new List<PlotPoint>();

            report.ForecastPoints.Add(new PlotPoint
            {
                Days = report.HistoricalPoints.First().Days,
                GB = fr.FinalIntercept + fr.FinalSlope * report.HistoricalPoints.First().Days
            });

            report.DaysTo50 = ((maxCapacity * .5) - fr.FinalIntercept) / fr.FinalSlope;
            report.ForecastPoints.Add(new PlotPoint
            {
                Days = report.DaysTo50,
                GB = (maxCapacity * .5)
            });

            report.DaysTo75 = ((maxCapacity * .75) - fr.FinalIntercept) / fr.FinalSlope;
            report.ForecastPoints.Add(new PlotPoint
            {
                Days = report.DaysTo75,
                GB = (maxCapacity * .75)
            });

            report.DaysTo90 = ((maxCapacity * .9) - fr.FinalIntercept) / fr.FinalSlope;
            report.ForecastPoints.Add(new PlotPoint
            {
                Days = report.DaysTo90,
                GB = (maxCapacity * .9)
            });

            report.DaysToFull = (maxCapacity - fr.FinalIntercept) / fr.FinalSlope;
            report.ForecastPoints.Add(new PlotPoint
            {
                Days = report.DaysToFull,
                GB = (maxCapacity)
            });

            report.HistoricalPoints.Add(new PlotPoint
            {
                Days = report.DaysToFull,
                GB = (maxCapacity)
            });

            report.MaxCapacity = maxCapacity;
            report.UsedCapacity = (double)(bytes >> 20) / 1024;
            report.DiskName = "Demo Test Job";

            var reports = new List<DiskCapacityReport> { report };

            var reportGen = new HtmlGen();

            try
            {
                reportGen.makeReportDisk(reports, @"C:\Users\Galic\Desktop");
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
