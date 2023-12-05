using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace QuanAoTreEm.Models
{
    public class Order
    {
        Database db;

        public int orderID { get; set; }
        public decimal total { get; set; }
        public DateTime orderDate { get; set; }
        public List<OrderDetails> orderList { get; set; }
        public Order()
        {
            db = new Database();
        }

        public List<Order> getFullOrder()
        {
            List<Order> orders = new List<Order>();
            string sql = "Select * From Orders Order By OrderDate";
            DataTable dt = db.Execute(sql);
            foreach (DataRow row in dt.Rows)
            {
                Order order = new Order();
                order.orderID = Convert.ToInt32(row["OrderID"]);
                order.orderDate = DateTime.Parse(row["OrderDate"].ToString());
                order.orderList = order.getOrderItem(order.orderID);
            }
            return orders;
        }

        public void order(List<CartItem> lst, int userID, string address)
        {
            DateTime now = DateTime.Now;
            decimal total = lst.Sum(item => item.Quantity * item.Price);
            string sql = $"Insert Into Orders(UserID,Total, OrderDate, Address) Values({userID},{total} ,'{now}', N'{address}'); Select SCOPE_IDENTITY()";
            int orderID = Convert.ToInt32(db.ExecuteScalar(sql));

            foreach (CartItem item in lst)
            {
                sql = $"Insert Into OrderDetails Values({orderID},{item.ProductID},{item.Quantity},{item.Price}, {item.Quantity * item.Price})";
                db.ExecuteNonQuery(sql);
            }

            sql = "Delete From CartItems Where UserID = " + userID;
            db.ExecuteNonQuery(sql);
        }

        public List<Order> GetListOrder(int userID)
        {
            List<Order> lst = new List<Order>();
            string sql = "Select * From Orders Where UserID = " + userID;
            DataTable dt = db.Execute(sql);
            foreach (DataRow dr in dt.Rows)
            {
                Order order = new Order();
                order.orderID = int.Parse(dr["OrdersID"].ToString());
                order.total = Convert.ToDecimal(dr["Total"].ToString());
                order.orderDate = DateTime.Parse(dr["OrderDate"].ToString());
                order.orderList = getOrderItem(order.orderID);
                lst.Add(order);
            }
            return lst;
        }

        public List<OrderDetails> getOrderItem(int orderID)
        {
            string query = $"SELECT * FROM OrderDetails, Product WHERE OrderID = {orderID} AND OrderDetails.ProductID = Product.ProductID ORDER BY OrderDetailID";
            DataTable dt = db.Execute(query);
            List<OrderDetails> orderList = new List<OrderDetails>();

            foreach (DataRow row in dt.Rows)
            {
                int productId = Convert.ToInt32(row["ProductID"]);
                string imagePath;
                // Lấy đường dẫn của ảnh đầu tiên từ bảng ProductImages
                DataTable table = db.Execute($"SELECT TOP 1 ImagePath FROM ProductImage WHERE ProductID = {productId}");
                if (table.Rows.Count > 0)
                {
                    imagePath = table.Rows[0][0].ToString();
                }
                else
                {
                    imagePath = "default.jpg";
                }

                OrderDetails orderDetail = new OrderDetails();
                orderDetail.OrderDetailID = Convert.ToInt32(row["OrderDetailID"]);
                orderDetail.ProductName = row["ProductName"].ToString();
                orderDetail.Quantity = Convert.ToInt32(row["Quantity"]);
                orderDetail.Price = Convert.ToInt32(row["Price"]);
                orderDetail.ImagePath = imagePath;
                orderList.Add(orderDetail);
            }

            return orderList;
        }
    }
}