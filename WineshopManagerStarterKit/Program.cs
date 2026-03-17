using Microsoft.EntityFrameworkCore;
using WineshopManagerStarterKit.Data;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Database configuration: try MySQL first, fall back to SQL Server
var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
var sqlServerConnection = builder.Configuration.GetConnectionString("SqlServerConnection")
    ?? "Server=localhost;Database=WineShopDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

var useMySql = true;

// Test if MySQL is reachable
try
{
    using var testConnection = new MySqlConnector.MySqlConnection(mySqlConnection);
    testConnection.Open();
    testConnection.Close();
    Console.WriteLine("MySQL connection successful. Using MySQL.");
}
catch
{
    useMySql = false;
    Console.WriteLine("MySQL is not available. Falling back to SQL Server.");
}

if (useMySql)
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseMySql(mySqlConnection!, ServerVersion.AutoDetect(mySqlConnection!)));
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(sqlServerConnection));
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
