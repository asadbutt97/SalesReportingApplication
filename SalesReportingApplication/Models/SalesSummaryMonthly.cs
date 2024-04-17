namespace SalesReportingApplication.Models
{
    public class SalesSummaryMonthly
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public decimal TotalSales { get; set; }
    }
}
