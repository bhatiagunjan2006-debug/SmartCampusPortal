using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SmartCampusPortal.Data;
using SmartCampusPortal.Models;

namespace SmartCampusPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ServiceRequestDAL _dal;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ServiceRequestDAL dal, ILogger<HomeController> logger)
        {
            _dal = dal;
            _logger = logger;
        }

        // Landing page (campus hero)
        public IActionResult Index() => View();

        public IActionResult Dashboard()
        {
            var vm = new DashboardViewModel();
            try
            {
                vm = _dal.GetStats();
                vm.Recent = _dal.GetRecent(8);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Dashboard database error");
                vm.DbError = "Unable to access the database. Please check the connection string in appsettings.json and that the App_Data folder is writable.";
            }
            return View(vm);
        }

        public IActionResult ApiInfo() => View();

        public IActionResult About() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
