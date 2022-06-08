IF NOT EXISTS (
   SELECT name
   FROM sys.databases
   WHERE name = N'ScrapperDB'
)
CREATE DATABASE ScrapperDB

IF OBJECT_ID('ScrapperDB.dbo.Stock', 'U') IS NULL
CREATE TABLE ScrapperDB.dbo.Stock
(
    TickerId BIGINT(1,1) PRIMARY KEY,
    Ticker  NVARCHAR(50) NOT NULL,
    Date NVARCHAR(10) NOT NULL, --mm/dd/yyyy (I don't care about time of day rn)
    Price DECIMAL(20,4) --most expensive stock ever to date was BRK.A at 487255. with 4 decimals that is just 10 digits, 20 should be pleanty
);

IF OBJECT_ID('ScrapperDB.dbo.Screener', 'U') IS NULL
CREATE TABLE ScrapperDB.dbo.Screener
(
    ScreenerId BIGINT(1,1) PRIMARY KEY,
    Title NVARCHAR(50) NOT NULL,
    Url NVARCHAR(MAX) NOT NULL,
);

IF OBJECT_ID('ScrapperDB.dbo.ScreenerRow', 'U') IS NULL
CREATE TABLE ScrapperDB.dbo.ScreenerRow
(
    ScreenerRowId BIGINT(1,1) PRIMARY KEY,
    ScreenerId BIGINT FOREIGN KEY REFERENCES ,
    TickerId BIGINT FOREIGN KEY,

    CONSTRAINT FK_Screener (ScreenerId)  REFERENCES Persons(PersonID)
);


CREATE LOGIN kepa WITH PASSWORD = 'reallySuperStrongPasswordBecauseThisDbDoesntNeedProtection';