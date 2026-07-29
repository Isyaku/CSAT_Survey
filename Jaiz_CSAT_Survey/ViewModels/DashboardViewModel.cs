namespace Jaiz_CSAT_Survey.ViewModels
{
    public class DashboardViewModel
    {

        public int TotalResponses { get; set; }

        public int LowRatingCount { get; set; }

        // NEW
        public decimal NPSScore { get; set; }

        public int Promoters { get; set; }

        public int Passives { get; set; }

        public int Detractors { get; set; }

        //BranchQR
        public int BranchTotalResponses { get; set; }

        public decimal BranchCSATScore { get; set; }
        public decimal BranchNPSScore { get; set; }

        public decimal BranchCSATAverageRating { get; set; }
        public decimal BranchNPSAverageRating { get; set; }

        public int BranchLowRatingCount { get; set; }

        //Email Survey
        public int EmailTotalResponses { get; set; }

        public decimal EmailCSATScore { get; set; }
        public decimal EmailNPSScore { get; set; }

        public decimal EmailCSATAverageRating { get; set; }
        public decimal EmailNPSAverageRating { get; set; }

        public int EmailLowRatingCount { get; set; }

        //Other
        public int OtherTotalResponses { get; set; }

        public decimal OtherCSATScore { get; set; }
        public decimal OtherNPSScore { get; set; }

        public decimal OtherCSATAverageRating { get; set; }
        public decimal OtherNPSAverageRating { get; set; }

        public int OtherLowRatingCount { get; set; }

        public List<ChannelSummaryViewModel> Channels { get; set; } = new();

        public List<BranchPerformanceViewModel> BranchPerformance { get; set; } = new();

        public List<RecentSurveyViewModel> RecentResponses { get; set; } = new();

        public List<DailyTrendViewModel> DailyTrend { get; set; } = new();
    }

    public class ChannelSummaryViewModel
    {
        public string Channel { get; set; } = "";

        public int TotalResponses { get; set; }

        public decimal AverageRating { get; set; }

        public decimal CSAT { get; set; }
        public int LowRatings { get; set; }
    }

    public class BranchPerformanceViewModel
    {
        public string BranchName { get; set; } = "";

        public int SurveyCount { get; set; }

        public decimal AverageRating { get; set; }

        public decimal CSAT { get; set; }
        public decimal NPS { get; set; }

        public int LowRatings { get; set; }
    }

    public class RecentSurveyViewModel
    {
        public long Id { get; set; }

        public string BranchName { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string CustomerPhone { get; set; } = "";
        public string TransactionId { get; set; } = "";
        public string Channel { get; set; } = "";

        public decimal AverageRating { get; set; }

        public string? Feedback { get; set; }
        public string? Email { get; set; }

        public DateTime SubmittedAt { get; set; }
    }

    public class DailyTrendViewModel
    {
        public string Date { get; set; } = "";

        public decimal CSAT { get; set; }

        public int Responses { get; set; }
    }
}