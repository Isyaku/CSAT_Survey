using System.ComponentModel.DataAnnotations;

namespace Jaiz_CSAT_Survey.Models
{
    public class SurveyAlert
    {
        [Key]
        public int Id { get; set; }

        public long SurveyId { get; set; }

        public string AlertType { get; set; }
        public string? Branch { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
