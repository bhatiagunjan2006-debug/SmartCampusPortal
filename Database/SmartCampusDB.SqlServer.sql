/* OPTIONAL - equivalent SQL Server script for the lab report (SmartCampusDB / ServiceRequests).
   The running project uses SQLite, so this file is not needed to run the app. */

IF DB_ID(N'SmartCampusDB') IS NULL
    CREATE DATABASE SmartCampusDB;
GO
USE SmartCampusDB;
GO
IF OBJECT_ID(N'dbo.ServiceRequests', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ServiceRequests
    (
        RequestId   INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        StudentName NVARCHAR(100) NOT NULL,
        Department  NVARCHAR(100) NOT NULL,
        ServiceType NVARCHAR(50)  NOT NULL,
        Description NVARCHAR(500) NOT NULL,
        Priority    NVARCHAR(20)  NOT NULL DEFAULT 'Medium',
        Status      NVARCHAR(20)  NOT NULL DEFAULT 'Pending',
        RequestDate DATETIME      NOT NULL DEFAULT GETDATE()
    );
END
GO
