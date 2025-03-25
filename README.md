## CertifyMe

**🎓 Course Certificate Generator Web App (.NET)**

ASP.NET Core web application that enables users to upload an Excel file containing participant data, generate personalized PDF course completion certificates (supports template configuration) and automatically email each certificate to the respective participant.
## Installation
- Open root folder in VSCode
- Create .env file in CertifyMe/CertifyMe/.env
```
SMTP_HOST = "<smtp host>"
SMTP_PORT = <smtp port>
SMTP_USER = "<user>"
SMTP_PWD = "<password>"
DB_CONN = "<db connection string>"
```
- Create a local SQL Server database with name CertifyMeDb
- dotnet ef migrations add Initialize
- dotnet ef database update
- run .NET and React projects using VSCode debug menu

## Additionally
- add to .csproj
```
<PropertyGroup><PreserveCompilationContext>true</PreserveCompilationContext></PropertyGroup>
```

- add to appsettings.json
```
"ImportExcelFileSettings": {
    "StartRowNum": 2,
    "NameColNum": 2,
    "SurnameColNum": 3,
    "EmailColNum": 4,
    "CourseColNum": 5,
    "CompletedColNum": 6
  }
```

- ImportExcelService.cs
```
// EPPlus NonCommercial license is free to use in non-commercial applications            
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
```
## Tech Stack

**Client:** Vite, React, Type Script

**Server:** C#, .NET 8, ASP.NET WebAPI, Entity Framework, Swagger, EPPlus, RazorLight, PdfSharp


## License

[Creative Commons Attribution-NonCommercial 4.0 International (CC BY-NC 4.0)](https://creativecommons.org/licenses/by-nc/4.0/).
