using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.CustomerViewModels;
using System.Security.Claims;

namespace ShipmentSolution.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService customerService;
        private readonly UserManager<IdentityUser> userManager;

        public CustomerController(ICustomerService customerService, UserManager<IdentityUser> userManager)
        {
            this.customerService = customerService;
            this.userManager = userManager;
        }

        // Public listing based on authentication logic inside the service
        public async Task<IActionResult> Index(string? searchTerm, int page = 1)
        {
            const int PageSize = 5;
            var userId = userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Administrator");
            var isLoggedIn = User.Identity?.IsAuthenticated ?? false;

            if (userId == null && isLoggedIn)
            {
                return Unauthorized(); // should never happen, but for safety
            }

            var model = await customerService.GetPaginatedAsync(
                page, PageSize, searchTerm, userId ?? string.Empty, isAdmin, isLoggedIn);

            ViewBag.CurrentSearch = searchTerm;
            ViewBag.IsLoggedIn = isLoggedIn;

            return View(model);
        }

        [Authorize(Roles = "RegisteredUser,Administrator")]
        public IActionResult Create()
        {
            var model = new CustomerCreateViewModel
            {
                ShippingMethodOptions = GetShippingMethods()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "RegisteredUser,Administrator")]
        public async Task<IActionResult> Create(CustomerCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ShippingMethodOptions = GetShippingMethods();
                return View(model);
            }

            var userId = userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            await customerService.CreateAsync(model, userId);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "RegisteredUser,Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            var model = await customerService.GetForEditAsync(id, userId, User);
            if (model == null)
            {
                return Unauthorized(); // or NotFound()
            }

            model.ShippingMethodOptions = GetShippingMethods();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "RegisteredUser,Administrator")]
        public async Task<IActionResult> Edit(CustomerEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ShippingMethodOptions = GetShippingMethods();
                return View(model);
            }

            var userId = userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            var success = await customerService.EditAsync(model, userId, User);
            if (!success)
            {
                return Unauthorized();
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await customerService.GetByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(new CustomerDeleteViewModel
            {
                CustomerId = model.CustomerId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await customerService.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private List<SelectListItem> GetShippingMethods() => new List<SelectListItem>
        {
            new SelectListItem { Text = "Standard", Value = "Standard" },
            new SelectListItem { Text = "Express", Value = "Express" },
            new SelectListItem { Text = "Overnight", Value = "Overnight" },
        };
    }
}
