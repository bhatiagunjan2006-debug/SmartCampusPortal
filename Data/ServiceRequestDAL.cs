using System.Text;
using Microsoft.Data.Sqlite;
using SmartCampusPortal.Models;

namespace SmartCampusPortal.Data
{
    /// <summary>
    /// ADO.NET data access layer (SQLite). Shared by the MVC controllers and the Web API controller.
    /// All queries are parameterised.
    /// </summary>
    public class ServiceRequestDAL
    {
        private const string Columns =
            "RequestId, StudentName, Department, ServiceType, Description, Priority, Status, RequestDate";

        private readonly string _connectionString;

        public ServiceRequestDAL(DbSettings settings)
        {
            _connectionString = settings.ConnectionString;
        }

        private static ServiceRequest Map(SqliteDataReader r) => new ServiceRequest
        {
            RequestId = r.GetInt32(0),
            StudentName = r.GetString(1),
            Department = r.GetString(2),
            ServiceType = r.GetString(3),
            Description = r.GetString(4),
            Priority = r.GetString(5),
            Status = r.GetString(6),
            RequestDate = DateTime.Parse(r.GetString(7))
        };

        public List<ServiceRequest> GetAll(string search = null, string serviceType = null,
                                           string priority = null, string status = null)
        {
            var list = new List<ServiceRequest>();
            var sql = new StringBuilder($"SELECT {Columns} FROM ServiceRequests WHERE 1 = 1");

            using var con = new SqliteConnection(_connectionString);
            using var cmd = con.CreateCommand();

            if (!string.IsNullOrWhiteSpace(search))
            {
                sql.Append(" AND (StudentName LIKE @search OR CAST(RequestId AS TEXT) = @exact)");
                cmd.Parameters.AddWithValue("@search", "%" + search.Trim() + "%");
                cmd.Parameters.AddWithValue("@exact", search.Trim().TrimStart('#'));
            }
            if (!string.IsNullOrWhiteSpace(serviceType))
            {
                sql.Append(" AND ServiceType = @serviceType");
                cmd.Parameters.AddWithValue("@serviceType", serviceType);
            }
            if (!string.IsNullOrWhiteSpace(priority))
            {
                sql.Append(" AND Priority = @priority");
                cmd.Parameters.AddWithValue("@priority", priority);
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                sql.Append(" AND Status = @status");
                cmd.Parameters.AddWithValue("@status", status);
            }
            sql.Append(" ORDER BY RequestDate DESC, RequestId DESC");
            cmd.CommandText = sql.ToString();

            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(Map(reader));
            return list;
        }

        public ServiceRequest GetById(int id)
        {
            using var con = new SqliteConnection(_connectionString);
            using var cmd = con.CreateCommand();
            cmd.CommandText = $"SELECT {Columns} FROM ServiceRequests WHERE RequestId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? Map(reader) : null;
        }

        /// <summary>Inserts a request. Status is always Pending and RequestDate is the current date/time.</summary>
        public int Insert(ServiceRequest r)
        {
            using var con = new SqliteConnection(_connectionString);
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
INSERT INTO ServiceRequests (StudentName, Department, ServiceType, Description, Priority, Status, RequestDate)
VALUES (@StudentName, @Department, @ServiceType, @Description, @Priority, 'Pending', @RequestDate);
SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("@StudentName", r.StudentName.Trim());
            cmd.Parameters.AddWithValue("@Department", r.Department.Trim());
            cmd.Parameters.AddWithValue("@ServiceType", r.ServiceType);
            cmd.Parameters.AddWithValue("@Description", r.Description.Trim());
            cmd.Parameters.AddWithValue("@Priority", string.IsNullOrWhiteSpace(r.Priority) ? "Medium" : r.Priority);
            cmd.Parameters.AddWithValue("@RequestDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            con.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>Returns true if a row was updated, false if the id does not exist.</summary>
        public bool UpdateStatus(int id, string status)
        {
            using var con = new SqliteConnection(_connectionString);
            using var cmd = con.CreateCommand();
            cmd.CommandText = "UPDATE ServiceRequests SET Status = @status WHERE RequestId = @id";
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public DashboardViewModel GetStats()
        {
            using var con = new SqliteConnection(_connectionString);
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
SELECT COUNT(*),
       IFNULL(SUM(CASE WHEN Status = 'Pending'     THEN 1 ELSE 0 END), 0),
       IFNULL(SUM(CASE WHEN Status = 'In Progress' THEN 1 ELSE 0 END), 0),
       IFNULL(SUM(CASE WHEN Status = 'Completed'   THEN 1 ELSE 0 END), 0)
FROM ServiceRequests";
            con.Open();
            using var r = cmd.ExecuteReader();
            var vm = new DashboardViewModel();
            if (r.Read())
            {
                vm.Total = r.GetInt32(0);
                vm.Pending = r.GetInt32(1);
                vm.InProgress = r.GetInt32(2);
                vm.Completed = r.GetInt32(3);
            }
            return vm;
        }

        public List<ServiceRequest> GetRecent(int count)
        {
            var list = new List<ServiceRequest>();
            using var con = new SqliteConnection(_connectionString);
            using var cmd = con.CreateCommand();
            cmd.CommandText = $"SELECT {Columns} FROM ServiceRequests ORDER BY RequestDate DESC, RequestId DESC LIMIT @n";
            cmd.Parameters.AddWithValue("@n", count);
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(Map(reader));
            return list;
        }
    }
}
