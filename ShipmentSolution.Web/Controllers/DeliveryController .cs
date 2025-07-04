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
            try
            {
                const int PageSize = 5;

                var model = await deliveryService.GetPaginatedAsync(page, PageSize, searchTerm, mailCarrierFilter);
                ViewBag.CurrentSearch = searchTerm;
                ViewBag.CurrentCarrier = mailCarrierFilter;
                ViewBag.MailCarrierOptions = await deliveryService.GetCarrierNamesAsync();

                return View(model);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Failed to load deliveries.");
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,RegisteredUser")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = await deliveryService.GetCreateModelAsync();
                return View(model);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Failed to load form data.");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,RegisteredUser")]
        public async Task<IActionResult> Create(DeliveryCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await deliveryService.CreateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Failed to create delivery.");
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,RegisteredUser")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await deliveryService.GetForEditAsync(id);
                if (model == null)
                    return NotFound();

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,RegisteredUser")]
        public async Task<IActionResult> Edit(DeliveryEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await deliveryService.EditAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Failed to update delivery.");
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = await deliveryService.GetDeleteViewModelAsync(id);
                if (model == null)
                    return NotFound();

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await deliveryService.SoftDeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Failed to delete delivery.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
