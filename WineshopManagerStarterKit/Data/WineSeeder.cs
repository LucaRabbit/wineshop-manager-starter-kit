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

        context.Wines.AddRange(
            new Wine { Name = "Château Margaux" },
            new Wine { Name = "Barolo Riserva" },
            new Wine { Name = "Chablis Premier Cru" },
            new Wine { Name = "Rioja Gran Reserva" },
            new Wine { Name = "Napa Valley Cabernet Sauvignon" }
        );

        context.SaveChanges();
    }
}
