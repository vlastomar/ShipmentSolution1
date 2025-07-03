using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.RouteViewModels;

namespace ShipmentSolution.Web.Controllers
{
    public class RouteController : Controller
    {
        private readonly IRouteService routeService;

        public RouteController(IRouteService routeService)
        {
            this.routeService = routeService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? searchTerm, string? priorityFilter, int page = 1)
        {
            const int PageSize = 5;
            var model = await routeService.GetPaginatedAsync(page, PageSize, searchTerm, priorityFilter);

            ViewBag.CurrentSearch = searchTerm;
            ViewBag.CurrentPriority = priorityFilter;
            ViewBag.PriorityOptions = new List<string> { "High", "Medium", "Low" };

            return View(model);
        }

        [Authorize(Roles = "Administrator,RegisteredUser")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new RouteCreateViewModel
            {
                MailCarriers = await routeService.GetCarrierListAsync()
            };
            return View(model);
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

            await routeService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator,RegisteredUser")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await routeService.GetForEditAsync(id);
            model.MailCarriers = await routeService.GetCarrierListAsync();
            return View(model);
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

            await routeService.EditAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await routeService.GetDeleteViewModelAsync(id);
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await routeService.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
