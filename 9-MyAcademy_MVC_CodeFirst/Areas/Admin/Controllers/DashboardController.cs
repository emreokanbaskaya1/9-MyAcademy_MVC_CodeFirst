using _9_MyAcademy_MVC_CodeFirst.Data.Context;
using _9_MyAcademy_MVC_CodeFirst.Data.Entities;
using _9_MyAcademy_MVC_CodeFirst.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace _9_MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();
        private readonly RevenueForecastService _forecastService = new RevenueForecastService();

        public ActionResult Index()
        {
            // Existing stats
            ViewBag.TotalCategories = _context.Categories.Count();
            ViewBag.TotalProducts = _context.Products.Count();
            ViewBag.TotalBlogs = _context.Blogs.Count();
            ViewBag.TotalTestimonials = _context.Testimonials.Count();

            // Policy Sales Stats
            var allPolicySales = _context.PolicySales.ToList();
            var activePolicySales = allPolicySales.Where(p => p.IsActive).ToList();

            ViewBag.TotalPolicySales = allPolicySales.Count;
            ViewBag.ActivePolicySales = activePolicySales.Count;
            ViewBag.TotalRevenue = activePolicySales.Sum(p => p.SaleAmount);
            ViewBag.TotalRevenueFormatted = activePolicySales.Sum(p => p.SaleAmount).ToString("C0");

            // Payment Status Stats
            ViewBag.PaidCount = activePolicySales.Count(p => p.PaymentStatus == PaymentStatus.Paid);
            ViewBag.PendingCount = activePolicySales.Count(p => p.PaymentStatus == PaymentStatus.Pending);
            ViewBag.PartiallyPaidCount = activePolicySales.Count(p => p.PaymentStatus == PaymentStatus.PartiallyPaid);
            
            // Policy Status Stats
            ViewBag.ActivePolicyCount = activePolicySales.Count(p => p.PolicyStatus == PolicyStatus.Active);
            ViewBag.ExpiredPolicyCount = activePolicySales.Count(p => p.PolicyStatus == PolicyStatus.Expired);
            ViewBag.CancelledPolicyCount = activePolicySales.Count(p => p.PolicyStatus == PolicyStatus.Cancelled);
            
            // Monthly Sales (Last 12 months for better forecast)
            var twelveMonthsAgo = DateTime.Now.AddMonths(-12);
            var monthlySales = activePolicySales
                .Where(p => p.CreatedDate >= twelveMonthsAgo)
                .GroupBy(p => new { p.CreatedDate.Year, p.CreatedDate.Month })
                .Select(g => new RevenueForecastService.MonthlyRevenue
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(p => p.SaleAmount),
                    SalesCount = g.Count()
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToList();

            // Last 6 months for the existing chart
            var sixMonthData = monthlySales.Skip(Math.Max(0, monthlySales.Count - 6)).ToList();
            ViewBag.MonthlyLabels = string.Join(",", sixMonthData.Select(m => $"'{m.Label}'"));
            ViewBag.MonthlyCounts = string.Join(",", sixMonthData.Select(m => m.SalesCount));
            ViewBag.MonthlyRevenue = string.Join(",", sixMonthData.Select(m => m.Revenue));

            // Top 5 Products by Sales
            var topProducts = activePolicySales
                .Where(p => p.Product != null)
                .GroupBy(p => p.Product.Name)
                .Select(g => new
                {
                    ProductName = g.Key,
                    Count = g.Count(),
                    Revenue = g.Sum(p => p.SaleAmount)
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();

            ViewBag.TopProductNames = string.Join(",", topProducts.Select(p => $"'{p.ProductName}'"));
            ViewBag.TopProductCounts = string.Join(",", topProducts.Select(p => p.Count));

            // Recent Sales (Last 5)
            ViewBag.RecentSales = activePolicySales
                .OrderByDescending(p => p.CreatedDate)
                .Take(5)
                .ToList();

            // ML Revenue Forecast
            var forecast = _forecastService.ForecastNextMonth(monthlySales);
            
            // Forecast chart data: historical revenue + predicted
            var forecastLabels = new List<string>();
            var forecastActual = new List<string>();
            var forecastPredicted = new List<string>();

            foreach (var month in sixMonthData)
            {
                forecastLabels.Add($"'{month.Label}'");
                forecastActual.Add(month.Revenue.ToString("F2", System.Globalization.CultureInfo.InvariantCulture));
                forecastPredicted.Add("null");
            }

            // Add predicted month
            if (forecast.PredictedMonth != null)
            {
                forecastLabels.Add($"'{forecast.PredictedMonth.Label}'");
                forecastActual.Add("null");
                forecastPredicted.Add(forecast.PredictedRevenue.ToString("F2", System.Globalization.CultureInfo.InvariantCulture));
            }

            // Connect last actual to prediction with a bridge point
            if (sixMonthData.Any() && forecast.PredictedMonth != null)
            {
                // Replace the last predicted null with the last actual value to draw connecting line
                forecastPredicted[forecastPredicted.Count - 2] = sixMonthData.Last().Revenue.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            }

            ViewBag.ForecastLabels = string.Join(",", forecastLabels);
            ViewBag.ForecastActual = string.Join(",", forecastActual);
            ViewBag.ForecastPredicted = string.Join(",", forecastPredicted);
            ViewBag.ForecastPredictedRevenue = forecast.PredictedRevenue;
            ViewBag.ForecastPredictedRevenueFormatted = forecast.PredictedRevenue.ToString("C0");
            ViewBag.ForecastPredictedCount = forecast.PredictedSalesCount;
            ViewBag.ForecastConfidenceLower = forecast.ConfidenceLower;
            ViewBag.ForecastConfidenceLowerFormatted = forecast.ConfidenceLower.ToString("C0");
            ViewBag.ForecastConfidenceUpper = forecast.ConfidenceUpper;
            ViewBag.ForecastConfidenceUpperFormatted = forecast.ConfidenceUpper.ToString("C0");
            ViewBag.ForecastRSquared = forecast.RSquared;
            ViewBag.ForecastRSquaredFormatted = (forecast.RSquared * 100).ToString("F2");
            ViewBag.ForecastTrendDirection = forecast.TrendDirection;
            ViewBag.ForecastTrendPercentage = forecast.TrendPercentage;
            ViewBag.ForecastNextMonthLabel = forecast.PredictedMonth?.Label ?? "N/A";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
