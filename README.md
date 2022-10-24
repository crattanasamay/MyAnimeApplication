# MyAnimeWebsite
A simple tracker where users can add their favorite anime to their list, and keep watch of current season changes in rating.
https://myanimewebsite.azurewebsites.net/
## Table of Contents
### Technologies Used
* C#
* CSS(Bootstrap)
* HTML
* Javascript
* RestSharp
* xUnit Testing
* Microsoft SQL

### Design Pattern
* For this project I mainly stuck with the MVC design princple that ASP.Net Core 6.0 has to offer.

### Api's
* MyAnimeList Rest API beta(v2) https://myanimelist.net/apiconfig/references/api/v2
* Jikans Anime API v4 https://jikan.moe/

### User Secrets 
```
{
  "MyAnimeListApiKey": "Your MyAnimeList API Key",
  "ConnectionStringAzure": "Your Connection String From Azure"
}
```
### Setting Up Database Tables
```
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Anime](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Season] [nvarchar](max) NOT NULL,
	[Summary] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Anime] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAnime](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AnimeId] [int] NOT NULL,
	[AnimeDateAdded] [datetime2](7) NOT NULL,
	[UserName] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserAnime] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Salt] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserAnime] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [AnimeDateAdded]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [Salt]
GO
```

