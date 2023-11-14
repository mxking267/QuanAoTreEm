using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

namespace QuanAoTreEm.Models
{
    public class Product
    {
        Database db;
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        // Constructor mặc định (không tham số)
        public Product()
        {
            db = new Database();
        }

        // Constructor với tham số
        public Product(string productName, string description, int categoryID, decimal price, int stockQuantity)
        {
            db = new Database();
            ProductName = productName;
            Description = description;
            CategoryID = categoryID;
            Price = price;
            StockQuantity = stockQuantity;
        }

        public List<Product> GetProducts()
        {
            string sql = "Select * From Product";
            return ConvertDataTableToList(db.Execute(sql));
        }

        public Product GetProductByID(int id)
        {
            string sql = $"Select * From Product Where ProductID = " + id;
            DataTable dt = db.Execute(sql);
            DataRow row = dt.Rows[0];
            Product product = new Product()
            {
                ProductID = Convert.ToInt32(row["ProductID"]),
                ProductName = row["ProductName"].ToString(),
                Description = row["Description"].ToString(),
                CategoryID = Convert.ToInt32(row["CategoryID"].ToString()),
                Price = Convert.ToDecimal(row["Price"]),
                StockQuantity = Convert.ToInt32(row["StockQuantity"])
            };
            return product;
        }

        public List<Product> GetProductsByCategoyID(int categoryID)
        {
            string sql = "Select * From Product Where CategoryID = " + categoryID;
            return ConvertDataTableToList(db.Execute(sql));
        }
        public static List<Product> ConvertDataTableToList(DataTable dataTable)
        {
            List<Product> productList = new List<Product>();

            foreach (DataRow row in dataTable.Rows)
            {
                Product product = new Product
                {
                    ProductID = Convert.ToInt32(row["ProductID"]),
                    ProductName = row["ProductName"].ToString(),
                    Description = row["Description"].ToString(),
                    CategoryID = Convert.ToInt32(row["CategoryID"].ToString()),
                    Price = Convert.ToDecimal(row["Price"]),
                    StockQuantity = Convert.ToInt32(row["StockQuantity"])
                    // Thêm các trường khác nếu cần
                };

                productList.Add(product);
            }

            return productList;
        }
    }
}