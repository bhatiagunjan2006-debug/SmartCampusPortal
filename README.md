# Marwadi University - Smart Campus Service Portal
### Experiment 11 - Feature Extension (MVC + Database + REST API)

ASP.NET Core MVC (.NET 8) · ADO.NET · **SQLite (no database installation)** · Web API · Swagger · Bootstrap 5

## What you need
Only the **.NET SDK 8 or newer** (you already have it if `dotnet --version` works).
No SQL Server, no LocalDB, no CDN/internet for the UI (Bootstrap, icons, jQuery are bundled in `wwwroot/lib`).
Internet is needed only once, for `dotnet restore` to download the NuGet packages.

## Run
**Easiest (Windows):** double-click `run.bat`.

**VS Code / terminal:**
```
cd SmartCampusPortal
dotnet run --launch-profile http
```
Open http://localhost:5080 (or press F5 in VS Code / Visual Studio).

The database file is created automatically at `App_Data/SmartCampusDB.db`.
Data stays saved after restarting the app. Delete that file to start fresh.

## Pages
| URL | Page |
|---|---|
| `/` | Landing page (campus hero) |
| `/Home/Dashboard` | Dashboard, live statistics |
| `/ServiceRequest/Create` | Submit request |
| `/ServiceRequest` | All requests: search, filters, update status |
| `/ServiceRequest/Track` | Track with progress indicator |
| `/Home/ApiInfo` | API documentation |
| `/swagger` | Swagger UI |

## API (same database as MVC)
| Method | URL | Body |
|---|---|---|
| GET | `/api/ServiceApi` | - |
| POST | `/api/ServiceApi` | JSON below |
| PUT | `/api/ServiceApi/UpdateStatus/{id}` | `{ "status": "In Progress" }` |

```json
{ "studentName": "Gunjan Bhatia", "department": "Computer Science & Engineering",
  "serviceType": "IT Support", "description": "Wi-Fi not working in Lab 5", "priority": "High" }
```
Test in Swagger (`/swagger` > Try it out > Execute) or Postman. Create a request via the API and it appears in *All Requests*.

## Campus photo
Put any campus photo at `wwwroot/images/campus.jpg` to use it as the landing-page background
(a built-in illustration is used until then).

## Structure
```
Models/  Data/ (ServiceRequestDAL, DatabaseInitializer)  Helpers/
Controllers/ (Home, ServiceRequest, ServiceApi)  Views/  wwwroot/  Database/
```
`Database/SmartCampusDB.SqlServer.sql` is an optional SQL Server version of the table for your lab report.
