namespace ThriftShop.Models
{
    public class OrderItemModel
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // Optional: include product info for display
        public string ProductName { get; set; }
    }

}
