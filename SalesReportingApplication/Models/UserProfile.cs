using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SalesReportingApplication.Models
{
    public class UserProfile
    {
        [Key]
        [JsonPropertyName("userID")]
        public int UserID { get; set; }
        [Required]
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        [Required]
        [JsonPropertyName("loginID")]
        public string LoginID { get; set; }
        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [Required]
        [JsonPropertyName("userRole")]
        public string UserRole { get; set; }
        [Required]
        [JsonPropertyName("reportingManager")]
        public  int ReportingManager  { get; set; }

    }
}
