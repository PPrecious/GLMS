using GLMS.Web.Data;
using GLMS.Web.Models;
using GLMS.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLMS.Web.Controllers
{
    public class ContractController : Controller
    {
        private readonly FileService _fileService;

        public ContractController(FileService fileService)
        {
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            var contracts = InMemoryDb.Contracts;

            foreach (var c in contracts)
            {
                c.Client = InMemoryDb.Clients.FirstOrDefault(x => x.Id == c.ClientId);
            }

            return View(contracts);
        }

        public IActionResult Create()
        {
            ViewBag.Clients = new SelectList(InMemoryDb.Clients, "Id", "Name");
            ViewBag.ServiceLevels = Enum.GetValues(typeof(ServiceLevel));

            return View();
        }

        [HttpPost]
        public IActionResult Create(Contract contract, IFormFile file)
        {
            if (file != null)
            {
                contract.SignedAgreementPath = _fileService.SavePdfAsync(file).Result;
            }

            contract.Id = InMemoryDb.Contracts.Count > 0
                ? InMemoryDb.Contracts.Max(x => x.Id) + 1
                : 1;

            InMemoryDb.Contracts.Add(contract);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Search(DateTime? startDate, DateTime? endDate, string status)
        {
            var query = InMemoryDb.Contracts.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(c => c.StartDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(c => c.EndDate <= endDate.Value);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(c => c.Status == status);

            var result = query.ToList();

            foreach (var c in result)
            {
                c.Client = InMemoryDb.Clients.FirstOrDefault(x => x.Id == c.ClientId);
            }

            return View("Index", result);
        }

        public IActionResult DownloadFile(int id)
        {
            var contract = InMemoryDb.Contracts.FirstOrDefault(x => x.Id == id);

            if (contract == null)
                return NotFound();

            if (string.IsNullOrEmpty(contract.SignedAgreementPath))
                return NotFound("No file attached to this contract.");

            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/uploads/contracts",
                contract.SignedAgreementPath
            );

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found on server.");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/pdf", contract.SignedAgreementPath);
        }

        public IActionResult Details(int id)
        {
            var contract = InMemoryDb.Contracts.FirstOrDefault(x => x.Id == id);

            if (contract == null)
                return NotFound();

            contract.Client = InMemoryDb.Clients.FirstOrDefault(x => x.Id == contract.ClientId);

            return View(contract);
        }

        public IActionResult Delete(int id)
        {
            var contract = InMemoryDb.Contracts.FirstOrDefault(x => x.Id == id);

            if (contract == null)
                return NotFound();

            contract.Client = InMemoryDb.Clients.FirstOrDefault(x => x.Id == contract.ClientId);

            return View(contract);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var contract = InMemoryDb.Contracts.FirstOrDefault(x => x.Id == id);

            if (contract == null)
                return NotFound();

            if (!string.IsNullOrEmpty(contract.SignedAgreementPath))
            {
                var filePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/uploads/contracts",
                    contract.SignedAgreementPath
                );

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            InMemoryDb.Contracts.Remove(contract);

            return RedirectToAction(nameof(Index));
        }
    }
}