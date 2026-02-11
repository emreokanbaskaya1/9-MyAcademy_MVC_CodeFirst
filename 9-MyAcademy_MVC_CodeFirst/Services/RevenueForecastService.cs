using System;
using System.Collections.Generic;
using System.Linq;

namespace _9_MyAcademy_MVC_CodeFirst.Services
{
    /// <summary>
    /// Simple revenue forecasting service using linear regression.
    /// Predicts next month's revenue based on historical monthly data.
    /// </summary>
    public class RevenueForecastService
    {
        /// <summary>
        /// Represents a single month's revenue data point.
        /// </summary>
        public class MonthlyRevenue
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public decimal Revenue { get; set; }
            public int SalesCount { get; set; }
            public string Label => $"{Year}-{Month:D2}";
        }

        /// <summary>
        /// Result of the forecast calculation.
        /// </summary>
        public class ForecastResult
        {
            public List<MonthlyRevenue> HistoricalData { get; set; }
            public MonthlyRevenue PredictedMonth { get; set; }
            public decimal PredictedRevenue { get; set; }
            public int PredictedSalesCount { get; set; }
            public decimal ConfidenceLower { get; set; }
            public decimal ConfidenceUpper { get; set; }
            public double RSquared { get; set; }
            public string TrendDirection { get; set; }
            public decimal TrendPercentage { get; set; }
        }

        /// <summary>
        /// Performs linear regression forecast for next month's revenue.
        /// Uses Ordinary Least Squares (OLS) method.
        /// </summary>
        public ForecastResult ForecastNextMonth(List<MonthlyRevenue> historicalData)
        {
            var result = new ForecastResult
            {
                HistoricalData = historicalData
            };

            // Calculate next month
            var lastMonth = historicalData.LastOrDefault();
            if (lastMonth != null)
            {
                var nextDate = new DateTime(lastMonth.Year, lastMonth.Month, 1).AddMonths(1);
                result.PredictedMonth = new MonthlyRevenue
                {
                    Year = nextDate.Year,
                    Month = nextDate.Month
                };
            }
            else
            {
                var now = DateTime.Now;
                result.PredictedMonth = new MonthlyRevenue
                {
                    Year = now.Year,
                    Month = now.Month
                };
            }

            if (historicalData.Count < 2)
            {
                // Not enough data for regression, use average or last value
                var avgRevenue = historicalData.Any() ? historicalData.Average(d => d.Revenue) : 0;
                var avgCount = historicalData.Any() ? (int)historicalData.Average(d => d.SalesCount) : 0;
                result.PredictedRevenue = avgRevenue;
                result.PredictedSalesCount = avgCount;
                result.ConfidenceLower = avgRevenue * 0.8m;
                result.ConfidenceUpper = avgRevenue * 1.2m;
                result.RSquared = 0;
                result.TrendDirection = "stable";
                result.TrendPercentage = 0;
                return result;
            }

            // Linear Regression for Revenue
            int n = historicalData.Count;
            double[] x = Enumerable.Range(1, n).Select(i => (double)i).ToArray();
            double[] yRevenue = historicalData.Select(d => (double)d.Revenue).ToArray();
            double[] yCounts = historicalData.Select(d => (double)d.SalesCount).ToArray();

            // Revenue prediction
            double slopeRevenue, interceptRevenue, rSquaredRevenue;
            CalculateLinearRegression(x, yRevenue, out slopeRevenue, out interceptRevenue, out rSquaredRevenue);

            double predictedRevenue = interceptRevenue + slopeRevenue * (n + 1);
            if (predictedRevenue < 0) predictedRevenue = 0;

            // Sales count prediction
            double slopeCounts, interceptCounts, rSquaredCounts;
            CalculateLinearRegression(x, yCounts, out slopeCounts, out interceptCounts, out rSquaredCounts);

            double predictedCounts = interceptCounts + slopeCounts * (n + 1);
            if (predictedCounts < 0) predictedCounts = 0;

            // Standard error for confidence interval
            double stdError = CalculateStandardError(x, yRevenue, slopeRevenue, interceptRevenue);
            double margin = 1.96 * stdError; // 95% confidence

            result.PredictedRevenue = (decimal)Math.Round(predictedRevenue, 2);
            result.PredictedSalesCount = (int)Math.Round(predictedCounts);
            result.ConfidenceLower = (decimal)Math.Max(0, Math.Round(predictedRevenue - margin, 2));
            result.ConfidenceUpper = (decimal)Math.Round(predictedRevenue + margin, 2);
            result.RSquared = Math.Round(rSquaredRevenue, 4);

            // Trend calculation
            if (historicalData.Count >= 2)
            {
                var lastRevenue = historicalData.Last().Revenue;
                if (lastRevenue > 0)
                {
                    var change = ((result.PredictedRevenue - lastRevenue) / lastRevenue) * 100;
                    result.TrendPercentage = Math.Round(change, 1);
                    result.TrendDirection = change > 2 ? "up" : (change < -2 ? "down" : "stable");
                }
                else
                {
                    result.TrendDirection = "up";
                    result.TrendPercentage = 100;
                }
            }

            return result;
        }

        private void CalculateLinearRegression(double[] x, double[] y, out double slope, out double intercept, out double rSquared)
        {
            int n = x.Length;
            double sumX = x.Sum();
            double sumY = y.Sum();
            double sumXY = x.Zip(y, (a, b) => a * b).Sum();
            double sumX2 = x.Sum(a => a * a);
            double sumY2 = y.Sum(a => a * a);

            double denominator = (n * sumX2 - sumX * sumX);
            if (Math.Abs(denominator) < 0.0001)
            {
                slope = 0;
                intercept = sumY / n;
                rSquared = 0;
                return;
            }

            slope = (n * sumXY - sumX * sumY) / denominator;
            intercept = (sumY - slope * sumX) / n;

            // R-squared
            double meanY = sumY / n;
            double ssTotal = y.Sum(yi => (yi - meanY) * (yi - meanY));
            double ssResidual = 0;
            for (int i = 0; i < n; i++)
            {
                double predicted = intercept + slope * x[i];
                ssResidual += (y[i] - predicted) * (y[i] - predicted);
            }

            rSquared = ssTotal > 0 ? 1 - (ssResidual / ssTotal) : 0;
            if (rSquared < 0) rSquared = 0;
        }

        private double CalculateStandardError(double[] x, double[] y, double slope, double intercept)
        {
            int n = x.Length;
            if (n <= 2) return 0;

            double sumSquaredResiduals = 0;
            for (int i = 0; i < n; i++)
            {
                double predicted = intercept + slope * x[i];
                sumSquaredResiduals += (y[i] - predicted) * (y[i] - predicted);
            }

            return Math.Sqrt(sumSquaredResiduals / (n - 2));
        }
    }
}
