using Microsoft.AspNetCore.Mvc;
using GLMS.Web.Models;
using GLMS.Web.Services;
using GLMS.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLMS.Web.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly CurrencyService _currencyService;
        private readonly ContractService _contractService;

        public ServiceRequestController(
            CurrencyService currencyService,
            ContractService contractService)
        {
            _currencyService = currencyService;
            _contractService = contractService;
        }

        public IActionResult Index()
        {
            return View(InMemoryDb.ServiceRequests);
        }

        public IActionResult Details(int id)
        {
            var request = InMemoryDb.ServiceRequests
                .FirstOrDefault(s => s.Id == id);

            if (request == null)
                return NotFound();

            return View(request);
        }

        public IActionResult Create()
        {
            LoadCreateViewData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest request)
        {
            if (!ModelState.IsValid)
            {
                LoadCreateViewData();
                return View(request);
            }

            var contract = InMemoryDb.Contracts
                .FirstOrDefault(c => c.Id == request.ContractId);

            if (contract == null)
            {
                ModelState.AddModelError(string.Empty, "Contract not found.");
                LoadCreateViewData();
                return View(request);
            }

            if (!_contractService.IsContractValid(contract.Status))
            {
                ModelState.AddModelError(string.Empty,
                    "Cannot create service request. Contract is Expired or On Hold.");

                LoadCreateViewData();
                return View(request);
            }

            var rate = await _currencyService.GetUsdToZarRateAsync();

            request.CostZAR =
                _currencyService.ConvertUsdToZar(request.CostUSD, rate);

            request.Status ??= "Pending";

            request.Id = InMemoryDb.ServiceRequests.Count + 1;

            InMemoryDb.ServiceRequests.Add(request);

            TempData["Success"] = "Service Request created successfully.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var request = InMemoryDb.ServiceRequests
                .FirstOrDefault(s => s.Id == id);

            if (request == null)
                return NotFound();

            return View(request);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var request = InMemoryDb.ServiceRequests
                .FirstOrDefault(x => x.Id == id);

            if (request != null)
            {
                InMemoryDb.ServiceRequests.Remove(request);
            }

            return RedirectToAction(nameof(Index));
        }

        private void LoadCreateViewData()
        {
            var contracts = InMemoryDb.Contracts
                .Where(c => c != null && c.Client != null)
                .Select(c => new
                {
                    c.Id,
                    Display = c.Client!.Name + " (" + c.Status + ")"
                })
                .ToList();

            ViewBag.Contracts = new SelectList(contracts, "Id", "Display");
        }
    }
}