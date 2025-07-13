using Microsoft.AspNetCore.Mvc;
using ShipmentSolution.Web.ViewModels.ShipmentViewModels;
using ShipmentSolution.Services.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ShipmentSolution.Web.Controllers
{
    public class ShipmentController : Controller
    {
        private readonly IShipmentService shipmentService;
        private readonly ILogger<ShipmentController> logger;

        public ShipmentController(IShipmentService shipmentService, ILogger<ShipmentController> logger)
        {
            this.shipmentService = shipmentService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, string? shippingMethod, int page = 1)
        {
            const int PageSize = 5;

            try
            {
                var model = await shipmentService.GetPaginatedAsync(page, PageSize, searchTerm, shippingMethod);

                ViewBag.CurrentSearch = searchTerm;
                ViewBag.CurrentShippingMethod = shippingMethod;
                ViewBag.ShippingMethods = new List<string> { "Express", "Ground" };

                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading shipments.");
                return RedirectToAction("Error500", "Home");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = await shipmentService.PrepareCreateViewModelAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error preparing create shipment form.");
                return RedirectToAction("Error500", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(ShipmentCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await shipmentService.PrepareCreateViewModelAsync();
                return View(model);
            }

            try
            {
                await shipmentService.CreateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating shipment.");
                ModelState.AddModelError("", "An error occurred while creating the shipment.");
                model = await shipmentService.PrepareCreateViewModelAsync();
                return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await shipmentService.GetForEditAsync(id);
                if (model == null)
                    return NotFound();

                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading shipment for editing.");
                return RedirectToAction("Error500", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(ShipmentEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Customers = await shipmentService.GetCustomerListAsync();
                return View(model);
            }

            try
            {
                await shipmentService.EditAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error editing shipment.");
                ModelState.AddModelError("", "An error occurred while editing the shipment.");
                model.Customers = await shipmentService.GetCustomerListAsync();
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = await shipmentService.GetForDeleteAsync(id);
                if (model == null)
                    return NotFound();

                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading shipment for deletion.");
                return RedirectToAction("Error500", "Home");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await shipmentService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting shipment.");
                return RedirectToAction("Error500", "Home");
            }
        }
    }
}
