USE UrlShortener
GO

IF (OBJECT_ID('UrlShorts') is not null)
	DROP TABLE UrlShorts
GO

CREATE TABLE UrlShorts
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	ShortUrl VARCHAR(4) NOT NULL,
	UserUrl VARCHAR(2040) NOT NULL
)
GO