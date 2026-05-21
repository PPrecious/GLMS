namespace GLMS.Web.Models
{
    public class DashboardViewModel
    {
        public int TotalClients { get; set; }
        public int TotalContracts { get; set; }
        public int ActiveContracts { get; set; }
        public int ExpiredContracts { get; set; }
        public int TotalServiceRequests { get; set; }
        public int PendingRequests { get; set; }
        public decimal TotalRevenueUSD { get; set; }
        public decimal TotalRevenueZAR { get; set; }
    }
}