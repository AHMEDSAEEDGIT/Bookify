
# Bookify

Bookify is an ASP.NET Core MVC sample application demonstrating a production-friendly e-commerce/bookstore reference implementation. It focuses on clean architecture, security, identity integration, Entity Framework Core (code-first), repository patterns, and common real-world features such as email notifications, social login, and Stripe payments. This README documents domain problems, requirements, architecture and technical details, and contains setup and deployment guidance.

## Domain Problem

- Provide an online storefront for selling books and related products.
- Allow customers to browse catalog, authenticate, add products to cart, checkout with payments, and receive email notifications.
- Allow administrators to manage categories, products, and seed initial data.
- Secure user data, support social login, and support deployment to cloud (Azure).

## Goals

- Demonstrate an enterprise-friendly N-tier architecture in ASP.NET Core.
- Show Identity integration and how to extend the user model.
- Implement repository and unit-of-work patterns with EF Core code-first migrations and seeding.
- Implement real-world features: email, social auth, Stripe payments, sessions, view components and tempdata.

## Functional Requirements

- Browse catalog by category and product pages.
- Admin CRUD for categories and products.
- User registration, login, logout, profile (extended fields), and role-based authorization (Admin, Customer).
- Social login (Google, Facebook).
- Email notifications for registration, order confirmation, and password recovery.
- Shopping cart session, checkout flow and Stripe-based payments.
- Database seeding for initial categories and products.
- Deployment-ready for Azure (App Service + managed database).

## Non-Functional Requirements

- Secure authentication & authorization (ASP.NET Core Identity, password policy, roles).
- Configurable and secrets-managed settings (user-secrets / Azure Key Vault in production).
- Reliable persistence with EF Core migrations and connection resiliency.
- Scalable hosting on Azure App Service.
- Clear separation of concerns (N-Tier) for testability and maintainability.
- Logging and error handling using ASP.NET Core logging abstractions.

## Architecture Overview

- N-Tier separation: Presentation (Bookify), DataAccess (EF Core + Migrations), Models (shared domain objects), Repository/UnitOfWork (Repository project), Utility (shared constants), and optional Razor pages project for demos.
- Patterns used:
	- MVC for presentation and controllers.
	- Repository Pattern + Unit of Work for data access abstraction.
	- Dependency Injection via built-in ASP.NET Core DI container.

Project layout (overview):
- Bookify (ASP.NET Core MVC web app)
- Bookify.DataAccess (EF Core DbContext, Migrations)
- Bookify.Models (domain models: `Product`, `Category`, etc.)
- Repository (repositories, UnitOfWork)
- Bookify.Utility (shared static details/constants)

Refer to the repository for files like `Bookify.DataAccess/Data/ApplicationDbContext.cs` and `Bookify/Controllers`.

## Key Technologies

- ASP.NET Core (MVC)
- ASP.NET Core Identity (with custom user fields)
- Entity Framework Core (Code First)
- SQL Server (local / Azure SQL)
- Stripe (payments)
- Social authentication (Google, Facebook)
- SMTP / SendGrid (email notifications)
- Azure App Service (deployment)

## Technical Details and How-To

### 1) Project Structure and ASP.NET Core conventions

- `Program.cs` and `Startup` style configurations (in `Program.cs` for .NET 6+ minimal host). Use `ConfigureServices` to register DbContext, Identity, Stripe, SMTP, and repositories.
- Controllers live in `Bookify/Controllers`, views in `Bookify/Views` and shared partials in `Bookify/Views/Shared`.

### 2) Identity: extend user and add fields

- Create an application user class that extends `IdentityUser` (e.g., `ApplicationUser`) to add profile fields (FirstName, LastName, Company, etc.).
- Update `ApplicationDbContext` to use `ApplicationUser`.
- Update registration views and account management to capture/display new fields.

Example (conceptual):

```
public class ApplicationUser : IdentityUser
{
		public string FirstName { get; set; }
		public string LastName { get; set; }
}
```

Migrations will scaffold the new columns after you add the class and update the DbContext.

### 3) Entity Framework Core (Code First) and Migrations

- Add EF Core packages to `Bookify.DataAccess` (Microsoft.EntityFrameworkCore, Microsoft.EntityFrameworkCore.SqlServer, Microsoft.EntityFrameworkCore.Tools).
- Typical commands:

```
dotnet restore
dotnet ef migrations add InitialCreate --project Bookify.DataAccess --startup-project Bookify
dotnet ef database update --project Bookify.DataAccess --startup-project Bookify
```

- Use `ApplicationDbContext` to register DbSets for `Product`, `Category`, etc.

### 4) Repository Pattern and Unit of Work

- Repositories live under `Repository/` (e.g., `ProductRepository`, `CategoryRepository`). They provide an abstraction over `ApplicationDbContext`.
- `UnitOfWork` coordinates repositories and `Save()` commits.

Example usage in controllers:

```
private readonly IUnitOfWork _unitOfWork;
public ProductsController(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }

var product = _unitOfWork.Product.Get(id);
_unitOfWork.Save();
```

### 5) Authentication & Authorization

- Configure Identity in `Program.cs` (password options, lockout, cookie settings).
- Use role-based checks and authorize attributes: `[Authorize(Roles = "Admin")]`.
- For finer-grained control use policies and claims.

### 6) Email Notifications

- Implement an `IEmailSender` service backed by SMTP or SendGrid.
- Store SMTP credentials in configuration (use user-secrets in development, Key Vault in production).

Minimal example config keys (appsettings.json):

```
"Smtp": { "Host": "smtp.example.com", "Port": 587, "User": "...", "Password": "..." }
```

### 7) Google and Facebook Login

- Register your application with Google and Facebook to obtain ClientId/ClientSecret.
- Configure social authentication in `Program.cs`:

```
services.AddAuthentication()
		.AddGoogle(google => { google.ClientId = "..."; google.ClientSecret = "..."; })
		.AddFacebook(fb => { fb.AppId = "..."; fb.AppSecret = "..."; });
```

- Keep secrets out of source control (use `dotnet user-secrets` or environment variables or Azure Key Vault).

### 8) Payments with Stripe

- Use Stripe .NET library. Store `Stripe:SecretKey` and `Stripe:PublishableKey` in config.
- Create a backend endpoint to create a payment intent or process the charge server-side, and validate webhooks for order fulfillment.

Example minimal flow:

1. Client requests payment intent from server.
2. Server creates PaymentIntent with amount and currency and returns `client_secret`.
3. Client completes the payment with Stripe.js using `client_secret`.

### 9) Sessions, View Components and TempData

- Sessions: enable with `services.AddSession()` and `app.UseSession()`; store lightweight cart ids or serialized cart data.
- View Components: encapsulate reusable UI logic (e.g., cart summary component).
- TempData: use for short-lived data across a redirect (e.g., success messages); backed by cookie or session-based provider.

### 10) Data Seeding

- Implement seeding in `ApplicationDbContext` or in a startup seed helper: create roles, admin user, categories, and products.
- Ensure seeding runs after `Database.Migrate()` in startup when appropriate (guard behind environment check in production if desired).

### 11) Deployment to Azure

- Typical steps:
	- Create an Azure SQL Database and set firewall rules.
	- Create Azure App Service for the web app.
	- Configure connection strings and app settings in Azure (Client secrets, Stripe keys, SMTP settings).
	- Publish from Visual Studio or use `dotnet publish` + CI/CD pipeline (GitHub Actions or Azure DevOps).

Quick Azure CLI / GitHub Actions tips:

```
az webapp up --name my-bookify-app --resource-group my-rg --runtime "DOTNETCORE:7.0"
```

Or create a GitHub Actions workflow to build, run migrations, and deploy.

## Development Setup (local)

Prerequisites: .NET SDK (6/7), SQL Server (localdb or SQL Express), Node/npm for client-side assets if used.

Commands:

```
dotnet restore
dotnet build
dotnet ef database update --project Bookify.DataAccess --startup-project Bookify
dotnet run --project Bookify
```

Configuring secrets and keys locally:

```
cd Bookify
dotnet user-secrets init
dotnet user-secrets set "Authentication:Google:ClientId" "<id>"
dotnet user-secrets set "Authentication:Google:ClientSecret" "<secret>"
dotnet user-secrets set "Stripe:SecretKey" "sk_test_..."
```

## Testing & Troubleshooting

- Use logging (`ILogger<T>`) to trace runtime behavior.
- Common issues:
	- Migration errors: ensure startup project has correct DbContext and connection string.
	- Social login: ensure redirect URIs match registration.
	- Stripe webhooks: use Stripe CLI in dev to forward events.

