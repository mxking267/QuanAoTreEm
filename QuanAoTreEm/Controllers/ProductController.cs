using QuanAoTreEm.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;

namespace QuanAoTreEm.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        Product product = new Product();
        public ActionResult Index()
        {
            product = new Product();
            List<Product> productList = product.GetProducts();
            return View(productList);
        }

        public ActionResult GetProductByCategory(int categoryId)
        {
            List<Product> productList = product.GetProductsByCategoyID(categoryId);

            return View(productList);
        }

        public ActionResult Detail(int? productId)
        {
            if (productId.HasValue)
            {
                Product pr = product.GetProductByID(productId);
                ViewBag.Images = pr.GetImages(productId);
                return View(pr);
            }
            return RedirectToAction("Index", "Product");
        }


    }
}