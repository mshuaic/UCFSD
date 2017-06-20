using CheckupExec.Controllers;
using CheckupExec.Models;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Analysis
{
    //Seems to work for my job histories - I have 3 on a weekly schedule which produce a negative slope and intercept that fit them well
    //To do: Recurrence values, validate, structure plotting steps and visualization, test
    public class Forecast
    {
        private static int _maxSubsetSize = 120;
        private static int _minSubsetSize = 5;

        private bool _forecastSuccessful;
        public bool ForecastSuccessful
        {
            get
            {
                return _forecastSuccessful;
            }
        }

        public string JobName { get; }

        public string StorageName { get; }

        public long UsedCapacityBytes { get; }

        public long MaxCapacityBytes { get; }

        public string StorageType { get; }

        public string RecurrenceType { get; }

        public Dictionary<double, double> plot;

        private double _finalSlope;
        public double FinalSlope
        {
            get
            {
                return _finalSlope;
            }
        }

        private double _finalIntercept;
        public double FinalIntercept
        {
            get
            {
                return _finalIntercept;
            }
        }

        private static DateTime _currentTime = DateTime.Now;

        public Forecast(string jobId)
        { 
            _forecastSuccessful = true;

            JobController jobController = new JobController();
            JobHistoryController jobHistoryController = new JobHistoryController();
            StorageController storageController = new StorageController();

            var jobPipeline = new Dictionary<string, string>
            {
                ["Id"] = jobId
            };
            var jobHistoryPipeline = new Dictionary<string, Dictionary<string, string>>
            {
                ["get-bejob"] = new Dictionary<string, string>
                                {
                                    ["Id"] = jobId
                                }
            };

            var jobAsList = jobController.GetJobsBy(jobPipeline);
            Job job = null;

            if (jobAsList.Count > 0)
            {
                job = jobAsList.First();
            }

            if (job != null && job.TaskName == "Full")
            {
                var storagePipeline = new Dictionary<string, string>
                {
                    ["Id"] = job.StorageId
                };

                var storageDeviceAsList = storageController.GetStoragesBy(storagePipeline);
                var storageDevice = storageDeviceAsList.First();

                _forecastSuccessful = (storageDevice.StorageType.Equals("0")) ? false : true;

                StorageName = storageDevice.Name;
                UsedCapacityBytes = storageDevice.UsedCapacityBytes;
                MaxCapacityBytes = storageDevice.TotalCapacityBytes;
                StorageType = storageDevice.StorageType;

                RecurrenceType = job.Schedule.getRecurrenceTypeString(job.Schedule.RecurrenceType);

                var jobHistories = jobHistoryController.GetJobHistoriesPipeline(jobHistoryPipeline);

                if (jobHistories != null && _forecastSuccessful)
                {
                    runForecast(jobHistories);
                    populatePlot(jobHistories);
                }
                else
                {
                    _forecastSuccessful = false;
                }
            }
            else
            {
                //Log that forecast was unsuccessful because job type is not full
                _forecastSuccessful = false;
            }
        }

        public Forecast(Storage storageDevice, List<JobHistory> jobHistories)
        {
            _forecastSuccessful = true;

            StorageName = storageDevice.Name;
            UsedCapacityBytes = storageDevice.UsedCapacityBytes;
            MaxCapacityBytes = storageDevice.TotalCapacityBytes;
            StorageType = storageDevice.StorageType;

            if (jobHistories != null && _forecastSuccessful)
            {
                runForecast(jobHistories);
                populatePlot(jobHistories);
            }
            else
            {
                _forecastSuccessful = false;
            }
        }

        private void runForecast(List<JobHistory> jobHistories)
        {
            jobHistories = filterJobs(jobHistories);

            if (!SortingUtility<JobHistory>.isSorted(jobHistories))
            {
                SortingUtility<JobHistory>.sort(jobHistories, 0, jobHistories.Count - 1);
            }

            //switch (RecurrenceType)
            //{
            //    case "Yearly":
            //        maxSubsetSize = 5;
            //        minSubsetSize = 2;
            //        break;
            //    case "Monthly":
            //        maxSubsetSize = 12;
            //        minSubsetSize = 3;
            //        break;
            //    case "Weekly":
            //        maxSubsetSize = 26;
            //        minSubsetSize = 2;
            //        break;
            //    case "Daily":
            //        maxSubsetSize = 90;
            //        minSubsetSize = 10;
            //        break;
            //    case "Hourly":
            //        maxSubsetSize = 24 * 14;
            //        minSubsetSize = 24;
            //        break;
            //    default:
            //        _forecastSuccessful = false;
            //        return;
            //}

            if (jobHistories.Count < _minSubsetSize)
            {
                _forecastSuccessful = false;
            }
            else
            {
                pWLinearRegression(jobHistories, _maxSubsetSize, _minSubsetSize);

                if (_finalSlope < 0)
                {
                    //This implies that the user will never reach their capacity, which is out of place given what we're doing
                    _forecastSuccessful = false;
                }
            }
        }

        private void pWLinearRegression(List<JobHistory> jobHistories, int maxSubsetSize, int minSubsetSize)
        {
            int recentIndex = jobHistories.Count - 1;
            int finalSubsetSize = minSubsetSize;
            int currentSubsetSize = minSubsetSize;
            int boundaryIndex = recentIndex - currentSubsetSize + 1;

            double maxCorr = Double.MinValue;
            double sumy = 0, sumx = 0;
            double candidateSlope = 0, candidateIntercept = 0;

            for (int i = boundaryIndex + 1, j = 1; i <= recentIndex && i >= 0; i++, j++)
            {
               
                sumy += (double)(jobHistories[i].TotalDataSizeBytes >> 20) / 1024;
                
                sumx += jobHistories[i].StartTime.Date.Subtract(_currentTime).TotalDays;
            }

            while (currentSubsetSize <= maxSubsetSize && boundaryIndex >= 0)
            {
                sumy += (double)(jobHistories[boundaryIndex].TotalDataSizeBytes >> 20) / 1024;
                
                sumx += jobHistories[boundaryIndex].StartTime.Date.Subtract(_currentTime).TotalDays;

                double meany = 0;
                double meanx = 0;

                try
                {
                    meany = sumy / currentSubsetSize;
                    meanx = sumx / currentSubsetSize;
                }
                catch (DivideByZeroException e)
                {
                    //log
                }

                double sumdevy = 0, sumdevy2 = 0;
                double sumdevx = 0, sumdevx2 = 0;
                double sumdevyx = 0;

                for (int i = recentIndex; i >= boundaryIndex && i >= 0; i--)
                {
                    double devY = (double)(jobHistories[i].TotalDataSizeBytes >> 20) / 1024 - meany;
                    double devX = jobHistories[i].StartTime.Date.Subtract(_currentTime).TotalDays - meanx;

                    sumdevy += devY;
                    sumdevx += devX;

                    sumdevy2 += Math.Pow(devY, 2);
                    sumdevx2 += Math.Pow(devX, 2);

                    sumdevyx += (devY) * (devX);
                }

                //Math.Sqrt(variance)
                double stdevy = Math.Sqrt(sumdevy2 / (currentSubsetSize - 1));
                double stdevx = Math.Sqrt(sumdevx2 / (currentSubsetSize - 1));

                double corr = 0;

                try
                {
                    corr = (sumdevyx) / Math.Sqrt(sumdevy2 * sumdevx2);
                }
                catch (DivideByZeroException e)
                {
                    //log, although i don't think it can get to this point (corr = NaN if denominator == 0)
                }

                double slope = corr * (stdevy / stdevx);
                double intercept = meany - slope * meanx;

                if (corr > maxCorr)
                {
                    maxCorr = corr;
                    finalSubsetSize = currentSubsetSize;
                    candidateSlope = slope;
                    candidateIntercept = intercept;
                }

                boundaryIndex--;
                currentSubsetSize++;
            }
            
            _finalSlope = candidateSlope;
            _finalIntercept = candidateIntercept;
        }

        private void populatePlot(List<JobHistory> jobHistories)
        {
            foreach (var jobHistory in jobHistories)
            {
                plot[jobHistory.StartTime.Date.Subtract(_currentTime.Date).TotalDays] = (double)(jobHistory.TotalDataSizeBytes << 20) / 1024;
            }
        }

        private List<JobHistory> filterJobs(List<JobHistory> jobHistories)
        {
            var filteredJobHistories = new List<JobHistory>();

            foreach (var jobHistory in jobHistories)
            {
                if (Convert.ToInt32(jobHistory.JobStatus) == JobHistory.SuccessfulFinalStatus && jobHistory.PercentComplete == 100)
                {
                    filteredJobHistories.Add(jobHistory);
                }
            }

            return filteredJobHistories;
        }

        private double Abs(double x)
        {
            return (x > 0) ? x : -x;
        }
    }
}
