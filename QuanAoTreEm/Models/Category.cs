using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QuanAoTreEm.Models
{
    public class Category
    {
        
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        Database db;
        // Constructor mặc định không tham số
        public Category()
        {
            db = new Database();
        }

        // Constructor với tham số
        public Category(int categoryID, string categoryName, string description)
        {
            db = new Database();
            CategoryID = categoryID;
            CategoryName = categoryName;
            Description = description;
        }

        public List<Category> GetCategories()
        {
            string sql = "Select * From Categories";
            return ConvertDataTableToList(db.Execute(sql));
        }

        public static List<Category> ConvertDataTableToList(DataTable dataTable)
        {
            List<Category> categoryList = new List<Category>();

            foreach (DataRow row in dataTable.Rows)
            {
                Category category = new Category()
                {
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    CategoryName = row["CategoryName"].ToString(),
                    Description = row["Description"].ToString()
                    // Thêm các trường khác nếu cần
                };

                categoryList.Add(category);
            }

            return categoryList;
        }
    }
}