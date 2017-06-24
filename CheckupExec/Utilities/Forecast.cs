using CheckupExec.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Utilities
{
    public class Forecast<T> where T : IComparable<T>
    {
        private const int _maxSubsetSize = 120;
        private const int _minSubsetSize = 5;

        private static DateTime _currentTime = DateTime.Now;

        private ForecastResults _forecastResults = new ForecastResults();

        public ForecastResults doForecast(List<JobHistory> jobHistories)
        {
            _forecastResults.ForecastSuccessful = true;
            _forecastResults.isDiskForecast = false;

            if (jobHistories != null)
            {
                runForecast(jobHistories);
                populatePlot(jobHistories);
            }
            else
            {
                _forecastResults.ForecastSuccessful = false;
            }

            return _forecastResults;
        }

        public ForecastResults doForecast(List<DiskCapacity> diskCapacities)
        {
            _forecastResults.ForecastSuccessful = true;
            _forecastResults.isDiskForecast = false;

            if (diskCapacities != null)
            {
                runForecast(diskCapacities);
                populatePlot(diskCapacities);
            }
            else
            {
                _forecastResults.ForecastSuccessful = false;
            }

            return _forecastResults;
        }

        private void runForecast(List<JobHistory> jobHistories)
        {
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
                _forecastResults.ForecastSuccessful = false;
            }
            else
            {
                pWLinearRegression(jobHistories, _maxSubsetSize, _minSubsetSize);

                if (_forecastResults.FinalSlope < 0)
                {
                    //This implies that the user will never reach their capacity, which is out of place given what we're doing
                    _forecastResults.ForecastSuccessful = false;
                }
            }
        }

        private void runForecast(List<DiskCapacity> diskCapacities)
        {
            if (diskCapacities.Count < _minSubsetSize)
            {
                _forecastResults.ForecastSuccessful = false;
            }
            else
            {
                pWLinearRegression(diskCapacities, _maxSubsetSize, _minSubsetSize);

                if (_forecastResults.FinalSlope < 0)
                {
                    //This implies that the user will never reach their capacity, which is out of place given what we're doing
                    _forecastResults.ForecastSuccessful = false;
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

            _forecastResults.FinalSlope = candidateSlope;
            _forecastResults.FinalIntercept = candidateIntercept;
        }

        private void pWLinearRegression(List<DiskCapacity> diskCapacities, int maxSubsetSize, int minSubsetSize)
        {
            int recentIndex = diskCapacities.Count - 1;
            int finalSubsetSize = minSubsetSize;
            int currentSubsetSize = minSubsetSize;
            int boundaryIndex = recentIndex - currentSubsetSize + 1;

            double maxCorr = Double.MinValue;
            double sumy = 0, sumx = 0;
            double candidateSlope = 0, candidateIntercept = 0;

            for (int i = boundaryIndex + 1, j = 1; i <= recentIndex && i >= 0; i++, j++)
            {

                sumy += (double)(diskCapacities[i].Bytes >> 20) / 1024;

                sumx += diskCapacities[i].Date.Date.Subtract(_currentTime).TotalDays;
            }

            while (currentSubsetSize <= maxSubsetSize && boundaryIndex >= 0)
            {
                sumy += (double)(diskCapacities[boundaryIndex].Bytes >> 20) / 1024;

                sumx += diskCapacities[boundaryIndex].Date.Date.Subtract(_currentTime).TotalDays;

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
                    double devY = (double)(diskCapacities[i].Bytes >> 20) / 1024 - meany;
                    double devX = diskCapacities[i].Date.Date.Subtract(_currentTime).TotalDays - meanx;

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

            _forecastResults.FinalSlope = candidateSlope;
            _forecastResults.FinalIntercept = candidateIntercept;
        }

        private void populatePlot(List<JobHistory> jobHistories)
        {
            foreach (var jobHistory in jobHistories)
            {
                _forecastResults.plot[jobHistory.StartTime.Date.Subtract(_currentTime.Date).TotalDays] = (double)(jobHistory.TotalDataSizeBytes << 20) / 1024;
            }
        }

        private void populatePlot(List<DiskCapacity> diskCapacities)
        {
            foreach (var diskCapacity in diskCapacities)
            {
                _forecastResults.plot[diskCapacity.Date.Date.Subtract(_currentTime.Date).TotalDays] = (double)(diskCapacity.Bytes << 20) / 1024;
            }
        }

        private double Abs(double x)
        {
            return (x > 0) ? x : -x;
        }
    }
}
