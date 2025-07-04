using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.RouteViewModels;

namespace ShipmentSolution.Web.Controllers
{
    public class RouteController : Controller
    {
        private readonly IRouteService routeService;
        private readonly ILogger<RouteController> logger;

        public RouteController(IRouteService routeService, ILogger<RouteController> logger)
        {
            this.routeService = routeService;
            this.logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? searchTerm, string? priorityFilter, int page = 1)
        {
            const int PageSize = 5;

            try
            {
                var model = await routeService.GetPaginatedAsync(page, PageSize, searchTerm, priorityFilter);

                ViewBag.CurrentSearch = searchTerm;
                ViewBag.CurrentPriority = priorityFilter;
                ViewBag.PriorityOptions = new List<string> { "High", "Medium", "Low" };

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
                var model = new RouteCreateViewModel
                {
                    MailCarriers = await routeService.GetCarrierListAsync()
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
                await routeService.CreateAsync(model);
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
                var model = await routeService.GetForEditAsync(id);
                if (model == null) return NotFound();

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
