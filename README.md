To execute this program locally, you need to :

1) Import the github repo
2) Download and install SQL Server Management Studio (https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)
3) Run SQL Server Management Studio, create a new DataBase (eg : TestDataBaseName)
4) Open the file XXXXXX, change the first line to USE [YourDataBaseName]
5) Run the content of the file XXX as a new script in your database
6) Change to connection string in appsettings.json such as : 
"DefaultConnection" = "Server=<server_name>;Database=<database_name>;User=<username>;Password=<password>;";
7. Run the project

Some data are already implemented such as the main admin user (email : garagevparrot@outlook.com, pwd : GarageVParrot1234!), the open hours, some reviews, cars and services for the exemple.


   
