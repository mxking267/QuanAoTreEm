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
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImagePath { get; set; }

        // Constructor mặc định (không tham số)
        public Product()
        {
            db = new Database();
        }

        // Constructor với tham số
        public Product(string productName, string description, int categoryID, decimal price)
        {
            db = new Database();
            ProductName = productName;
            Description = description;
            CategoryID = categoryID;
            Price = price;
        }

        public Product(int id, string productName, string description, int categoryID,  decimal price)
        {
            db = new Database();
            ProductID = id;
            ProductName = productName;
            Description = description;
            CategoryID = categoryID;
            Price = price;
        }

        public Product(int id, string productName, string description, int categoryID,string categoryName , decimal price)
        {
            db = new Database();
            ProductID = id;
            ProductName = productName;
            Description = description;
            CategoryID = categoryID;
            CategoryName = categoryName;
            Price = price;
        }

        public List<Product> GetProducts()
        {
            string sql = "Select * From Product, Categories Where Product.CategoryID = Categories.CategoryID";
            return ConvertDataTableToList(db.Execute(sql));
        }

        public Product GetProductByID(int? id)
        {
            string sql = $"Select * From Product, Categories Where Product.CategoryID = Categories.CategoryID AND ProductID = " + id;
            DataTable dt = db.Execute(sql);
            DataRow row = dt.Rows[0];
            Product product = new Product()
            {
                ProductID = Convert.ToInt32(row["ProductID"]),
                ProductName = row["ProductName"].ToString(),
                Description = row["Description"].ToString(),
                CategoryID = Convert.ToInt32(row["CategoryID"].ToString()),
                CategoryName = row["CategoryName"].ToString(),
                Price = Convert.ToDecimal(row["Price"]),
            };
            return product;
        }

        public List<string> GetImages(int? id)
        {
            string sql = $"Select ImagePath From ProductImage Where ProductID = " + id;
            DataTable dt = db.Execute(sql);
            List<string> images = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                images.Add(dr[0].ToString());
            }
            return images;
        }

        public string GetImage(int? id)
        {
            string sql = $"Select Top 1 ImagePath From ProductImage Where ProductID = " + id;
            DataTable result = db.Execute(sql);

            if (result != null && result.Rows.Count > 0)
            {
                // Có bản ghi được trả về, trả về giá trị của cột ImagePath từ bản ghi đầu tiên
                return result.Rows[0]["ImagePath"].ToString();
            }
            else
            {
                // Không có bản ghi nào được trả về, xử lý tương ứng (ví dụ: trả về một giá trị mặc định)
                return "default.jpg";
            }
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
                    CategoryName = row["CategoryName"].ToString(),
                    Price = Convert.ToDecimal(row["Price"]),
                    
                    // Thêm các trường khác nếu cần
                };
                product.ImagePath = product.GetImage(product.ProductID);

                productList.Add(product);
            }

            return productList;
        
        
        }
        public int Insert(Product product)
        {
            string sql = $"Insert Into Product(ProductName, Description, CategoryID, Price) Values(N'{product.ProductName}', N'{product.Description}', {product.CategoryID}, {product.Price}); Select SCOPE_IDENTITY();";
            return Convert.ToInt32(db.ExecuteScalar(sql));
        }

        public void Update(Product product)
        {
            string sql = $"Update Product Set ProductName = N'{product.ProductName}',Description = N'{product.Description}', CategoryID = {product.CategoryID}, Price = {product.Price} Where ProductID = {product.ProductID}";
            db.ExecuteNonQuery(sql);
        }

        public void InsertImages(int id, List<string> images)
        {
            foreach(string image in images) {
                string sql = $"Insert Into ProductImage Values({id}, '{image}')";
                db.ExecuteNonQuery(sql);
            }
        }

        public void DeleteImages(int id)
        {
            string sql = $"Delete From ProductImage Where ProductID = {id}";
            db.ExecuteNonQuery(sql);
        }

        public void DeleteProduct(int id)
        {
            DeleteImages(id);
            string sql = $"Delete From Product Where ProductID = {id}";
            db.ExecuteNonQuery(sql);
        }
    }
}