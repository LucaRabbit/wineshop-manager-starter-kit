using Microsoft.EntityFrameworkCore;
using WineshopManagerStarterKit.Data;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Database configuration: try MySQL (Pomelo) first, then SQL Server, then MariaDB (Oracle provider)
var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
var sqlServerConnection = builder.Configuration.GetConnectionString("SqlServerConnection")
    ?? "Server=localhost;Database=WineShopDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
var mariaDbConnection = builder.Configuration.GetConnectionString("MariaDbConnection")
    ?? "Server=localhost;Port=3306;Database=wineshop_db;User=root;Password=;";

string dbProvider = "mysql";

// Test if MySQL is reachable (for Pomelo)
try
{
    using var testConnection = new MySqlConnector.MySqlConnection(mySqlConnection);
    testConnection.Open();
    testConnection.Close();
    Console.WriteLine("MySQL connection successful. Using Pomelo/MySQL.");
}
catch
{
    // MySQL (Pomelo) not available, try SQL Server
    try
    {
        using var testSqlConnection = new Microsoft.Data.SqlClient.SqlConnection(sqlServerConnection);
        testSqlConnection.Open();
        testSqlConnection.Close();
        dbProvider = "sqlserver";
        Console.WriteLine("SQL Server connection successful. Using SQL Server.");
    }
    catch
    {
        // SQL Server not available either, fall back to MariaDB (Oracle provider)
        dbProvider = "mariadb";
        Console.WriteLine("MySQL and SQL Server not available. Falling back to MariaDB (Oracle provider).");
    }
}

switch (dbProvider)
{
    case "mysql":
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(mySqlConnection!, ServerVersion.AutoDetect(mySqlConnection!)));
        break;
    case "sqlserver":
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(sqlServerConnection));
        break;
    case "mariadb":
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySQL(mariaDbConnection));
        break;
}

var app = builder.Build();

// Initialize database and seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    DbSeeder.Seed(context);
    Console.WriteLine("Database initialized and seeded successfully.");
}

app.MapControllers();

app.Run();
