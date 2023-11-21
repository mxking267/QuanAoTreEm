using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

namespace QuanAoTreEm.Models
{
    public class Cart
    {
        Database db;
        public Cart()
        {
            db = new Database();
        }

        public void AddToCart(int userId, int productId, int quantity, decimal price)
        {
            // Kiểm tra xem giỏ hàng đã tồn tại chưa, nếu chưa thì tạo mới
            string checkCartQuery = $"SELECT * FROM Cart WHERE UserID = {userId}";

            // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng chưa, nếu chưa thì thêm mới
            string checkProductQuery = $"SELECT * FROM CartItems WHERE UserID = {userId} AND ProductID = {productId}";
            DataTable productResult = db.Execute(checkProductQuery);

            if (productResult.Rows.Count == 0)
            {
                // Nếu sản phẩm chưa tồn tại, thêm mới
                string addProductQuery = $"INSERT INTO CartItems (UserID, ProductID, Quantity, Price) " +
                    $"VALUES ({userId}, {productId}, {quantity}, {price})";
                db.ExecuteNonQuery(addProductQuery);
            }
            else
            {
                // Nếu sản phẩm đã tồn tại, cập nhật số lượng
                int existingQuantity = Convert.ToInt32(productResult.Rows[0]["Quantity"]);
                int newQuantity = existingQuantity + quantity;

                string updateProductQuery = $"UPDATE CartItems SET Quantity = {newQuantity} " +
                    $"WHERE ProductID = {productId} AND UserID = {userId}";
                db.ExecuteNonQuery(updateProductQuery);
            }
        }

        public List<CartItem> getCartItems(int UserID)
        {
            string query = $"SELECT * FROM CartItems, Product WHERE UserID = {UserID} AND CartItems.ProductID = Product.ProductID  ORDER BY CartItemID";
            DataTable dt = db.Execute(query);
            List<CartItem> cartItems = new List<CartItem>();

            foreach (DataRow row in dt.Rows)
            {
                int productId = Convert.ToInt32(row["ProductID"]);
                string imagePath;
                // Lấy đường dẫn của ảnh đầu tiên từ bảng ProductImages
                DataTable table = db.Execute($"SELECT TOP 1 ImagePath FROM ProductImage WHERE ProductID = {productId}");
                if(table.Rows.Count > 0)
                {
                    imagePath = table.Rows[0][0].ToString();
                }
                else
                {
                    imagePath = "default.jpg";
                }

                CartItem cartItem = new CartItem
                {
                    CartItemID = Convert.ToInt32(row["CartItemID"]),
                    UserID = Convert.ToInt32(row["UserID"]),
                    ProductName = row["ProductName"].ToString(),
                    Quantity = Convert.ToInt32(row["Quantity"]),
                    Price = Convert.ToDecimal(row["Price"]),
                    ImagePath = imagePath ?? "default.jpg",
                    ProductID = productId
                };
                cartItems.Add(cartItem);
            }

            return cartItems;
        }

        public void DeleteCartItem(int UserID, int ProductID)
        {
            string sql = $"Delete From CartItems Where UserID = {UserID} AND ProductID = {ProductID}";
            db.ExecuteNonQuery(sql);
        }
    }
}