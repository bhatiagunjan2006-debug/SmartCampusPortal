/* SQLite script (the database the project actually uses).
   The app runs this automatically on first start and creates App_Data/SmartCampusDB.db.
   You can open the .db file with "DB Browser for SQLite" or the VS Code "SQLite Viewer" extension. */

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
);

SELECT * FROM ServiceRequests;
