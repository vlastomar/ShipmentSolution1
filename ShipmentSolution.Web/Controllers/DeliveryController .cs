using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.DeliveryViewModels;

namespace ShipmentSolution.Web.Controllers
{
    public class DeliveryController : Controller
    {
        private readonly IDeliveryService deliveryService;
        private readonly UserManager<IdentityUser> userManager;

        public DeliveryController(IDeliveryService deliveryService, UserManager<IdentityUser> userManager)
        {
            this.deliveryService = deliveryService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, string? mailCarrierFilter, int page = 1)
        {
            try
            {
                const int PageSize = 5;

                var userId = userManager.GetUserId(User);
                var isAdmin = User.IsInRole("Administrator");
                var isLoggedIn = User.Identity!.IsAuthenticated;

                var model = await deliveryService.GetPaginatedAsync(
                    page,
                    PageSize,
                    searchTerm,
                    mailCarrierFilter,
                    userId ?? string.Empty,
                    isAdmin,
                    isLoggedIn);

                ViewBag.CurrentSearch = searchTerm;
                ViewBag.CurrentCarrier = mailCarrierFilter;
                ViewBag.MailCarrierOptions = await deliveryService.GetCarrierNamesAsync();
                ViewBag.IsLoggedIn = isLoggedIn;
                ViewBag.IsAdmin = isAdmin;

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
                var userId = userManager.GetUserId(User);
                var model = new DeliveryCreateViewModel
                {
                    MailCarriers = await deliveryService.GetCarrierListAsync(userId, User),
                    Routes = await deliveryService.GetRouteListAsync(userId, User),
                    Shipments = await deliveryService.GetShipmentListAsync(userId, User) // ✅ Add this line
                };
                return View(model);
            }
            catch (Exception ex)
            {
                //logger.LogError(ex, "Error loading delivery creation form.");
                return RedirectToAction("Error500", "Home");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,RegisteredUser")]
        public async Task<IActionResult> Create(DeliveryCreateViewModel model)
        {
            var userId = userManager.GetUserId(User);

            if (!ModelState.IsValid)
            {
                var freshModel = await deliveryService.GetCreateModelAsync(userId, User);
                model.Shipments = freshModel.Shipments;
                model.MailCarriers = freshModel.MailCarriers;
                model.Routes = freshModel.Routes;
                return View(model);
            }

            try
            {
                await deliveryService.CreateAsync(model, userId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var freshModel = await deliveryService.GetCreateModelAsync(userId, User);
                model.Shipments = freshModel.Shipments;
                model.MailCarriers = freshModel.MailCarriers;
                model.Routes = freshModel.Routes;

                ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,RegisteredUser")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var userId = userManager.GetUserId(User);
                var model = await deliveryService.GetForEditAsync(id, userId, User);

                if (model == null)
                    return Unauthorized();

                return View(model);
            }
            catch
            {
                TempData["Error"] = "Failed to load edit form.";
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

            var userId = userManager.GetUserId(User);

            try
            {
                var success = await deliveryService.EditAsync(model, userId, User);

                if (!success)
                {
                    ModelState.AddModelError("", "You are not authorized or the update failed.");
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
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
                var userId = userManager.GetUserId(User);
                var model = await deliveryService.GetDeleteViewModelAsync(id, userId, User);

                if (model == null)
                    return NotFound();

                return View(model);
            }
            catch
            {
                TempData["Error"] = "Failed to load delete confirmation.";
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
                var userId = userManager.GetUserId(User);
                var success = await deliveryService.SoftDeleteAsync(id, userId, User);

                if (!success)
                {
                    TempData["Error"] = "Failed to delete delivery.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Failed to delete delivery.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
