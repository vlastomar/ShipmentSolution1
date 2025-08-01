using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.RouteViewModels;

namespace ShipmentSolution.Web.Controllers
{
    public class RouteController : Controller
    {
        private readonly IRouteService routeService;
        private readonly ILogger<RouteController> logger;
        private readonly UserManager<IdentityUser> userManager;

        public RouteController(
            IRouteService routeService,
            ILogger<RouteController> logger,
            UserManager<IdentityUser> userManager)
        {
            this.routeService = routeService;
            this.logger = logger;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? searchTerm, string? priorityFilter, int page = 1)
        {
            const int PageSize = 5;

            try
            {
                var isLoggedIn = User.Identity?.IsAuthenticated ?? false;
                var isAdmin = User.IsInRole("Administrator");
                var userId = userManager.GetUserId(User);

                if (!isLoggedIn)
                {
                    ViewBag.ShowWarning = true;
                    ViewBag.CurrentSearch = searchTerm;
                    ViewBag.CurrentPriority = priorityFilter;
                    ViewBag.PriorityOptions = new List<string> { "High", "Medium", "Low" };
                    return View(new Web.ViewModels.Common.PaginatedList<RouteViewModel>()); // empty list
                }

                var model = await routeService.GetPaginatedAsync(
                    page, PageSize, searchTerm, priorityFilter, userId, isAdmin);

                ViewBag.CurrentSearch = searchTerm;
                ViewBag.CurrentPriority = priorityFilter;
                ViewBag.PriorityOptions = new List<string> { "High", "Medium", "Low" };
                ViewBag.IsAdmin = isAdmin;
                ViewBag.IsLoggedIn = isLoggedIn;

                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading route list.");
                return RedirectToAction("Error500", "Home");
            }
        }

        [Authorize(Roles = "Administrator,RegisteredUser")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var userId = userManager.GetUserId(User);

                var model = new RouteCreateViewModel
                {
                    
                    MailCarriers = await routeService.GetMailCarriersAsync(userId, User)
                };

                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading create route view.");
                return RedirectToAction("Error500", "Home");
            }
        }

        [Authorize(Roles = "Administrator,RegisteredUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RouteCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.MailCarriers = await routeService.GetCarrierListAsync();
                return View(model);
            }

            try
            {
                var userId = userManager.GetUserId(User)!;
                await routeService.CreateAsync(model, userId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating route.");
                ModelState.AddModelError("", "An error occurred while creating the route.");
                model.MailCarriers = await routeService.GetCarrierListAsync();
                return View(model);
            }
        }

        [Authorize(Roles = "Administrator,RegisteredUser")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var route = await routeService.GetByIdAsync(id);
                if (route == null) return NotFound();

                var userId = userManager.GetUserId(User);
                var isAdmin = User.IsInRole("Administrator");

                if (!isAdmin && route.CreatedByUserId != userId)
                {
                    return Forbid(); // Only owner or admin can edit
                }

                var model = await routeService.GetForEditAsync(id);
                model.MailCarriers = await routeService.GetCarrierListAsync();

                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading route for edit.");
                return RedirectToAction("Error500", "Home");
            }
        }

        [Authorize(Roles = "Administrator,RegisteredUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RouteEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.MailCarriers = await routeService.GetCarrierListAsync();
                return View(model);
            }

            try
            {
                var route = await routeService.GetByIdAsync(model.RouteId);
                var userId = userManager.GetUserId(User);
                var isAdmin = User.IsInRole("Administrator");

                if (route == null)
                    return NotFound();

                if (!isAdmin && route.CreatedByUserId != userId)
                    return Forbid();

                await routeService.EditAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error editing route.");
                ModelState.AddModelError("", "An error occurred while updating the route.");
                model.MailCarriers = await routeService.GetCarrierListAsync();
                return View(model);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = await routeService.GetDeleteViewModelAsync(id);
                if (model == null) return NotFound();

                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading route for deletion.");
                return RedirectToAction("Error500", "Home");
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await routeService.SoftDeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting route.");
                return RedirectToAction("Error500", "Home");
            }
        }
    }
}
