using Microsoft.Data.Sqlite;

namespace SmartCampusPortal.Data
{
    /// <summary>
    /// Creates the SmartCampusDB database file and the ServiceRequests table on first run.
    /// SQLite needs no server or installation - the database is a single file in App_Data.
    /// </summary>
    public class DatabaseInitializer
    {
        private readonly string _connectionString;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(DbSettings settings, ILogger<DatabaseInitializer> logger)
        {
            _connectionString = settings.ConnectionString;
            _logger = logger;
        }

        public void Initialize()
        {
            try
            {
                var csb = new SqliteConnectionStringBuilder(_connectionString);
                var folder = Path.GetDirectoryName(csb.DataSource);
                if (!string.IsNullOrEmpty(folder)) Directory.CreateDirectory(folder);

                using var con = new SqliteConnection(_connectionString);
                con.Open();
                const string sql = @"
CREATE TABLE IF NOT EXISTS ServiceRequests
(
    RequestId   INTEGER PRIMARY KEY AUTOINCREMENT,
    StudentName TEXT NOT NULL,
    Department  TEXT NOT NULL,
    ServiceType TEXT NOT NULL,
    Description TEXT NOT NULL,
    Priority    TEXT NOT NULL DEFAULT 'Medium',
    Status      TEXT NOT NULL DEFAULT 'Pending',
    RequestDate TEXT NOT NULL DEFAULT (datetime('now','localtime'))
);";
                using var cmd = new SqliteCommand(sql, con);
                cmd.ExecuteNonQuery();
                _logger.LogInformation("SmartCampusDB ready at {Path}", csb.DataSource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not initialise SmartCampusDB.");
            }
        }
    }
}
