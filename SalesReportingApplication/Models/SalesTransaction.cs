using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Transactions;

namespace SalesReportingApplication.Models
{
    public class SalesTransaction
    {
        [Key]
        [JsonPropertyName("transactionID")]
        public int TransactionID { get; set; }
        [JsonPropertyName("salesItem")]
        public string SalesItem { get; set; }
        [JsonPropertyName("salesDate")]
        public DateTime SalesDate { get; set; }
        [JsonPropertyName("transactionUserID")]
        public int TransactionUserID { get; set; }
        [JsonPropertyName("userProfile")]
        [ForeignKey("TransactionUserID")]
        [ValidateNever]
        public UserProfile UserProfile { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("updatedDate")]
        public DateTime UpdatedDate { get; set; }

  
    }
}
