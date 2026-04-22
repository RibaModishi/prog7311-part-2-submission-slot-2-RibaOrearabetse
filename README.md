# GLMS (General Logistics Management System)

This is my PROG7311 POE Part 2 submission project.  
I built a logistics management web application in ASP.NET Core MVC where staff can manage clients, contracts, and service requests with business rules, API integration, and unit testing.

## Student Details

- **Module:** PROG7311
- **Assessment:** POE Part 2
- **Student Name:** Orearabetse Riba
- **Student Number:** ST10446648
- **Repository:** `<Your GitHub Repo URL>`

---

## Project Overview

GLMS is an internal system for managing:

- Client records
- Contract lifecycle and status
- Service requests linked to contracts
- Currency conversion from USD input to ZAR output
- Admin contract filtering by date range and status

This project uses Entity Framework Core (Code First), SQL Server, and ASP.NET Core MVC with Razor views.

---

## Tech Stack

- .NET / ASP.NET Core MVC
- Entity Framework Core (Code First)
- SQL Server (SQLEXPRESS)
- Razor Views + Bootstrap
- xUnit (unit testing)
- Frankfurter API (exchange rate data)

---

## Features Implemented

## 1) Core Data + Database Design

- Created models for `Client`, `Contract`, and `ServiceRequest`
- Added enum statuses (`ContractStatus`, `ServiceRequestStatus`)
- Configured `GlmsDbContext` with Fluent API:
  - keys
  - required and length rules
  - relationships
  - indexes
  - decimal precision for money
- Used EF migration to generate and apply schema to SQL Server

---

## 2) Workflow Logic

- Enforced business rule:
  - a `ServiceRequest` cannot be created if the parent contract is `Expired` or `OnHold`
- Implemented in service layer (`ServiceRequestWorkflowService`)
- Called from `ServiceRequestController` before saving
- Added admin search/filter with LINQ (`ContractQueryService`):
  - filter by status
  - filter by date range
  - combine both filters

---

## 3) External API Integration (Consumption)

- Integrated external currency API using `HttpClient`
- Endpoint used: Frankfurter (`USD -> ZAR`)
- On the create service request page:
  - user enters **USD** amount
  - system retrieves current USD/ZAR rate
  - UI shows a conversion preview
  - server converts and saves final cost in **ZAR**
- Added handling when API is unavailable (validation message shown and save blocked)

---

## 4) Unit Testing (Quality Assurance)

A separate test project was created: `GLMS.Tests` using **xUnit**.

Tested business logic includes:

- **Currency conversion**
  - correct math output for known inputs
  - rounding behavior
  - invalid input handling (negative amount / invalid rate)
- **File validation**
  - allows `.pdf`
  - rejects restricted types like `.exe`, `.png`, `.txt`
  - rejects empty file names

Current test result: **11 tests passed**.

### Unit Test Evidence

![Unit tests passing](https://github.com/user-attachments/assets/e02a121f-0dde-4b33-85cd-92342c0c1d1f)

```

---

## Security and Validation Notes

- `[Authorize]` used for authenticated access to protected pages
- `[ValidateAntiForgeryToken]` used on POST create action
- Data annotations used on view models for form validation
- Business rules validated server-side (not only JavaScript)

---

## How to Run the Project

1. Clone repository
2. Open solution in Visual Studio
3. Check connection string in `appsettings.json`
4. Run EF migration update:

```powershell
dotnet ef database update
```

5. Run project:

```powershell
dotnet run
```

6. Open browser and navigate to app URL

---

## How to Run Tests

From solution folder:

```powershell
dotnet test
```

or run tests from Visual Studio Test Explorer.

---

## AI Assistance Declaration

Parts of this project were assisted by **DeepSeek** for guidance, explanation support, and draft code suggestions.  
All implementation decisions, integration into this codebase, debugging, testing, and final submission checks were completed and verified by me.

---

## References 

Microsoft Corporation (2024) *EF Core DbContext configuration and dependency injection.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/ (Accessed: 18 April 2026).

Microsoft Corporation (2024) *EF Core model building.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/ef/core/modeling/ (Accessed: 18 April 2026).

Microsoft Corporation (2024) *EF Core relationships.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/ef/core/modeling/relationships (Accessed: 18 April 2026).

Microsoft Corporation (2024) *EF Core indexes.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/ef/core/modeling/indexes (Accessed: 18 April 2026).

Microsoft Corporation (2024) *EF Core entity properties.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/ef/core/modeling/entity-properties (Accessed: 18 April 2026).

Microsoft Corporation (2024) *Dependency injection in ASP.NET Core.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection (Accessed: 18 April 2026).

Microsoft Corporation (2024) *Controllers and actions in ASP.NET Core MVC.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/actions (Accessed: 20 April 2026).

Microsoft Corporation (2024) *Model binding in ASP.NET Core.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/model-binding (Accessed: 20 April 2026).

Microsoft Corporation (2024) *Model validation in ASP.NET Core MVC and Razor Pages.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation (Accessed: 20 April 2026).

Microsoft Corporation (2024) *Prevent Cross-Site Request Forgery (XSRF/CSRF) attacks in ASP.NET Core.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/aspnet/core/security/anti-request-forgery (Accessed: 20 April 2026).

Microsoft Corporation (2024) *Authorization in ASP.NET Core (Introduction).* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/introduction (Accessed: 20 April 2026).

Microsoft Corporation (2024) *Role-based authorization in ASP.NET Core.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles (Accessed: 20 April 2026).

Microsoft Corporation (2024) *Make HTTP requests using IHttpClientFactory in ASP.NET Core.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests (Accessed: 20 April 2026).

Microsoft Corporation (2024) *System.Text.Json overview.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview (Accessed: 20 April 2026).

Frankfurter (n.d.) *Frankfurter API Documentation.* Available at: https://www.frankfurter.app/docs/ (Accessed: 20 April 2026).

Microsoft Corporation (2024) *System.ComponentModel.DataAnnotations Namespace.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations (Accessed: 20 April 2026).

Microsoft Corporation (2024) *Querying Data in EF Core.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/ef/core/querying/ (Accessed: 20 April 2026).

Microsoft Corporation (2024) *Loading Related Data in EF Core.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/ef/core/querying/related-data/ (Accessed: 20 April 2026).

Microsoft Corporation (2024) *Math.Round Method.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/api/system.math.round (Accessed: 22 April 2026).

Microsoft Corporation (2024) *MidpointRounding Enum.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding (Accessed: 22 April 2026).

Microsoft Corporation (2024) *Path.GetExtension Method.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/api/system.io.path.getextension (Accessed: 22 April 2026).

Microsoft Corporation (2024) *StringComparison Enum.* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/api/system.stringcomparison (Accessed: 22 April 2026).

xUnit.net (n.d.) *Getting Started with xUnit.net.* Available at: https://xunit.net/docs/getting-started/v2/getting-started (Accessed: 22 April 2026).

xUnit API (n.d.) *Assert.Throws.* Available at: https://api.xunit.net/v3/2.0.1/v3.2.0.1-Xunit.Assert.Throws.html (Accessed: 22 April 2026).

xUnit API (n.d.) *Record.Exception.* Available at: https://api.xunit.net/v3/3.0.0/v3.3.0.0-Xunit.Record.Exception.html (Accessed: 22 April 2026).
```

