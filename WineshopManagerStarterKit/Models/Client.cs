using System.ComponentModel.DataAnnotations;

namespace WineshopManagerStarterKit.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public string? email { get; set; }

        [StringLength(50)]
        public string? street { get; set; }

        [RegularExpression(@"^\d{5}$", ErrorMessage = "Post code must be a 5-digit number.")]
        public string? postCode { get; set; }

        [StringLength (50, MinimumLength = 2)]
        public string? city { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be a 10-digit number.")]
        public string? phone { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
