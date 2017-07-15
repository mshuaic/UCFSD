using CheckupExec.Models;
using CheckupExec.Models.AnalysisModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Utilities
{
    //really only generic enough for JobHistories and DiskCapacities
    public class Forecast<T> where T : IComparable<T>
    {
        //JobHistories subset 
        private const int _maxSubsetSizeBE = 120;
        private const int _minSubsetSizeBE = 5;

        //DiskCapacities subset 
        private const int _maxSubsetSizeDC = 100;
        private const int _minSubsetSizeDC = 5;

        private DateTime _currentTime;

        private ForecastResults _forecastResults = new ForecastResults();

        public Forecast()
        {
            _currentTime = DateTime.Now;
        }

        public ForecastResults doForecast(List<JobHistory> jobHistories)
        {
            _forecastResults.ForecastSuccessful = true;
            _forecastResults.isDiskForecast     = false;
            _forecastResults.plot               = new List<PlotPoint>();

            if (jobHistories != null && jobHistories.Count > 0)
            {
                runForecast(jobHistories);
                if (_forecastResults.ForecastSuccessful)
                {
                    populatePlot(jobHistories);
                }
            }
            else
            {
                _forecastResults.ForecastSuccessful = false;
            }

            return _forecastResults;
        }

        public ForecastResults doForecast(List<UsedCapacity> usedCapacities)
        {
            _forecastResults.ForecastSuccessful = true;
            _forecastResults.isDiskForecast     = true;
            _forecastResults.plot               = new List<PlotPoint>();

            if (usedCapacities != null && usedCapacities.Count > 0)
            {
                runForecast(usedCapacities);
                if (_forecastResults.ForecastSuccessful)
                {
                    populatePlot(usedCapacities);
                }
            }
            else
            {
                _forecastResults.ForecastSuccessful = false;
            }

            return _forecastResults;
        }

        private void runForecast(List<JobHistory> jobHistories)
        {
            //*Would be nice to do this, but not worthwhile if the enums cannot be obtained/correlated with bemcli*
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

            // && false for testing with our sets
            //if we meet minimum subset rqmnts, run forecast
            if (jobHistories.Count < _minSubsetSizeBE && false)
            {
                _forecastResults.ForecastSuccessful = false;
            }
            else
            {
                pWLinearRegression(jobHistories, _maxSubsetSizeBE, _minSubsetSizeBE);

                //This implies that the user will never reach their capacity, which is out of place given what we're doing
                _forecastResults.ForecastSuccessful = (_forecastResults.FinalSlope < 0) ? false : true;
            }
        }

        private void runForecast(List<UsedCapacity> usedCapacities)
        {
            //if we meet minimum subset rqmnts, run forecast
            if (usedCapacities.Count < _minSubsetSizeDC)
            {
                _forecastResults.ForecastSuccessful = false;
            }
            else
            {
                pWLinearRegression(usedCapacities, _maxSubsetSizeDC, _minSubsetSizeDC);

                //This implies that the user will never reach their capacity, which is out of place given what we're doing
                _forecastResults.ForecastSuccessful = (_forecastResults.FinalSlope < 0) ? false : true;
            }
        }

        //piecewise simple linear regression (method of ord least squares) is run on each subset starting at minimum and incrementing by 
        //one instance of JobHistory until we 1) run out of instances or 2) reach maximum subset size. 
        //Ultimately, the subset that had the highest pearson correlation is chosen (slope and intercept).
        private void pWLinearRegression(List<JobHistory> jobHistories, int maxSubsetSize, int minSubsetSize)
        {
            int recentIndex       = jobHistories.Count - 1;
            int finalSubsetSize   = minSubsetSize;
            int currentSubsetSize = 2;// minSubsetSize;
            int boundaryIndex     = recentIndex - currentSubsetSize + 1;

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
                catch
                {
                    //log
                }

                double sumdevy  = 0, sumdevy2 = 0;
                double sumdevx  = 0, sumdevx2 = 0;
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

                double stdevy = 0;
                double stdevx = 0;

                try
                {
                    //Math.Sqrt(variance)
                    stdevy = Math.Sqrt(sumdevy2 / (currentSubsetSize - 1));
                    stdevx = Math.Sqrt(sumdevx2 / (currentSubsetSize - 1));
                }
                catch
                {
                    //log
                }

                double corr = 0;

                try
                {
                    corr = (sumdevyx) / Math.Sqrt(sumdevy2 * sumdevx2);
                }
                catch
                {
                    //log, although i don't think it can get to this point (corr = NaN if denominator == 0)
                }

                double slope     = 0;
                double intercept = 0;

                try
                {
                    slope     = corr * (stdevy / stdevx);
                    intercept = meany - slope * meanx;
                }
                catch
                {
                    //log
                }
                if (corr > maxCorr)
                {
                    maxCorr            = corr;
                    finalSubsetSize    = currentSubsetSize;
                    candidateSlope     = slope;
                    candidateIntercept = intercept;
                }

                boundaryIndex--;
                currentSubsetSize++;
            }

            _forecastResults.FinalSlope     = candidateSlope;
            _forecastResults.FinalIntercept = candidateIntercept;
        }

        //piecewise simple linear regression (method of ord least squares) is run on each subset starting at minimum and incrementing by 
        //one instance of DiskCapacity until we 1) run out of instances or 2) reach maximum subset size. 
        //Ultimately, the subset that had the highest pearson correlation is chosen (slope and intercept).
        private void pWLinearRegression(List<UsedCapacity> usedCapacities, int maxSubsetSize, int minSubsetSize)
        {
            int recentIndex       = usedCapacities.Count - 1;
            int finalSubsetSize   = minSubsetSize;
            int currentSubsetSize = minSubsetSize;
            int boundaryIndex     = recentIndex - currentSubsetSize + 1;

            double maxCorr = Double.MinValue;
            double sumy = 0, sumx = 0;
            double candidateSlope = 0, candidateIntercept = 0;

            for (int i = boundaryIndex + 1, j = 1; i <= recentIndex && i >= 0; i++, j++)
            {

                sumy += (double)(usedCapacities[i].Bytes >> 20) / 1024;

                sumx += usedCapacities[i].Date.Date.Subtract(_currentTime).TotalDays;
            }

            while (currentSubsetSize <= maxSubsetSize && boundaryIndex >= 0)
            {
                sumy += (double)(usedCapacities[boundaryIndex].Bytes >> 20) / 1024;

                sumx += usedCapacities[boundaryIndex].Date.Date.Subtract(_currentTime).TotalDays;

                double meany = 0;
                double meanx = 0;

                try
                {
                    meany = sumy / currentSubsetSize;
                    meanx = sumx / currentSubsetSize;
                }
                catch
                {
                    //log
                }

                double sumdevy  = 0, sumdevy2 = 0;
                double sumdevx  = 0, sumdevx2 = 0;
                double sumdevyx = 0;

                for (int i = recentIndex; i >= boundaryIndex && i >= 0; i--)
                {
                    double devY = (double)(usedCapacities[i].Bytes >> 20) / 1024 - meany;
                    double devX = usedCapacities[i].Date.Date.Subtract(_currentTime).TotalDays - meanx;

                    sumdevy += devY;
                    sumdevx += devX;

                    sumdevy2 += Math.Pow(devY, 2);
                    sumdevx2 += Math.Pow(devX, 2);

                    sumdevyx += (devY) * (devX);
                }

                double stdevy = 0;
                double stdevx = 0;

                try
                {
                    //Math.Sqrt(variance)
                    stdevy = Math.Sqrt(sumdevy2 / (currentSubsetSize - 1));
                    stdevx = Math.Sqrt(sumdevx2 / (currentSubsetSize - 1));
                }
                catch
                {
                    //log
                }

                double corr = 0;

                try
                {
                    corr = (sumdevyx) / Math.Sqrt(sumdevy2 * sumdevx2);
                }
                catch
                {
                    //log, although i don't think it can get to this point (corr = NaN if denominator == 0)
                }

                double slope     = 0;
                double intercept = 0;

                try
                {
                    slope     = corr * (stdevy / stdevx);
                    intercept = meany - slope * meanx;
                }
                catch
                {
                    //log
                }

                if (corr > maxCorr)
                {
                    maxCorr            = corr;
                    finalSubsetSize    = currentSubsetSize;
                    candidateSlope     = slope;
                    candidateIntercept = intercept;
                }

                boundaryIndex--;
                currentSubsetSize++;
            }

            _forecastResults.FinalSlope     = candidateSlope;
            _forecastResults.FinalIntercept = candidateIntercept;
        }

        //generate our plot will ALL instances (this is for visualization)
        private void populatePlot(List<JobHistory> jobHistories)
        {
            foreach (JobHistory jobHistory in jobHistories)
            {
                _forecastResults.plot.Add(new PlotPoint
                {
                    Days = jobHistory.StartTime.Date.Subtract(_currentTime.Date).TotalDays,
                    GB   = (double)(jobHistory.TotalDataSizeBytes >> 20) / 1024
                });
            }
        }

        //generate our plot with ALL instances (this is for visualization)
        private void populatePlot(List<UsedCapacity> usedCapacities)
        {
            foreach (var usedCapacity in usedCapacities)
            {
                _forecastResults.plot.Add(new PlotPoint
                {
                    Days = usedCapacity.Date.Date.Subtract(_currentTime.Date).TotalDays,
                    GB   = (double)(usedCapacity.Bytes >> 20) / 1024
                });
            }
        }

        private double Abs(double x)
        {
            return (x > 0) ? x : -x;
        }
    }
}
