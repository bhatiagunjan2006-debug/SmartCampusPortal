namespace SmartCampusPortal.Helpers
{
    public static class Lookups
    {
        public static readonly string[] ServiceTypes = { "IT Support", "Library", "Maintenance", "Transport" };
        public static readonly string[] Priorities = { "Low", "Medium", "High" };
        public static readonly string[] Statuses = { "Pending", "In Progress", "Completed" };
        public static readonly string[] Departments =
        {
            "Computer Science & Engineering", "Information Technology", "Electrical Engineering",
            "Mechanical Engineering", "Civil Engineering", "Computer Applications (MCA/BCA)",
            "Management (MBA/BBA)", "Pharmacy", "Science"
        };
    }

    public static class UiHelper
    {
        public static string StatusClass(string status) => status switch
        {
            "Pending" => "badge-pending",
            "In Progress" => "badge-progress",
            "Completed" => "badge-done",
            _ => "bg-secondary"
        };

        public static string PriorityClass(string priority) => priority switch
        {
            "High" => "prio-high",
            "Medium" => "prio-medium",
            _ => "prio-low"
        };

        public static int StatusStep(string status) => status switch
        {
            "Pending" => 1,
            "In Progress" => 2,
            "Completed" => 3,
            _ => 1
        };

        public static string ServiceIcon(string type) => type switch
        {
            "IT Support" => "bi-pc-display",
            "Library" => "bi-book",
            "Maintenance" => "bi-tools",
            "Transport" => "bi-bus-front",
            _ => "bi-grid"
        };
    }
}
