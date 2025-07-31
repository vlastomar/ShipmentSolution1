using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.MailCarrierViewModels;
using System.Security.Claims;

namespace ShipmentSolution.Web.Controllers
{
    public class MailCarrierController : Controller
    {
        private readonly IMailCarrierService mailCarrierService;
        private readonly ILogger<MailCarrierController> logger;

        public MailCarrierController(IMailCarrierService mailCarrierService, ILogger<MailCarrierController> logger)
        {
            this.mailCarrierService = mailCarrierService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index(string? searchTerm, string? statusFilter, int page = 1)
        {
            const int PageSize = 5;

            try
            {
                string? userId = User.Identity?.IsAuthenticated == true
                    ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                    : null;

                bool isAdmin = User.IsInRole("Administrator");

                var result = await mailCarrierService.GetPaginatedAsync(
                    page, PageSize, searchTerm, statusFilter, userId, isAdmin);

                ViewBag.CurrentSearch = searchTerm;
                ViewBag.CurrentStatus = statusFilter;

                var statusOptions = (await mailCarrierService.GetAllAsync())
                    .Select(c => c.Status)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct()
                    .ToList();

                ViewBag.StatusOptions = statusOptions;
                ViewBag.ShowWarning = string.IsNullOrEmpty(userId) && !isAdmin;

                return View(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading MailCarrier list.");
                return RedirectToAction("Error500", "Home");
            }
        }

        [Authorize(Roles = "RegisteredUser,Administrator")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new MailCarrierCreateViewModel
            {
                StatusOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "-- Select Status --", Value = "" },
                    new SelectListItem { Text = "Available", Value = "Available" },
                    new SelectListItem { Text = "On Break", Value = "On Break" },
                    new SelectListItem { Text = "On a Delivery", Value = "On a Delivery" }
                }
            };

            return View(model);
        }

        [Authorize(Roles = "RegisteredUser,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MailCarrierCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.StatusOptions = GetStatusOptions();
                return View(model);
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await mailCarrierService.CreateAsync(model, userId); // 🔁 Pass userId to service
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating MailCarrier.");
                ModelState.AddModelError("", "An error occurred while creating the mail carrier.");
                model.StatusOptions = GetStatusOptions();
                return View(model);
            }
        }

        [Authorize(Roles = "RegisteredUser,Administrator")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await mailCarrierService.GetForEditAsync(id);
                if (model == null) return NotFound();

                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading MailCarrier for edit.");
                return RedirectToAction("Error500", "Home");
            }
        }

        [Authorize(Roles = "RegisteredUser,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MailCarrierEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.StatusOptions = GetStatusOptions();
                return View(model);
            }

            try
            {
                await mailCarrierService.EditAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error editing MailCarrier.");
                ModelState.AddModelError("", "An error occurred while editing the mail carrier.");
                model.StatusOptions = GetStatusOptions();
                return View(model);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = await mailCarrierService.GetByIdAsync(id);
                if (model == null) return NotFound();

                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading MailCarrier for deletion.");
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
                await mailCarrierService.SoftDeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting MailCarrier.");
                return RedirectToAction("Error500", "Home");
            }
        }

        // Helper to reuse dropdown list
        private List<SelectListItem> GetStatusOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "-- Select Status --", Value = "" },
                new SelectListItem { Text = "Available", Value = "Available" },
                new SelectListItem { Text = "On Break", Value = "On Break" },
                new SelectListItem { Text = "On a Delivery", Value = "On a Delivery" }
            };
        }
    }
}
