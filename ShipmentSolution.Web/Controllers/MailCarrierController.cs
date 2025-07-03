using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.MailCarrierViewModels;

namespace ShipmentSolution.Web.Controllers
{
    public class MailCarrierController : Controller
    {
        private readonly IMailCarrierService mailCarrierService;

        public MailCarrierController(IMailCarrierService mailCarrierService)
        {
            this.mailCarrierService = mailCarrierService;
        }

        public async Task<IActionResult> Index(string? searchTerm, string? statusFilter, int page = 1)
        {
            const int PageSize = 5;

            var result = await mailCarrierService.GetPaginatedAsync(page, PageSize, searchTerm, statusFilter);

            ViewBag.CurrentSearch = searchTerm;
            ViewBag.CurrentStatus = statusFilter;

            var statusOptions = (await mailCarrierService.GetAllAsync())
                .Select(c => c.Status)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct()
                .ToList();

            ViewBag.StatusOptions = statusOptions;

            return View(result);
        }

        [Authorize(Roles = "RegisteredUser,Administrator")]
        [HttpGet]
        public IActionResult Create() => View();

        [Authorize(Roles = "RegisteredUser,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MailCarrierCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await mailCarrierService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "RegisteredUser,Administrator")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await mailCarrierService.GetForEditAsync(id);
            return View(model);
        }

        [Authorize(Roles = "RegisteredUser,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MailCarrierEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await mailCarrierService.EditAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await mailCarrierService.GetByIdAsync(id);
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await mailCarrierService.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
