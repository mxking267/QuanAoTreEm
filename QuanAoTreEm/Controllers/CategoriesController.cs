using QuanAoTreEm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanAoTreEm.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: Categories
        Category category;
        public ActionResult Index()
        {
            category = new Category();
            return View(category.GetCategories());
        }
    }
}