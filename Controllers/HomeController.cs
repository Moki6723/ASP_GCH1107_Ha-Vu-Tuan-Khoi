using Asmmvc1670.Data;
using Asmmvc1670.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Asmmvc1670.Controllers
{
    public class HomeController : Controller
    {
        private readonly Asmmvc1670Context _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, Asmmvc1670Context context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var product = _context.Product.ToList();
            return View(product);
        }

        public IActionResult Privacy()
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