using QuanAoTreEm.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace QuanAoTreEm.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        Product product;
        Category category;
        Account account;
        public ActionResult Index()
        {
            account = new Account();    
            int userId = Convert.ToInt32(Session["UserId"]);
            if(account.getRole(userId) == "Admin")
            {
                product = new Product();
                List<Product> list = product.GetProducts();
                return View(list);
            }
            return new RedirectResult("/Error/AccessDenied");
        }

        public ActionResult ProductPartial(Product item)
        {
            return View(item);
        }

        [HttpGet]
        public ActionResult Create()
        {
            category = new Category();
            product = new Product();
            List<Category> list = category.GetCategories();
            ViewBag.listCategories = list;
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection f, HttpPostedFileBase[] images) {
            product = new Product();
            string producName = f["ProductName"];
            string description = f["Description"];
            int categoryID = Convert.ToInt32(f["CategoryID"]);
            decimal price = decimal.Parse(f["Price"]);
            List<string> lstImages = new List<string>();

            foreach (var image in images)
            {
                if (image != null && image.ContentLength > 0)
                {
                    // Lưu trữ hoặc xử lý ảnh theo yêu cầu của bạn
                    string imagePath = SaveImage(image);
                    lstImages.Add(Path.GetFileName(image.FileName));
                }
            }
            Product newProduct = new Product(producName, description, categoryID, price);
            int newID = product.Insert(newProduct);
            product.InsertImages(newID, lstImages);
            return RedirectToAction("Index", "Admin");
        }
        
        public ActionResult Detail(int id)
        {
            category = new Category();
            product = new Product();
            List<Category> list = category.GetCategories();
            ViewBag.listCategories = list;

            if (TempData["UpdateMessage"] != null)
            {
                ViewBag.UpdateMessage = TempData["UpdateMessage"];
            }

            Product detail = product.GetProductByID(id);
            return View(detail);
        }

        [HttpPost]
        public ActionResult Update(FormCollection f)
        {
            product = new Product();
            int id = Convert.ToInt32(f["ProductID"]);
            string producName = f["ProductName"];
            string description = f["Description"];
            int categoryID = Convert.ToInt32(f["CategoryID"]);
            decimal price = decimal.Parse(f["Price"]);
            Product updateProduct = new Product(id, producName, description, categoryID, price);
            product.Update(updateProduct);

            TempData["UpdateMessage"] = "Cập nhật thành công!";

            return RedirectToAction("Detail", "Admin", new { id });
        }

        public ActionResult Delete(int id)
        {
            product = new Product();
            product.DeleteProduct(id);
            return RedirectToAction("Index", "Admin");
        }

        private string SaveImage(HttpPostedFileBase image)
        {
            // Xử lý lưu trữ ảnh vào thư mục và trả về đường dẫn
            // Đây là một phần của logic, bạn có thể điều chỉnh theo yêu cầu thực tế của bạn

            string imagePath = "/Content/Assets/Images/Product/" + Path.GetFileName(image.FileName);
            image.SaveAs(Server.MapPath(imagePath));
            return imagePath;
        }

        public ActionResult AccountManagement()
        {
            return View();
        }
    }
}