using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WineshopManagerStarterKit.Models;

namespace WineshopManagerStarterKit.Data;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Wine> Wines { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
}