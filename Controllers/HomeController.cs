using ajaxdemonew.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ajaxdemonew.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDBContext _context;
        public HomeController(ILogger<HomeController> logger, MyDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Address()
        {
            return View();
        }
        public IActionResult show()
        {
            return View();
        }
        public IActionResult register()
        {
            return View();
        }
        public IActionResult JSONTest()
        {
            return View();
        }
        public IActionResult CheckAccountAction()
        {
            return View();
        }
        public IActionResult first()
        {
            return View(_context.Categories);
        }
        public IActionResult travel()
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
