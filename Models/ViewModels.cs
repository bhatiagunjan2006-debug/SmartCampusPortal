using System.ComponentModel.DataAnnotations;

namespace SmartCampusPortal.Models
{
    public class DashboardViewModel
    {
        public int Total { get; set; }
        public int Pending { get; set; }
        public int InProgress { get; set; }
        public int Completed { get; set; }
        public List<ServiceRequest> Recent { get; set; } = new();
        public string DbError { get; set; }
    }

    public class ServiceRequestListViewModel
    {
        public List<ServiceRequest> Requests { get; set; } = new();
        public string Search { get; set; }
        public string ServiceType { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string DbError { get; set; }
    }

    // Body of PUT api/ServiceApi/UpdateStatus/{id}
    public class UpdateStatusDto
    {
        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }
    }

    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
