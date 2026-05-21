using GLMS.Web.Data;
using GLMS.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.Web.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View(InMemoryDb.Clients.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Regions = Enum.GetValues(typeof(Region));
            return View();
        }

        [HttpPost]
        public IActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                client.Id = InMemoryDb.Clients.Count > 0
                    ? InMemoryDb.Clients.Max(c => c.Id) + 1
                    : 1;

                InMemoryDb.Clients.Add(client);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Regions = Enum.GetValues(typeof(Region));
            return View(client);
        }

        public IActionResult Delete(int id)
        {
            var client = InMemoryDb.Clients.FirstOrDefault(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var client = InMemoryDb.Clients.FirstOrDefault(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            InMemoryDb.Clients.Remove(client);

            return RedirectToAction(nameof(Index));
        }
    }
}