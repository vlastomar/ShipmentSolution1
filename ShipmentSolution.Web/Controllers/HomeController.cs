using Microsoft.AspNetCore.Mvc;
using ShipmentSolution.Data.Models;
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Default error handler (used by default in development)
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // 🔴 404 Not Found custom page
        public IActionResult Error404()
        {
            return View("Error404");
        }

        // 🔥 500 Internal Server Error custom page
        public IActionResult Error500()
        {
            return View("Error500");
        }
    }
}
