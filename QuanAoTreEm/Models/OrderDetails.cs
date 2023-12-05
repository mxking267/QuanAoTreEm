using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanAoTreEm.Models
{
    public class OrderDetails
    {
        public int OrderDetailID { get; set; }
        public string ProductName { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }

        // Constructor mặc định
        public OrderDetails()
        {
        }

        // Constructor với tham số
        public OrderDetails(int orderdetailsID, string productName, int quantity, int price)
        {
            OrderDetailID = orderdetailsID;
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