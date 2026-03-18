namespace WineshopManagerStarterKit.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        WineSeeder.Seed(context);
        ClientSeeder.Seed(context);
        // Add other seeders here as needed, e.g.:
        // SupplierSeeder.Seed(context);
    }
}
