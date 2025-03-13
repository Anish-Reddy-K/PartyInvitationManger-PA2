using Microsoft.AspNetCore.Mvc;
using PartyInvitationManager.Models;
using System.Diagnostics;

namespace PartyInvitationManager.Controllers
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
            // Check if the FirstVisit cookie exists
            if (!Request.Cookies.ContainsKey("FirstVisit"))
            {
                // Set the cookie with current datetime
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(1)
                };
                Response.Cookies.Append("FirstVisit", DateTime.Now.ToString(), cookieOptions);
            }

            return View();
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