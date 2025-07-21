namespace ThriftShop.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } // e.g. Pending, Completed, Cancelled
        public decimal TotalAmount { get; set; }

        // Optional: if you want to display order items together
        public List<OrderItemModel> Items { get; set; }
    }

}


