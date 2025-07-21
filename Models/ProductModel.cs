namespace ThriftShop.Models
{
    public class ProductModel
    {

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public string StockStatus { get; set; } // "InStock" or "OutOfStock"
        public string Size { get; set; } // Comma-separated sizes (XS,S,M,L,XL,XXL)
        public int Quantity { get; set; }
        public string ProductHeading { get; set; }
        public string Conditions { get; set; }
        public string ImagePaths { get; set; } // Comma-separated file paths
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? category{ get; set; }
    }
}
