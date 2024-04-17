namespace SalesReportingApplication.Models
{
    public class SalesSummaryYearly
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public decimal TotalSales { get; set; }
    }
}
