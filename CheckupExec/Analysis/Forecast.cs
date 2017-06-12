using CheckupExec.Controllers;
using CheckupExec.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Utilities
{
    //Seems to work for my job histories - I have 3 on a weekly schedule which produce a negative slope and intercept that fit them well
    //To do: Recurrence values, validate, structure plotting steps and visualization, test
    public class Forecast
    {
        private bool _forecastSuccessful;
        public bool ForecastSuccessful
        {
            get
            {
                return _forecastSuccessful;
            }
        }

        public string JobName { get; }

        public string storageName { get; }

        public long usedCapacityBytes { get; }

        public long maxCapacityBytes { get; }

        public string storageType { get; }

        public string RecurrenceType { get; }

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
                ["get-bejob"] = new Dictionary<string, string>()
                                {
                                    ["Id"] = jobId
                                }
            };

            var jobAsList = jobController.GetJobsBy(jobPipeline);
            var job = jobAsList.First<Job>();

            if (job.TaskName == "Full")
            {
                var storagePipeline = new Dictionary<string, string>
                {
                    ["Id"] = job.StorageId
                };

                var storageDeviceAsList = storageController.GetStoragesBy(storagePipeline);
                var storageDevice = storageDeviceAsList.First();

                storageName = storageDevice.Name;
                usedCapacityBytes = storageDevice.UsedCapacityBytes;
                maxCapacityBytes = storageDevice.TotalCapacityBytes;
                storageType = storageDevice.StorageType;

                RecurrenceType = job.Schedule.getRecurrenceTypeString(job.Schedule.RecurrenceType);

                var jobHistories = jobHistoryController.GetJobHistoriesPipeline(jobHistoryPipeline);

                if (jobHistories != null)
                {
                    runForecast(jobHistories);
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

        private void runForecast(List<JobHistory> jobHistories)
        {
            jobHistories = filterJobs(jobHistories);

            if (!isSorted(jobHistories))
            {
                sortJobHistories(jobHistories, 0, jobHistories.Count - 1);
            }

            int maxSubsetSize = 0, minSubsetSize = 0;

            switch (RecurrenceType)
            {
                case "Yearly":
                    maxSubsetSize = 5;
                    minSubsetSize = 2;
                    break;
                case "Monthly":
                    maxSubsetSize = 12;
                    minSubsetSize = 3;
                    break;
                case "Weekly":
                    maxSubsetSize = 26;
                    minSubsetSize = 2;
                    break;
                case "Daily":
                    maxSubsetSize = 90;
                    minSubsetSize = 10;
                    break;
                case "Hourly":
                    maxSubsetSize = 24 * 14;
                    minSubsetSize = 24;
                    break;
                default:
                    _forecastSuccessful = false;
                    return;
            }

            if (jobHistories.Count < minSubsetSize)
            {
                _forecastSuccessful = false;
            }
            else
            {
                pWLinearRegression(jobHistories, maxSubsetSize, minSubsetSize);

                if (_finalSlope < 0)
                {
                    //This implies that the user will never reach their capacity, which is out of place given what we're doing
                    _forecastSuccessful = false;
                }
            }
        }

        private void pWLinearRegression(List<JobHistory> jobHistories, int maxSubsetSize, int minSubsetSize)
        {
            double maxCorr = Double.MinValue;
            double sumy = 0, sumx = 0, sumy2 = 0, sumx2 = 0;
            int recentIndex = jobHistories.Count - 1;
            int finalSubsetSize = minSubsetSize;
            int currentSubsetSize = minSubsetSize;
            int boundaryIndex = recentIndex - currentSubsetSize + 1;
            double candidateSlope = 0, candidateIntercept = 0;

            for (int i = boundaryIndex + 1, j = 1; i <= recentIndex && i >= 0; i++, j++)
            {
                sumy += jobHistories[i].TotalDataSizeBytes >> 30;
                sumy2 += Math.Pow(jobHistories[i].TotalDataSizeBytes >> 30, 2);
                sumx += j;
                sumx2 = Math.Pow(j, 2);
            }

            while (currentSubsetSize <= maxSubsetSize && boundaryIndex >= 0)
            {
                sumy += jobHistories[boundaryIndex].TotalDataSizeBytes >> 30;
                sumy2 += Math.Pow(jobHistories[boundaryIndex].TotalDataSizeBytes >> 30, 2);
                sumx += currentSubsetSize;
                sumx2 += Math.Pow(currentSubsetSize, 2);

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

                double sumdevy = 0, vary = 0;
                double sumdevx = 0, varx = 0;
                double sumdevyx = 0;

                for (int i = boundaryIndex, l = 1; i <= recentIndex && i >= 0; i++, l++)
                {
                    sumdevy += (jobHistories[i].TotalDataSizeBytes >> 30) - meany;
                    vary += Math.Pow(Abs((jobHistories[i].TotalDataSizeBytes >> 30) - meany), 2);
                    sumdevx += l - meanx;
                    varx += Math.Pow(Abs(l - meanx), 2);
                    sumdevyx += sumdevy * sumdevx;
                }

                double stdevy = Math.Sqrt(vary);
                double stdevx = Math.Sqrt(varx);

                double corr = 0;

                try
                {
                    corr = (sumdevyx) / Math.Sqrt(vary * varx);
                }
                catch (DivideByZeroException e)
                {
                    //log
                }

                double slope = corr * (vary / varx);
                double intercept = meany - slope * meanx;

                if (corr > maxCorr)
                {
                    maxCorr = corr;
                    finalSubsetSize = recentIndex - boundaryIndex;
                    candidateSlope = slope;
                    candidateIntercept = intercept;
                }

                boundaryIndex--;
                currentSubsetSize++;
            }

            _finalSlope = candidateSlope;
            _finalIntercept = candidateIntercept;
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

        private bool isSorted(List<JobHistory> jobHistories)
        {
            int count = jobHistories.Count;
            for (int i = 0; i < count - 1; i++)
            {
                if (jobHistories[i].CompareTo(jobHistories[i + 1]) == 1)
                {
                    return false;
                }
            }
            return true;
        }

        private void sortJobHistories(List<JobHistory> jobHistories, int left, int right)
        {
            int i = left, j = right;
            JobHistory pivot = jobHistories[(left + right) / 2];

            while (i <= j)
            {
                while (jobHistories[i].CompareTo(pivot) < 0)
                {
                    i++;
                }
                while (jobHistories[j].CompareTo(pivot) > 0)
                {
                    j--;
                }
                if (i <= j)
                {
                    swap(jobHistories[i], jobHistories[j]);
                    i++;
                    j--;
                }
            }

            if (left < j)
            {
                sortJobHistories(jobHistories, left, j);
            }
            if (i < right)
            {
                sortJobHistories(jobHistories, i, right);
            }
        }

        private void swap(JobHistory a, JobHistory b)
        {
            JobHistory temp = a;
            a = b;
            b = temp;
        }

        private double Abs(double x)
        {
            return (x > 0) ? x : -x;
        }
    }
}
