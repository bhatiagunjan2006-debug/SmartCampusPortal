using Microsoft.Data.Sqlite;
using Microsoft.OpenApi.Models;
using SmartCampusPortal.Data;

var builder = WebApplication.CreateBuilder(args);

// MVC + API controllers
builder.Services.AddControllersWithViews();

// Swagger (API testing page at /swagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Marwadi University - Smart Campus Service API",
        Version = "v1",
        Description = "REST API for Smart Campus service requests (Experiment 11)."
    });
});

// Resolve the SQLite file path (App_Data/SmartCampusDB.db inside the project folder)
var rawConn = builder.Configuration.GetConnectionString("SmartCampusDB") ?? "Data Source=App_Data/SmartCampusDB.db";
var csb = new SqliteConnectionStringBuilder(rawConn);
if (!Path.IsPathRooted(csb.DataSource))
    csb.DataSource = Path.Combine(builder.Environment.ContentRootPath, csb.DataSource);
builder.Services.AddSingleton(new DbSettings(csb.ConnectionString));

// One shared data-access layer used by BOTH the MVC controllers and the API controller
builder.Services.AddScoped<ServiceRequestDAL>();
builder.Services.AddSingleton<DatabaseInitializer>();

var app = builder.Build();

// Create SmartCampusDB + ServiceRequests table automatically if they do not exist
app.Services.GetRequiredService<DatabaseInitializer>().Initialize();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Campus API v1");
});

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
