using Microsoft.Data.SqlClient;
using System.Data;
using ThriftShop.Models;

namespace ThriftShop.DataAccessLayer
{
    public class ProductDAL : BaseDAL
    {
        //private readonly string _connectionString;

        //public ProductDAL(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetConnectionString("DefaultConnection");
        //}
        public ProductDAL(IConfiguration configuration) : base(configuration) { }

        public void InsertProduct(ProductModel product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_InsertProduct", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@StockStatus", product.StockStatus);
                    command.Parameters.AddWithValue("@Size", product.Size);
                    command.Parameters.AddWithValue("@Quantity", product.Quantity);
                    command.Parameters.AddWithValue("@ProductHeading", product.ProductHeading);
                    command.Parameters.AddWithValue("@Conditions", product.Conditions);
                    command.Parameters.AddWithValue("@ImagePaths", product.ImagePaths);
                    command.Parameters.AddWithValue("@category", product.category);
                    command.ExecuteNonQuery();
                }
            }
        }

        //public void UpdateProduct(ProductModel product)
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();
        //        using (SqlCommand command = new SqlCommand("sp_UpdateProduct", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@ProductId", product.ProductId);
        //            command.Parameters.AddWithValue("@ProductName", product.ProductName);
        //            command.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
        //            command.Parameters.AddWithValue("@Price", product.Price);
        //            command.Parameters.AddWithValue("@StockStatus", product.StockStatus);
        //            command.Parameters.AddWithValue("@Size", product.Size);
        //            command.Parameters.AddWithValue("@Quantity", product.Quantity);
        //            command.Parameters.AddWithValue("@ProductHeading", product.ProductHeading);
        //            command.Parameters.AddWithValue("@Conditions", product.Conditions);
        //            command.Parameters.AddWithValue("@ImagePaths", product.ImagePaths);

        //            command.ExecuteNonQuery();
        //        }
        //    }
        //}

        public void DeleteProduct(int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_DeleteProduct", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductId", productId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<ProductModel> GetAllProducts()
        {
            var products = new List<ProductModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_GetAllProducts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new ProductModel
                            {
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductName = reader["ProductName"].ToString(),
                                ProductDescription = reader["ProductDescription"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                StockStatus = reader["StockStatus"].ToString(),
                                Size = reader["Size"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                ProductHeading = reader["ProductHeading"].ToString(),
                                Conditions = reader["Conditions"].ToString(),
                                ImagePaths = reader["ImagePaths"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"])
                            });
                        }
                    }
                }
            }

            return products;
        }

        public List<ProductModel> GetAllProducts1()
        {
            var products = new List<ProductModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_GetAllProducts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new ProductModel
                            {
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductName = reader["ProductName"].ToString(),
                                ProductDescription = reader["ProductDescription"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                StockStatus = reader["StockStatus"].ToString(),
                                Size = reader["Size"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                ProductHeading = reader["ProductHeading"].ToString(),
                                Conditions = reader["Conditions"].ToString(),
                                ImagePaths = reader["ImagePaths"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"])
                            });
                        }
                    }
                }
            }

            return products;
        }


        public ProductModel GetProductById(int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_GetProductById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductId", productId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ProductModel
                            {
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductName = reader["ProductName"].ToString(),
                                ProductDescription = reader["ProductDescription"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                StockStatus = reader["StockStatus"].ToString(),
                                Size = reader["Size"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                ProductHeading = reader["ProductHeading"].ToString(),
                                Conditions = reader["Conditions"].ToString(),
                                ImagePaths = reader["ImagePaths"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"])
                            };
                        }
                    }
                }
            }
            return null; // Return null if no product is found
        }


        public void UpdateProduct(ProductModel product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_UpdateProduct", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    command.Parameters.AddWithValue("@ProductId", product.ProductId);
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@StockStatus", product.StockStatus);
                    command.Parameters.AddWithValue("@Size", product.Size);
                    command.Parameters.AddWithValue("@Quantity", product.Quantity);
                    command.Parameters.AddWithValue("@ProductHeading", product.ProductHeading);
                    command.Parameters.AddWithValue("@Conditions", product.Conditions);

                    // Handle null or empty ImagePaths
                    if (string.IsNullOrEmpty(product.ImagePaths))
                    {
                        command.Parameters.AddWithValue("@ImagePaths", DBNull.Value); // Pass NULL if no images
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ImagePaths", product.ImagePaths);
                    }

                    command.ExecuteNonQuery();
                }
            }
        }
        public List<CartItemModel> GetCartItemsByUserId(int userId)
        {
            var cartItems = new List<CartItemModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT 
                ci.CartItemId AS CartId,
                p.ProductName,
                p.Price,
                ci.Quantity,
                (p.Price * ci.Quantity) AS TotalPrice
            FROM CartItems ci
            INNER JOIN Products p ON ci.ProductId = p.ProductId
            WHERE ci.UserId = @UserId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cartItems.Add(new CartItemModel
                    {
                        CartId = Convert.ToInt32(reader["CartId"]),
                        ProductName = reader["ProductName"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        TotalPrice = Convert.ToDecimal(reader["TotalPrice"])
                    });
                }
            }

            return cartItems;
        }


        public List<OrderModel> GetAllOrders()
        {
            List<OrderModel> orders = new List<OrderModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Orders";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    orders.Add(new OrderModel
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        Status = reader["Status"].ToString(),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"])
                    });
                }
            }

            return orders;
        }

        public int InsertOrder(OrderModel order)
        {
            int orderId = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Orders (UserId, OrderDate, Status, TotalAmount) 
                         OUTPUT INSERTED.OrderId 
                         VALUES (@UserId, @OrderDate, @Status, @TotalAmount)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", order.UserId);
                cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                cmd.Parameters.AddWithValue("@Status", order.Status);
                cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);

                conn.Open();
                orderId = (int)cmd.ExecuteScalar(); // This is critical
            }

            return orderId;
        }

        public void InsertOrderItem(OrderItemModel item)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO OrderItems (OrderId, ProductId, Price, Quantity)
                         VALUES (@OrderId, @ProductId, @Price, @Quantity)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderId", item.OrderId);
                cmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                cmd.Parameters.AddWithValue("@Price", item.Price);
                cmd.Parameters.AddWithValue("@Quantity", item.Quantity);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void ClearCart(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM CartItems WHERE UserId = @UserId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();
                cmd.ExecuteNonQuery(); // Executes the delete command
            }
        }

        public int GetProductIdByName(string productName)
        {
            int productId = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT ProductId FROM Products WHERE ProductName = @ProductName";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductName", productName);

                conn.Open();
                var result = cmd.ExecuteScalar(); // ExecuteScalar is great for single values
                if (result != null)
                {
                    productId = Convert.ToInt32(result);
                }
            }

            return productId;
        }




        public List<ProductModel> GetFilteredProducts(string searchTerm, string sortBy, string sizeFilter, string conditionFilter,string categoryfilter)
        {
            var products = new List<ProductModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
            SELECT 
                ProductId,
                ProductName,
                ProductDescription,
                Price,
                StockStatus,
                Size,
                Quantity,
                ProductHeading,
                Conditions,
                ImagePaths,
                CreatedAt,
                UpdatedAt,
                 category
            FROM Products
            WHERE 1=1";

                // Add search term filter
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query += " AND ProductName LIKE @SearchTerm";
                }

                // Add size filter
                if (!string.IsNullOrEmpty(sizeFilter))
                {
                    query += " AND Size = @SizeFilter";
                }

                // Add condition filter
                if (!string.IsNullOrEmpty(conditionFilter))
                {
                    query += " AND Conditions = @ConditionFilter";
                }
                if (!string.IsNullOrEmpty(categoryfilter))
                {
                    query += " AND category = @Categoryfilter";
                }
                // Add sorting
                if (sortBy == "PriceLowToHigh")
                {
                    query += " ORDER BY Price ASC";
                }
                else if (sortBy == "PriceHighToLow")
                {
                    query += " ORDER BY Price DESC";
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                    }

                    if (!string.IsNullOrEmpty(sizeFilter))
                    {
                        command.Parameters.AddWithValue("@SizeFilter", sizeFilter);
                    }

                    if (!string.IsNullOrEmpty(conditionFilter))
                    {
                        command.Parameters.AddWithValue("@ConditionFilter", conditionFilter);
                    }
                    if (!string.IsNullOrEmpty(categoryfilter))
                    {
                        command.Parameters.AddWithValue("@categoryfilter", categoryfilter);
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new ProductModel
                            {
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductName = reader["ProductName"].ToString(),
                                ProductDescription = reader["ProductDescription"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                StockStatus = reader["StockStatus"].ToString(),
                                Size = reader["Size"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                ProductHeading = reader["ProductHeading"].ToString(),
                                Conditions = reader["Conditions"].ToString(),
                                ImagePaths = reader["ImagePaths"]?.ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"]),
                                category = reader["category"].ToString()
                            });
                        }
                    }
                }
            }

            return products;
        }

    }
}
