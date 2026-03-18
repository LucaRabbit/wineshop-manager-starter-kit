using System.ComponentModel.DataAnnotations;

namespace WineshopManagerStarterKit.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        public Client Client { get; set; } = null!;

        [Required]
        public DateTime DateSale { get; set; }
        
    }
}
