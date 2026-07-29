using System.ComponentModel.DataAnnotations;

namespace Jaiz_CSAT_Survey.ViewModels
{
    public class SurveyViewModel
    {
        [Key]
        public int SurveyId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? TransactionId { get; set; }
        public string? SurveyType { get; set; }
        public string? Branch { get; set; }
        public string? Staff { get; set; }
        public string? NPSRating { get; set; }
        public string? Email { get; set; }

        [Required]
        [Range(1, 5)]
        public byte ServiceSatisfaction { get; set; }

        [Required]
        [Range(0, 10)]
        public byte RecommendationLikelihood { get; set; }

        [StringLength(1000)]
        public string? Comments { get; set; }
        public BranchQRViewModel? BranchQRViewModel { get; set; }
        public EmailAlertViewModel? EmailAlertViewModel { get; set; }
        public OtherSurveyViewModel? OtherSurveyViewModel { get; set; }
    }
    public class BranchQRViewModel
    {     
        [Required]
        [Range(1, 5)]
        public byte StaffProfessionalism { get; set; }
        [Required]
        [Range(1, 5)]
        public byte BranchAmbience { get; set; }
             
    }
    public class EmailAlertViewModel
    {
        [Required]
        [Range(1, 5)]
        public byte TransactionEase { get; set; }

        [Required]
        [Range(1, 5)]
        public byte ProductRating { get; set; }     
       
    }
    public class OtherSurveyViewModel
    {  
        [Required]
        [Range(1, 5)]
        public byte IssueResolution { get; set; }        

        [Required]
        [Range(1, 5)]
        public byte WebRating { get; set; }
    }
}
