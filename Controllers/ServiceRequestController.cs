using Microsoft.AspNetCore.Mvc;
using SmartCampusPortal.Data;
using SmartCampusPortal.Helpers;
using SmartCampusPortal.Models;

namespace SmartCampusPortal.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly ServiceRequestDAL _dal;
        private readonly ILogger<ServiceRequestController> _logger;

        public ServiceRequestController(ServiceRequestDAL dal, ILogger<ServiceRequestController> logger)
        {
            _dal = dal;
            _logger = logger;
        }

        private const string DbErrorText =
            "Unable to access the database. Please check the connection string in appsettings.json and that the App_Data folder is writable.";

        // GET: /ServiceRequest  (All Requests with search + filters)
        public IActionResult Index(string search, string serviceType, string priority, string status)
        {
            var vm = new ServiceRequestListViewModel
            {
                Search = search, ServiceType = serviceType, Priority = priority, Status = status
            };
            try
            {
                vm.Requests = _dal.GetAll(search, serviceType, priority, status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading requests");
                vm.DbError = DbErrorText;
            }
            return View(vm);
        }

        // GET: /ServiceRequest/Create
        public IActionResult Create(string serviceType)
        {
            var model = new ServiceRequest { Priority = "Medium" };
            if (Lookups.ServiceTypes.Contains(serviceType)) model.ServiceType = serviceType;
            return View(model);
        }

        // POST: /ServiceRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ServiceRequest model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                int id = _dal.Insert(model);
                TempData["Success"] = $"Service request #{id} submitted successfully. Current status: Pending.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving request");
                ModelState.AddModelError(string.Empty, DbErrorText);
                return View(model);
            }
        }

        // POST: /ServiceRequest/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, string status, string returnUrl)
        {
            if (!Lookups.Statuses.Contains(status))
            {
                TempData["Error"] = "Invalid status selected.";
            }
            else
            {
                try
                {
                    if (_dal.UpdateStatus(id, status))
                        TempData["Success"] = $"Request #{id} status updated to \"{status}\".";
                    else
                        TempData["Error"] = $"Request #{id} was not found.";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating status");
                    TempData["Error"] = DbErrorText;
                }
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);
            return RedirectToAction(nameof(Index));
        }

        // GET: /ServiceRequest/Track?search=5
        public IActionResult Track(string search)
        {
            var vm = new ServiceRequestListViewModel { Search = search };
            try
            {
                var all = _dal.GetAll(search);
                vm.Requests = string.IsNullOrWhiteSpace(search) ? all.Take(10).ToList() : all;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking requests");
                vm.DbError = DbErrorText;
            }
            return View(vm);
        }
    }
}
