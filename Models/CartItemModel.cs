namespace ThriftShop.Models
{
    public class CartItemModel
    {
        public int CartId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

