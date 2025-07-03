using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.CustomerViewModels;

namespace ShipmentSolution.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        public async Task<IActionResult> Index(string? searchTerm, int page = 1)
        {
            const int PageSize = 5;
            var model = await customerService.GetPaginatedAsync(page, PageSize, searchTerm);
            ViewBag.CurrentSearch = searchTerm;
            return View(model);
        }

        [Authorize(Roles = "RegisteredUser,Administrator")]
        [HttpGet]
        public IActionResult Create() => View();

        [Authorize(Roles = "RegisteredUser,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await customerService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "RegisteredUser,Administrator")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await customerService.GetForEditAsync(id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        [Authorize(Roles = "RegisteredUser,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await customerService.EditAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await customerService.GetByIdAsync(id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await customerService.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
