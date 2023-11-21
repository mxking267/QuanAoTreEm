using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace QuanAoTreEm.Models
{
    public class CartItem
    {
        public int CartItemID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }

        // Constructor mặc định
        public CartItem()
        {
        }

        // Constructor với tham số
        public CartItem(int cartItemID, int userID, string productName, int quantity, decimal price)
        {
            CartItemID = cartItemID;
            UserID = userID;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }

        public int GetPriceByQuantity()
        {
            return Convert.ToInt32(this.Quantity * this.Price);
        }

    }
}