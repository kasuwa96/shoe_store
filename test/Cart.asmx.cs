using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;

namespace test
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Cart : System.Web.Services.WebService
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLlocaldb;Initial Catalog=DD_Footwear;Integrated Security=True;";

        [WebMethod]
        public void AddToCart(int productID, string size, int quantity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO CartItems (ProductID, Size, Quantity) VALUES (@ProductID, @Size, @Quantity)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productID);
                    command.Parameters.AddWithValue("@Size", size);
                    command.Parameters.AddWithValue("@Quantity", quantity);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        [WebMethod]
        public void RemoveFromCart(int cartItemID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM CartItems WHERE CartItemID = @CartItemID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CartItemID", cartItemID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        [WebMethod]
        public void UpdateCartItemQuantity(int cartItemID, int newQuantity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE CartItems SET Quantity = @NewQuantity WHERE CartItemID = @CartItemID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewQuantity", newQuantity);
                    command.Parameters.AddWithValue("@CartItemID", cartItemID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        [WebMethod]
        public List<CartItem> GetCartItems()
        {
            List<CartItem> cartItems = new List<CartItem>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT CartItemID, ProductID, Size, Quantity FROM CartItems";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        CartItem cartItem = new CartItem
                        {
                            CartItemID = Convert.ToInt32(reader["CartItemID"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            Size = reader["Size"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"])
                        };

                        cartItems.Add(cartItem);
                    }

                    reader.Close();
                }
            }

            return cartItems;
        }

        public class CartItem
        {
            public int CartItemID { get; set; }
            public int ProductID { get; set; }
            public string Size { get; set; }
            public int Quantity { get; set; }
        }

        // Add more cart-related methods as needed...
    }
}
