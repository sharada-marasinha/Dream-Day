# DreamDay

##Install EF tools
dotnet tool install --global dotnet-ef

## Add EF Core Tools and Migrations
```
dotnet ef dbcontext info -p DreamDay.DAL -s DreamDay.UI
dotnet ef migrations add InitialCreate -p DreamDay.DAL -s DreamDay.UI -o Data/Migrations
dotnet ef migrations remove -p DreamDay.DAL -s DreamDay.UI
dotnet ef database update -p DreamDay.DAL -s DreamDay.UI
dotnet ef migrations list -p DreamDay.DAL -s DreamDay.UI
```

## Ensure EF packages installed in DAL project
```
dotnet add DreamDay.DAL package Microsoft.EntityFrameworkCore
dotnet add DreamDay.DAL package Microsoft.EntityFrameworkCore.SqlServer
dotnet add DreamDay.DAL package Microsoft.EntityFrameworkCore.Tools
```

dotnet add DreamDay.UI package Microsoft.EntityFrameworkCore.Design

## push force
git push origin <branch-name> --force

dotnet add DreamDay.BLL package MailKit
dotnet remove DreamDay.BLL package MailKit
