namespace WineshopManagerStarterKit.Models
{
    public class Order
    {
        public int Id { get; set; }
        public Supplier supplier { get; set; }
        public DateTime DateOrder { get; set; }
        public DateTime DateDelivery { get; set; }
        public string Street { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
    }
}
