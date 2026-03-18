using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using WineshopManagerStarterKit.Data;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// Database configuration: try MySQL (Pomelo) first, then SQL Server, then MariaDB (Oracle provider)
var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=localhost;Port=3306;Database=wineshop_db;User=root;Password=;Convert Zero Datetime=true;";
var sqlServerConnection = builder.Configuration.GetConnectionString("SqlServerConnection")
    ?? "Server=localhost;Database=WineShopDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
var mariaDbConnection = builder.Configuration.GetConnectionString("MariaDbConnection")
    ?? "Server=localhost;Port=3306;Database=wineshop_db;User=root;Password=;";

string dbProvider = "mysql";

// Test if MySQL is reachable (for Pomelo)
try
{
    // Test MySQL connection using default database to avoid "database not found" errors
    var testMySqlConnectionString = mySqlConnection.Replace("Database=wineshop_db", "Database=mysql");
    using var testConnection = new MySqlConnector.MySqlConnection(testMySqlConnectionString);
    testConnection.Open();
    testConnection.Close();
    Console.WriteLine("MySQL connection successful. Using Pomelo/MySQL.");
}
catch
{
    // MySQL (Pomelo) not available, try SQL Server
    try
    {
        // Test SQL Server connection using master database to avoid "database not found" errors
        var testSqlServerConnectionString = sqlServerConnection.Replace("Database=WineShopDb", "Database=master");
        using var testSqlConnection = new Microsoft.Data.SqlClient.SqlConnection(testSqlServerConnectionString);
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

// Configure ASP.NET Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})

.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Configure JWT authentification
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? "ThisIsADefaultJwtKeyForDevelopmentOnly!ChangeThisInProduction!"; // Clé secrète pour signer les tokens JWT (doit être sécurisée en production)
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "WineshopManager"; // L'émetteur du token JWT (doit être une URL ou un nom unique en production)
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "WineshopManagerUsers"; // L'audience du token JWT (doit être une URL ou un nom unique en production)

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Utiliser JWT Bearer pour l'authentification par défaut
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Utiliser JWT Bearer pour l'authentification par défaut
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Validation de l'émetteur
        ValidateAudience = true, // Validation de l'audience
        ValidateLifetime = true, // Temps de durée de vie du token
        ValidateIssuerSigningKey = true, // Validation de la clé de signature
        ValidIssuer = jwtIssuer, // L'émetteur attendu
        ValidAudience = jwtAudience, // L'audience attendue
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)), // Clé de signature
    };
});

var app = builder.Build();

// Initialize database and seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    DbSeeder.Seed(context);

    // Seed Identity roles and admin user
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(); // Récupérer le RoleManager pour gérer les rôles d'Identity
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>(); // Récupérer le UserManager pour gérer les utilisateurs d'Identity
    await IdentitySeeder.SeedAsync(roleManager, userManager); // Appeler la méthode de seeding pour les rôles et l'utilisateur admin d'Identity

    Console.WriteLine("Database initialized and seeded successfully.");
}

app.UseAuthentication(); // Activer l'authentification
app.UseAuthorization(); // Activer l'autorisation

app.MapControllers();

app.Run();
