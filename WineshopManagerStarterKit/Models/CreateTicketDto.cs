using System.ComponentModel.DataAnnotations;

namespace WineshopManagerStarterKit.Models
{
    public class CreateTicketDto
    {
        [Required]
        public int ClientId { get; set; }

        [Required]
        public DateTime DateSale { get; set; }
    }
}
