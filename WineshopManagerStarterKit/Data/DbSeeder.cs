namespace WineshopManagerStarterKit.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        TypeWineSeeder.Seed(context);
        WineSeeder.Seed(context);
        // Add other seeders here as needed, e.g.:
    }
}
