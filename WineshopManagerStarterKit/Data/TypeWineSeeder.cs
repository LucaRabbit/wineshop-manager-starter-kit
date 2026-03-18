using System.Linq;
using WineshopManagerStarterKit.Models;

namespace WineshopManagerStarterKit.Data;

public static class TypeWineSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.TypeWines.Any())
        {
            return;
        }

        context.TypeWines.AddRange(
            new TypeWine { Name = "Red" },
            new TypeWine { Name = "White" },
            new TypeWine { Name = "Rosé" },
            new TypeWine { Name = "Sparkling" },
            new TypeWine { Name = "Dessert" }
        );

        context.SaveChanges();
    }
}
