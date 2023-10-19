using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SportEvent.Models;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace SportEvent.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IConfiguration configure)
        {
            _logger = logger;

        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}