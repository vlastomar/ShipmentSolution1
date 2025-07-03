using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.DeliveryViewModels;

namespace ShipmentSolution.Web.Controllers
{
    public class DeliveryController : Controller
    {
        private readonly IDeliveryService deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            this.deliveryService = deliveryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, string? mailCarrierFilter, int page = 1)
        {
            const int PageSize = 5;

            var model = await deliveryService.GetPaginatedAsync(page, PageSize, searchTerm, mailCarrierFilter);

            ViewBag.CurrentSearch = searchTerm;
            ViewBag.CurrentCarrier = mailCarrierFilter;
            ViewBag.MailCarrierOptions = await deliveryService.GetCarrierNamesAsync();

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,RegisteredUser")]
        public async Task<IActionResult> Create()
        {
            var model = await deliveryService.GetCreateModelAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,RegisteredUser")]
        public async Task<IActionResult> Create(DeliveryCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await deliveryService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,RegisteredUser")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await deliveryService.GetForEditAsync(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,RegisteredUser")]
        public async Task<IActionResult> Edit(DeliveryEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await deliveryService.EditAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await deliveryService.GetByIdAsync(id);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await deliveryService.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
