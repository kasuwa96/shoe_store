using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Data.SqlClient;

namespace test
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class index : System.Web.Services.WebService
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLlocaldb;Initial Catalog=DD_Footwear;Integrated Security=True;";

        [WebMethod]
        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ProductID, ProductName, Quantity, ImagePath, Category, Price FROM Products";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ImagePath = reader["ImagePath"].ToString(),
                        Category = reader["Category"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"])
                    };

                    products.Add(product);
                }

                reader.Close();
            }

            return products;
        }

        [WebMethod]
        public void UpdateProductQuantity(int productId, int quantityChange)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string updateQuery = "UPDATE Products SET Quantity = Quantity + @QuantityChange WHERE ProductID = @ProductID";

                SqlCommand command = new SqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@ProductID", productId);
                command.Parameters.AddWithValue("@QuantityChange", quantityChange);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public class Product
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public string ImagePath { get; set; }
            public string Category { get; set; }
            public decimal Price { get; set; }
        }
        [WebMethod]
        public void AddToCart(int productID, string selectedSize, int selectedQuantity)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO CartItems (ProductImage, ProductID, Size, Quantity) VALUES (@ProductImage, @ProductID, @Size, @Quantity)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    string imagePath = GetImagePathFromDatabase(productID); // Call the existing method to get image path
                    cmd.Parameters.AddWithValue("@ProductImage", imagePath);
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    cmd.Parameters.AddWithValue("@Size", selectedSize);
                    cmd.Parameters.AddWithValue("@Quantity", selectedQuantity);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }





    }
}
