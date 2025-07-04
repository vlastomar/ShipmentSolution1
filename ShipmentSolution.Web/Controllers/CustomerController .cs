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
            try
            {
                const int PageSize = 5;
                var model = await customerService.GetPaginatedAsync(page, PageSize, searchTerm);
                ViewBag.CurrentSearch = searchTerm;
                return View(model);
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                ModelState.AddModelError("", "An error occurred while loading the customers.");
                return View();
            }
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

            try
            {
                await customerService.CreateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Failed to create customer. Please try again.");
                return View(model);
            }
        }

        [Authorize(Roles = "RegisteredUser,Administrator")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await customerService.GetForEditAsync(id);
                if (model == null)
                    return NotFound();

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "RegisteredUser,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await customerService.EditAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Failed to update customer. Please try again.");
                return View(model);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = await customerService.GetByIdAsync(id);
                if (model == null)
                    return NotFound();

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await customerService.SoftDeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                // log the error
                TempData["Error"] = "Failed to delete customer.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
