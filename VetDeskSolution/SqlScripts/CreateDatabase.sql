USE [master]
GO

CREATE DATABASE VetDesk
GO

CREATE LOGIN [vetdesk] WITH PASSWORD=N'[PASSWORD HERE]', DEFAULT_DATABASE=[VetDesk]
GO

USE [VetDesk]
GO

CREATE USER [vetdesk] FOR LOGIN [vetdesk] WITH DEFAULT_SCHEMA=[dbo]
GO

ALTER ROLE [db_owner] ADD MEMBER [vetdesk]
GO

