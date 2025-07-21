using Microsoft.Data.SqlClient;
using ThriftShop.Models;

namespace ThriftShop.DataAccessLayer
{
    public class PurchaseDAL : BaseDAL
    {
        public PurchaseDAL(IConfiguration configuration) : base(configuration) { }
        public List<Purchase> GetAllPurchases()
        {
            var purchases = new List<Purchase>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Purchases ORDER BY PurchaseDate DESC", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            purchases.Add(new Purchase
                            {
                                PurchaseId = Convert.ToInt32(reader["PurchaseId"]),
                                Username = reader["Username"].ToString(),
                                ProductName = reader["ProductName"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                TotalPrice = Convert.ToDecimal(reader["TotalPrice"]),
                                PurchaseDate = Convert.ToDateTime(reader["PurchaseDate"])
                            });
                        }
                    }
                }
            }

            return purchases;
        }

        public List<Purchase> GetPurchasesByUsername(string username)
        {
            var purchases = new List<Purchase>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Purchases WHERE Username = @Username ORDER BY PurchaseDate DESC", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            purchases.Add(new Purchase
                            {
                                PurchaseId = Convert.ToInt32(reader["PurchaseId"]),
                                Username = reader["Username"].ToString(),
                                ProductName = reader["ProductName"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                TotalPrice = Convert.ToDecimal(reader["TotalPrice"]),
                                PurchaseDate = Convert.ToDateTime(reader["PurchaseDate"])
                            });
                        }
                    }
                }
            }

            return purchases;
        }

        public void AddPurchase(Purchase purchase)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_AddPurchase", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Username", purchase.Username);
                    command.Parameters.AddWithValue("@ProductName", purchase.ProductName);
                    command.Parameters.AddWithValue("@Price", purchase.Price);
                    command.Parameters.AddWithValue("@Quantity", purchase.Quantity);
                    command.Parameters.AddWithValue("@TotalPrice", purchase.TotalPrice);
                    command.Parameters.AddWithValue("@PurchaseDate", purchase.PurchaseDate);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}