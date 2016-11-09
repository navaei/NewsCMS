# News CMS
News cms base on asp.net core

# Online demo (Azure Website)
http://www.tazeyab.com

## Prerequisite:
1. .Net core SDK (https://www.microsoft.com/net/core)
2.  Ms Sql Server

## echnologies and frameworks used:
1. .NET MVC Core 1.0.1 on .NET Core 1.0.1
2. ity Framework Core 1.0.1
3. .NET Identity Core 1.0
4. ofac 4.0.0
5. ular 1.5
6. iatR for domain event

## How to run on local
1. Install .NET Core SDK for Visual Studio (https://www.microsoft.com/net/core#windows)
2. Create a database in SQL Server
3. Update the connection string in appsettings.json in SimplCommerce.WebHost
4. Build whole solution. Sometimes you need to build twice to make sure all the build output of modules are copied to the WebHost
5. Open Package Manager Console Window and type "Update-Database" then press Enter. This action will create database schema
6. Run src/Database/StaticData.sql to create seeding data
7. Press Control + F5
