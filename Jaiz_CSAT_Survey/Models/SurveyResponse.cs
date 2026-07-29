using System.ComponentModel.DataAnnotations;

namespace Jaiz_CSAT_Survey.Models
{
    public class SurveyResponse
    {
        [Key]
        public long SurveyId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? TransactionId { get; set; }
        public string? Branch { get; set; }
        public string? Staff { get; set; }

        public byte? ServiceSatisfaction { get; set; }

        public byte? StaffProfessionalism { get; set; }

        public byte? BranchAmbience { get; set; }

        public byte? RecommendationLikelihood { get; set; }
        public byte? WebRating { get; set; }
        public byte? IssueResolution { get; set; }
        public byte? TransactionEase { get; set; }
        public byte? ProductRating { get; set; }

        public string? Feedback { get; set; }
        public string? Email { get; set; }
        public string? SurveyType { get; set; }

        public DateTime SubmittedAt { get; set; }
    }
}
