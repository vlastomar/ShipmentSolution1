using Microsoft.AspNetCore.Mvc;
using ShipmentSolution.Web.ViewModels.ShipmentViewModels;
using ShipmentSolution.Services.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ShipmentSolution.Web.Controllers
{
    public class ShipmentController : Controller
    {
        private readonly IShipmentService shipmentService;

        public ShipmentController(IShipmentService shipmentService)
        {
            this.shipmentService = shipmentService;
        }

        public async Task<IActionResult> Index(string? searchTerm, string? shippingMethod, int page = 1)
        {
            const int PageSize = 5;

            var model = await shipmentService.GetPaginatedAsync(page, PageSize, searchTerm, shippingMethod);

            ViewBag.CurrentSearch = searchTerm;
            ViewBag.CurrentShippingMethod = shippingMethod;
            ViewBag.ShippingMethods = new List<string> { "Standard", "Express", "Next-Day" };

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await shipmentService.PrepareCreateViewModelAsync();
            return View(model);
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

            await shipmentService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await shipmentService.GetForEditAsync(id);
            return View(model);
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

            await shipmentService.EditAsync(model);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await shipmentService.GetForDeleteAsync(id);
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await shipmentService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }



    }
}
