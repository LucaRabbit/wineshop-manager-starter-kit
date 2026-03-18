using System.Linq;
using WineshopManagerStarterKit.Models;

namespace WineshopManagerStarterKit.Data;

public static class WineSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Wines.Any())
        {
            return;
        }

        // Get types from database (seeded by TypeWineSeeder)
        var red = context.TypeWines.FirstOrDefault(t => t.Name == "Red");
        var white = context.TypeWines.FirstOrDefault(t => t.Name == "White");
        var sparkling = context.TypeWines.FirstOrDefault(t => t.Name == "Sparkling");
        var rosé = context.TypeWines.FirstOrDefault(t => t.Name == "Rosé");
        var dessert = context.TypeWines.FirstOrDefault(t => t.Name == "Dessert");

        context.Wines.AddRange(
            new Wine
            {
                Name = "Château Margaux",
                Type = red,
                quantity = 12,
                Price = 349.99f,
                TaxRate = 0.20f,
                WarningPoint = 3
            },
            new Wine
            {
                Name = "Barolo Riserva",
                Type = dessert,
                quantity = 8,
                Price = 129.50f,
                TaxRate = 0.20f,
                WarningPoint = 2
            },
            new Wine
            {
                Name = "Chablis Premier Cru",
                Type = white,
                quantity = 20,
                Price = 59.75f,
                TaxRate = 0.10f,
                WarningPoint = 5
            },
            new Wine
            {
                Name = "Rioja Gran Reserva",
                Type = sparkling,
                quantity = 15,
                Price = 39.99f,
                TaxRate = 0.20f,
                WarningPoint = 4
            },
            new Wine
            {
                Name = "Napa Valley Cabernet Sauvignon",
                Type = rosé,
                quantity = 10,
                Price = 89.00f,
                TaxRate = 0.20f,
                WarningPoint = 3
            }
        );

        context.SaveChanges();
    }
}
