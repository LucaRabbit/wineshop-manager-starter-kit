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
                new Client { Name = "John Doe", email = "john.doe@example.com", street = "123 Main St", postCode = "12345", city = "Anytown", phone = "555-1234" },
                new Client { Name = "Jane Smith", email = "jane.smith@example.com", street = "456 Elm St", postCode = "67890", city = "Othertown", phone = "555-5678" }
            );
            context.SaveChanges();
        }
    }
}