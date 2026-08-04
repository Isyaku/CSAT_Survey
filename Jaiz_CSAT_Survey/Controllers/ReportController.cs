using Jaiz_CSAT_Survey.Data;
using Jaiz_CSAT_Survey.Models;
using Jaiz_CSAT_Survey.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;



namespace Jaiz_CSAT_Survey.Controllers
{
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!SetSessionData())
            {
                return RedirectToAction("Login", "Home");
            }

            var surveys = await _context.SurveyResponses.OrderByDescending(x => x.SubmittedAt).ToListAsync();

            var vm = BuildDashboard(surveys);

            return View(vm);
        }

        public async Task<IActionResult> Global()
        {
            if (!SetSessionData())
            {
                return RedirectToAction("Login", "Home");
            }

            var surveys = await _context.SurveyResponses.OrderByDescending(x => x.SubmittedAt).ToListAsync();

            var vm = BuildDashboard(surveys);

            return View(vm);
        }

        private DashboardViewModel BuildDashboard(List<SurveyResponse> surveys)
        {
            var vm = new DashboardViewModel();

            int totalResponses = surveys.Count;


            int lowRatingCount = surveys.Count(x => (x.ServiceSatisfaction >= 1 && x.ServiceSatisfaction <= 2) || (x.StaffProfessionalism >= 1 && x.StaffProfessionalism <= 2) || (x.BranchAmbience >= 1 && x.BranchAmbience <= 2) || 
            (x.RecommendationLikelihood <= 2) || (x.WebRating >= 1 && x.WebRating <= 2) || (x.IssueResolution >= 1 && x.IssueResolution <= 2) || (x.TransactionEase >= 1 && x.TransactionEase <= 2) || (x.ProductRating >= 1 && x.ProductRating <= 2)
);

            int promoters = surveys.Count(x => x.RecommendationLikelihood >= 9);

            int passives = surveys.Count(x => x.RecommendationLikelihood >= 7 && x.RecommendationLikelihood <= 8);

            int detractors = surveys.Count(x => x.RecommendationLikelihood <= 6);

            decimal promoterPercent = totalResponses == 0 ? 0 : (decimal)promoters / totalResponses * 100;

            decimal detractorPercent = totalResponses == 0 ? 0 : (decimal)detractors / totalResponses * 100;

            decimal npsScore = promoterPercent - detractorPercent;

            vm.TotalResponses = totalResponses;
            vm.LowRatingCount = lowRatingCount;
            vm.NPSScore = Math.Round(npsScore, 1);

            vm.Promoters = promoters;
            vm.Passives = passives;
            vm.Detractors = detractors;

            //Branch QR
            var branchSurvey = surveys.Where(x => x.SurveyType == "BranchQR").ToList();

            vm.BranchTotalResponses = branchSurvey.Count();

            vm.BranchCSATScore = branchSurvey.Any() ? Math.Round((decimal)branchSurvey.Count(x => x.ServiceSatisfaction >= 4) / branchSurvey.Count() * 100, 2) : 0;
            vm.BranchNPSScore = branchSurvey.Any() ? Math.Round((((decimal)branchSurvey.Count(x => x.RecommendationLikelihood >= 9) / branchSurvey.Count()) - ((decimal)branchSurvey.Count(x => x.RecommendationLikelihood <= 6) / branchSurvey.Count())) * 100, 2) : 0;


            vm.BranchCSATAverageRating = branchSurvey.Any() ? Math.Round(branchSurvey.Average(x => ((x.ServiceSatisfaction ?? 0) + (x.StaffProfessionalism ?? 0) + (x.BranchAmbience ?? 0)) / 3m), 2) : 0;
            vm.BranchNPSAverageRating = (decimal)(branchSurvey.Any() ? Math.Round(branchSurvey.Average(x => x.RecommendationLikelihood ?? 0), 2) : 0);


            vm.BranchLowRatingCount = branchSurvey.Count(x => x.ServiceSatisfaction <= 2 || x.StaffProfessionalism <= 2 || x.BranchAmbience <= 2 || x.RecommendationLikelihood <= 2);


            //Email Survey
            var emailSurvey = surveys.Where(x => x.SurveyType == "EmailAlert");

            vm.EmailTotalResponses = emailSurvey.Count();

            vm.EmailCSATScore = emailSurvey.Any() ? Math.Round((decimal)emailSurvey.Count(x => x.ServiceSatisfaction >= 4) / emailSurvey.Count() * 100, 2) : 0;
            vm.EmailNPSScore = emailSurvey.Any() ? Math.Round((((decimal)emailSurvey.Count(x => x.RecommendationLikelihood >= 9) / emailSurvey.Count()) - ((decimal)emailSurvey.Count(x => x.RecommendationLikelihood <= 6) / emailSurvey.Count())) * 100, 2) : 0;

            vm.EmailCSATAverageRating = emailSurvey.Any() ? Math.Round(emailSurvey.Average(x => ((x.ServiceSatisfaction ?? 0) + (x.TransactionEase ?? 0) + (x.ProductRating ?? 0)) / 3m), 2) : 0;
            vm.EmailNPSAverageRating = (decimal)(emailSurvey.Any() ? Math.Round(emailSurvey.Average(x => x.RecommendationLikelihood ?? 0), 2) : 0);


            vm.EmailLowRatingCount = emailSurvey.Count(x => x.ServiceSatisfaction <= 2 || x.TransactionEase <= 2 || x.ProductRating <= 2 || x.RecommendationLikelihood <= 2);

            //Other Channels
            var otherSurvey = surveys.Where(x => x.SurveyType == "OtherChannels");

            vm.OtherTotalResponses = otherSurvey.Count();

            vm.OtherCSATScore = otherSurvey.Any() ? Math.Round((decimal)otherSurvey.Count(x => x.ServiceSatisfaction >= 4) / otherSurvey.Count() * 100, 2) : 0;
            vm.OtherNPSScore = otherSurvey.Any() ? Math.Round((((decimal)otherSurvey.Count(x => x.RecommendationLikelihood >= 9) / otherSurvey.Count()) - ((decimal)otherSurvey.Count(x => x.RecommendationLikelihood <= 6) / otherSurvey.Count())) * 100, 2) : 0;


            vm.OtherCSATAverageRating = otherSurvey.Any() ? Math.Round(otherSurvey.Average(x => ((x.ServiceSatisfaction ?? 0) + (x.WebRating ?? 0) + (x.IssueResolution ?? 0)) / 3m), 2) : 0;
            vm.OtherNPSAverageRating = (decimal)(otherSurvey.Any() ? Math.Round(otherSurvey.Average(x => x.RecommendationLikelihood ?? 0), 2) : 0);


            vm.OtherLowRatingCount = otherSurvey.Count(x => x.ServiceSatisfaction <= 2 || x.WebRating <= 2 || x.IssueResolution <= 2 || x.RecommendationLikelihood <= 2);


            vm.Channels = surveys.GroupBy(x => string.IsNullOrWhiteSpace(x.SurveyType) ? "Unknown" : x.SurveyType).Select(g => new ChannelSummaryViewModel
            {
                Channel = g.Key,
                TotalResponses = g.Count()
            }).OrderByDescending(x => x.TotalResponses).ToList();

            vm.BranchPerformance = surveys.Where(x => !string.IsNullOrWhiteSpace(x.Branch)).GroupBy(x => x.Branch).Select(g => new BranchPerformanceViewModel
            {
                BranchName = g.Key,
                SurveyCount = g.Count(),
                AverageRating = Math.Round(g.Average(x => ((x.ServiceSatisfaction ?? 0) + (x.StaffProfessionalism ?? 0) + (x.BranchAmbience ?? 0) + (x.RecommendationLikelihood ?? 0)) / 4m), 2),
                CSAT = Math.Round((decimal)g.Count(x => x.ServiceSatisfaction >= 4) / g.Count() * 100, 2),
                NPS = Math.Round((((decimal)g.Count(x => x.RecommendationLikelihood >= 9) / g.Count()) - ((decimal)g.Count(x => x.RecommendationLikelihood <= 6) / g.Count())) * 100, 2),
                LowRatings = g.Count(x => x.ServiceSatisfaction <= 2 || x.StaffProfessionalism <= 2 || x.BranchAmbience <= 2 || x.RecommendationLikelihood <= 6)
            }).OrderByDescending(x => x.CSAT).ThenByDescending(x => x.SurveyCount).ToList();

            vm.RecentResponses = surveys.OrderByDescending(x => x.SubmittedAt).Take(100).Select(x =>
            {
                var ratings = new decimal?[]
                {
                    x.ServiceSatisfaction,
                    x.StaffProfessionalism,
                    x.BranchAmbience,
                    x.RecommendationLikelihood,
                    x.WebRating,
                    x.IssueResolution,
                    x.TransactionEase,
                    x.ProductRating
                };

                var validRatings = ratings.Where(r => r != 0).Select(r => r.Value);

                return new RecentSurveyViewModel
                {
                    Id = x.SurveyId,
                    BranchName = x.Branch,
                    CustomerName = x.CustomerName,
                    CustomerPhone = x.CustomerPhone,
                    TransactionId = x.TransactionId,
                    Channel = x.SurveyType,
                    Feedback = x.Feedback,
                    Email = x.Email,
                    SubmittedAt = x.SubmittedAt,
                    AverageRating = Math.Round(validRatings.Any() ? validRatings.Sum() / validRatings.Count() : 0, 2)
                };
            }).ToList();


            vm.DailyTrend = surveys.GroupBy(x => x.SubmittedAt.Date).OrderBy(x => x.Key).Select(g => new DailyTrendViewModel
            {
                Date = g.Key.ToString("dd MMM"),
                Responses = g.Count(),
                CSAT = Math.Round((decimal)g.Count(x => x.ServiceSatisfaction >= 4) / g.Count() * 100, 2)
            }).ToList();

            return vm;
        }

      
        public async Task<IActionResult> ExportBranchPerformance(DateTime? fromDate, DateTime? toDate)
        {
            //var surveys = _context.SurveyResponses.ToList();

            var surveys = _context.SurveyResponses.AsQueryable();
            if (fromDate.HasValue)
            {
                surveys = surveys.Where(x => x.SubmittedAt >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                surveys = surveys.Where(x => x.SubmittedAt < toDate.Value.Date.AddDays(1));
            }

            var branchSurveys = await surveys.OrderByDescending(x => x.SubmittedAt).ToListAsync();


            var branchPerformance = branchSurveys.GroupBy(x => string.IsNullOrWhiteSpace(x.Branch) ? "Other Channels" : x.Branch).Select(g => new BranchPerformanceViewModel
            {
                BranchName = g.Key,
                SurveyCount = g.Count(),

                AverageRating = Math.Round(g.Average(x =>
                    ((x.ServiceSatisfaction ?? 0) +
                     (x.StaffProfessionalism ?? 0) +
                     (x.BranchAmbience ?? 0) +
                     (x.RecommendationLikelihood ?? 0)) / 4m), 2),

                CSAT = Math.Round((decimal)g.Count(x => x.ServiceSatisfaction >= 4) / g.Count() * 100, 2),

                NPS = Math.Round((((decimal)g.Count(x => x.RecommendationLikelihood >= 9) / g.Count()) - ((decimal)g.Count(x => x.RecommendationLikelihood <= 6) / g.Count())) * 100, 2),

                LowRatings = g.Count(x => x.ServiceSatisfaction <= 2 || x.StaffProfessionalism <= 2 || x.BranchAmbience <= 2)
            }).OrderByDescending(x => x.CSAT).ToList();

            using var workbook = new XLWorkbook();

            var ws = workbook.Worksheets.Add("Branch Performance");

            //Headers
            ws.Cell(1, 1).Value = "Rank";
            ws.Cell(1, 2).Value = "Branch";
            ws.Cell(1, 3).Value = "Surveys";
            ws.Cell(1, 4).Value = "Average Rating";
            ws.Cell(1, 5).Value = "CSAT Score (%)";
            ws.Cell(1, 6).Value = "NPS Score";
            ws.Cell(1, 7).Value = "Low Ratings";

            var header = ws.Range("A1:G1");
            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.DarkBlue;
            header.Style.Font.FontColor = XLColor.White;

            int row = 2;
            int rank = 1;

            foreach (var item in branchPerformance)
            {
                ws.Cell(row, 1).Value = rank++;
                ws.Cell(row, 2).Value = item.BranchName;
                ws.Cell(row, 3).Value = item.SurveyCount;
                ws.Cell(row, 4).Value = item.AverageRating;
                ws.Cell(row, 5).Value = item.CSAT;
                ws.Cell(row, 6).Value = item.NPS;
                ws.Cell(row, 7).Value = item.LowRatings;

                row++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"BranchPerformance_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        }

        public async Task<IActionResult> ExportRecentSurveyResponses(DateTime? fromDate, DateTime? toDate)
        
        {
            //var RecentSurveyResponses = _context.SurveyResponses.OrderByDescending(x => x.SubmittedAt).ToList();

            var RecentSurveyResponses = _context.SurveyResponses.AsQueryable();

            if (fromDate.HasValue)
            {
                RecentSurveyResponses = RecentSurveyResponses.Where(x => x.SubmittedAt >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                RecentSurveyResponses = RecentSurveyResponses.Where(x => x.SubmittedAt < toDate.Value.Date.AddDays(1));
            }

            var surveys = await RecentSurveyResponses.OrderByDescending(x => x.SubmittedAt).ToListAsync();

            using var workbook = new XLWorkbook();

            var ws = workbook.Worksheets.Add("Survey Responses");

            //Headers
            ws.Cell(1, 1).Value = "CustomerName";
            ws.Cell(1, 2).Value = "CustomerPhone";
            ws.Cell(1, 3).Value = "TransactionId";
            ws.Cell(1, 4).Value = "Branch";
            ws.Cell(1, 5).Value = "Staff";
            ws.Cell(1, 6).Value = "ServiceSatisfaction";
            ws.Cell(1, 7).Value = "StaffProfessionalism";
            ws.Cell(1, 8).Value = "BranchAmbience";
            ws.Cell(1, 9).Value = "RecommendationLikelihood";
            ws.Cell(1, 10).Value = "WebRating";
            ws.Cell(1, 11).Value = "IssueResolution";
            ws.Cell(1, 12).Value = "TransactionEase";
            ws.Cell(1, 13).Value = "ProductRating";
            ws.Cell(1, 14).Value = "Feedback";
            ws.Cell(1, 15).Value = "SurveyType";
            ws.Cell(1, 16).Value = "SubmittedAt";
            ws.Cell(1, 17).Value = "Email";

            var header = ws.Range("A1:Q1");
            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.DarkBlue;
            header.Style.Font.FontColor = XLColor.White;

            int row = 2;

            foreach (var item in RecentSurveyResponses)
            {
                ws.Cell(row, 1).Value = item.CustomerName;
                ws.Cell(row, 2).Value = item.CustomerPhone;
                ws.Cell(row, 3).Value = item.TransactionId;
                ws.Cell(row, 4).Value = item.Branch;
                ws.Cell(row, 5).Value = item.Staff;
                ws.Cell(row, 6).Value = item.ServiceSatisfaction;
                ws.Cell(row, 7).Value = item.StaffProfessionalism;
                ws.Cell(row, 8).Value = item.BranchAmbience;
                ws.Cell(row, 9).Value = item.RecommendationLikelihood;
                ws.Cell(row, 10).Value = item.WebRating;
                ws.Cell(row, 11).Value = item.IssueResolution;
                ws.Cell(row, 12).Value = item.TransactionEase;
                ws.Cell(row, 13).Value = item.ProductRating;
                ws.Cell(row, 14).Value = item.Feedback;
                ws.Cell(row, 15).Value = item.SurveyType;
                ws.Cell(row, 16).Value = item.SubmittedAt;
                ws.Cell(row, 17).Value = item.Email;

                row++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"SurveyResponses_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        }

        private bool SetSessionData()
        {
            try
            {
                var user = HttpContext.Session.GetString("user");
                if (user == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }
    }
}