using GLMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using GLMS.Web.Data;

namespace GLMS.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                TotalClients = InMemoryDb.Clients.Count,
                TotalContracts = InMemoryDb.Contracts.Count,
                ActiveContracts = InMemoryDb.Contracts.Count(c => c.Status == "Active"),
                ExpiredContracts = InMemoryDb.Contracts.Count(c => c.Status == "Expired"),
                TotalServiceRequests = InMemoryDb.ServiceRequests.Count,
                PendingRequests = InMemoryDb.ServiceRequests.Count(s => s.Status == "Pending"),
                TotalRevenueUSD = InMemoryDb.ServiceRequests.Sum(s => s.CostUSD),
                TotalRevenueZAR = InMemoryDb.ServiceRequests.Sum(s => s.CostZAR)
            };

            return View(model);
        }
    }
}