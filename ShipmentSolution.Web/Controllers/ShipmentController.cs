using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.ShipmentViewModels;
using System.Security.Claims;

namespace ShipmentSolution.Web.Controllers
{
    public class ShipmentController : Controller
    {
        private readonly IShipmentService shipmentService;
        private readonly ILogger<ShipmentController> logger;
        private readonly UserManager<IdentityUser> userManager;

        public ShipmentController(
            IShipmentService shipmentService,
            ILogger<ShipmentController> logger,
            UserManager<IdentityUser> userManager)
        {
            this.shipmentService = shipmentService;
            this.logger = logger;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, string? shippingMethod, int page = 1)
        {
            const int PageSize = 5;

            try
            {
                string? userId = User.Identity?.IsAuthenticated == true
                    ? userManager.GetUserId(User)
                    : null;

                bool isAdmin = User.IsInRole("Administrator");
                bool isLoggedIn = User.Identity?.IsAuthenticated == true;

                var model = await shipmentService.GetPaginatedAsync(page, PageSize, searchTerm, shippingMethod, userId, isAdmin);

                ViewBag.CurrentSearch = searchTerm;
                ViewBag.CurrentShippingMethod = shippingMethod;
                ViewBag.ShippingMethods = new List<string> { "Express", "Ground" };
                ViewBag.IsAdmin = isAdmin;
                ViewBag.IsLoggedIn = isLoggedIn;

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
                string userId = userManager.GetUserId(User);
                var model = await shipmentService.PrepareCreateViewModelAsync(userId, User);
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
            string userId = userManager.GetUserId(User);

            if (!ModelState.IsValid)
            {
                model = await shipmentService.PrepareCreateViewModelAsync(userId, User);
                return View(model);
            }

            try
            {
                await shipmentService.CreateAsync(model, userId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating shipment.");
                ModelState.AddModelError("", "An error occurred while creating the shipment.");
                model = await shipmentService.PrepareCreateViewModelAsync(userId, User);
                return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                string userId = userManager.GetUserId(User);
                var model = await shipmentService.GetForEditAsync(id, userId, User);
                if (model == null)
                    return Unauthorized();

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
            string userId = userManager.GetUserId(User);

            if (!ModelState.IsValid)
            {
                model.Customers = await shipmentService.GetCustomerListAsync(userId, User);
                return View(model);
            }

            try
            {
                bool success = await shipmentService.EditAsync(model, userId, User);
                if (!success)
                    return Unauthorized();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error editing shipment.");
                ModelState.AddModelError("", "An error occurred while editing the shipment.");
                model.Customers = await shipmentService.GetCustomerListAsync(userId, User);
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
                var userId = userManager.GetUserId(User);
                var success = await shipmentService.DeleteAsync(id, userId, User);

                if (!success)
                    return Forbid();

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
