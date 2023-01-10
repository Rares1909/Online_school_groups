using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_school.Data;
using Online_school.Models;
using System.Diagnostics;

namespace Online_school.Controllers
{
    public class HomeController : Controller

    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger
        , ApplicationDbContext context,
            UserManager< ApplicationUser > userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;

            _userManager = userManager;

            _roleManager = roleManager;

            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Editor"))
                ViewBag.Role = "editor";
            if (User.IsInRole("User"))
                ViewBag.Role = "user";
            if (User.IsInRole("Admin"))
                ViewBag.Role = "admin";
            ViewBag.user = _userManager.GetUserId(User);
            return View();
        }

        public IActionResult Index2()
        {
            var user = _userManager.GetUserId(User);
            return RedirectToAction("Index2" ,"Groups", new{id=user });
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