using System.ComponentModel.DataAnnotations;

namespace SmartCampusPortal.Models
{
    public class ServiceRequest
    {
        public int RequestId { get; set; }

        [Required(ErrorMessage = "Student name is required.")]
        [StringLength(100, ErrorMessage = "Student name cannot exceed 100 characters.")]
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [StringLength(100, ErrorMessage = "Department cannot exceed 100 characters.")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Please select a service type.")]
        [RegularExpression("^(IT Support|Library|Maintenance|Transport)$",
            ErrorMessage = "Service type must be IT Support, Library, Maintenance or Transport.")]
        [Display(Name = "Service Type")]
        public string ServiceType { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [RegularExpression("^(Low|Medium|High)$", ErrorMessage = "Priority must be Low, Medium or High.")]
        public string Priority { get; set; } = "Medium";

        // Set automatically by the server: always "Pending" on creation
        public string Status { get; set; } = "Pending";

        // Set automatically by the server (current date and time)
        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; }
    }
}
