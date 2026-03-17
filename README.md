# WineshopManager Starter Kit

A simple ASP.NET Web API project for managing a wine catalog.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (or the version matching your course)
- **One** of the following database servers:
  - **MySQL** (recommended) — e.g. via [XAMPP](https://www.apachefriends.org/), [MySQL Installer](https://dev.mysql.com/downloads/installer/), or Docker
  - **SQL Server** — e.g. [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or [SQL Server Developer](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

## Getting Started

### 1. Clone the repository

```bash
git clone <repository-url>
cd WineshopManagerStarterKit
```

### 2. Restore dependencies

```bash
dotnet restore
```

This downloads all required NuGet packages (Entity Framework Core, Pomelo MySQL, SQL Server provider, etc.).

### 3. Configure your database connection

Edit `WineshopManagerStarterKit/appsettings.json`:

#### If using MySQL (default)

The app tries to connect to MySQL first using the `DefaultConnection` string. Update it with your MySQL credentials:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=wineshop_db;User=root;Password=mysql;Convert Zero Datetime=true;"
  }
}
```

#### If using SQL Server

If MySQL is not available, the app automatically falls back to SQL Server using the `SqlServerConnection` string:

```json
{
  "ConnectionStrings": {
    "SqlServerConnection": "Server=localhost;Database=WineShopDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

> **Note:** The fallback is automatic — no configuration change is needed. If MySQL is running, it will be used. Otherwise, SQL Server is used.

### 4. Run the application

```bash
cd WineshopManagerStarterKit
dotnet run
```

The API will start on `http://localhost:5180` (or `https://localhost:7047` for HTTPS).

### 5. Test the API

You can use your browser, [Postman](https://www.postman.com/), or `curl` to test the endpoints:

| Method   | URL                          | Description        |
|----------|------------------------------|--------------------|
| `GET`    | `/api/wines`                 | Get all wines      |
| `GET`    | `/api/wines/{id}`            | Get a wine by ID   |
| `POST`   | `/api/wines`                 | Create a new wine  |
| `PUT`    | `/api/wines/{id}`            | Update a wine      |
| `DELETE` | `/api/wines/{id}`            | Delete a wine      |

#### Example: Get all wines

```bash
curl http://localhost:5180/api/wines
```

#### Example: Create a wine

```bash
curl -X POST http://localhost:5180/api/wines \
  -H "Content-Type: application/json" \
  -d '{"name": "Pinot Noir"}'
```

## Project Structure

```
WineshopManagerStarterKit/
├── Controllers/
│   └── WinesController.cs      # API endpoints for wines
├── Data/
│   ├── AppDbContext.cs          # Entity Framework database context
│   ├── DbSeeder.cs             # Main seeder (calls individual seeders)
│   └── WineSeeder.cs           # Seeds 5 wines into the database
├── Models/
│   ├── Wine.cs                 # Wine entity (Id, Name)
│   └── Supplier.cs             # Supplier entity (Id, Name)
├── Program.cs                  # App entry point, DB setup & configuration
└── appsettings.json            # Configuration (connection strings, etc.)
```

## Troubleshooting

- **"Failed to connect with mysql"**: Make sure your MySQL server is running and the credentials in `appsettings.json` are correct.
- **Build errors after cloning**: Run `dotnet restore` to install all NuGet dependencies.
- **Port already in use**: Change the port in `Properties/launchSettings.json`.

## Important: Updating Models

This project uses `EnsureCreated()` to create the database — **it does not use migrations**. This means that if you:

- Add a new model (e.g. a `Region` class)
- Add or rename a property on an existing model (e.g. adding `Price` to `Wine`)

...the existing database **will not be updated automatically**. You must **drop the database first** and let the app recreate it on the next run.

### How to drop the database

**MySQL** (via command line or phpMyAdmin):

```sql
DROP DATABASE wineshop_db;
```

**SQL Server** (via SSMS or command line):

```sql
DROP DATABASE WineShopDb;
```

Then simply run the app again — it will recreate the database with the updated schema and re-seed the data.

## Configuration: How `appsettings` Works

ASP.NET loads configuration in layers:

1. `appsettings.json` — base config, loaded in **all** environments
2. `appsettings.Development.json` — overrides/extends the base, loaded only in **Development** mode

You do **not** need to duplicate connection strings in `appsettings.Development.json`. Everything in `appsettings.json` is already available when running locally. The Development file is only useful if you want to override a specific value for local development.
