using WineshopManagerStarterKit.Models;

namespace WineshopManagerStarterKit.Data

{
    public static class ClientSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Clients.Any())
            {
                return;
            }
            context.Clients.AddRange(
                new Client { Name = "John Doe", Email = "john.doe@example.com", Street = "123 Main St", PostCode = "12345", City = "Anytown", Phone = "555-1234" },
                new Client { Name = "Jane Smith", Email = "jane.smith@example.com", Street = "456 Elm St", PostCode = "67890", City = "Othertown", Phone = "555-5678" }
            );
            context.SaveChanges();
        }
    }
}