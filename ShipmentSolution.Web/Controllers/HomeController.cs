using Microsoft.AspNetCore.Mvc;
using ShipmentSolution.Data.Models;
using ShipmentSolution.Web.ViewModels;
using System.Diagnostics;

namespace ShipmentSolution.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Index page.");
                return RedirectToAction("Error500");
            }
        }

        public IActionResult Privacy()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Privacy page.");
                return RedirectToAction("Error500");
            }
        }

        // Default error handler
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            try
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error displaying error page.");
                return RedirectToAction("Error500");
            }
        }

        // 404 Not Found custom page
        public IActionResult Error404()
        {
            try
            {
                return View("Error404");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error displaying 404 page.");
                return RedirectToAction("Error500");
            }
        }

        // 500 Internal Server Error custom page
        public IActionResult Error500()
        {
            try
            {
                return View("Error500");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical failure displaying 500 page.");
                return Content("A critical error occurred and even the error page could not be displayed.");
            }
        }
    }
}
