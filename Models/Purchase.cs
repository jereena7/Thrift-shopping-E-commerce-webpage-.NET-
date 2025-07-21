namespace ThriftShop.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public string Username { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
    }

}
