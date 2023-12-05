using QuanAoTreEm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanAoTreEm.Controllers
{
    public class OrderController : Controller
    {
        Cart cart = new Cart();
        Order order = new Order();

        // GET: Order
        public ActionResult Index()
        {
            int? userId = Session["UserId"] as int?;
            if(userId.HasValue)
            {
                List<Order> lst = order.GetListOrder(userId.Value);
                return View(lst);
            }

            return View();
        }

        public ActionResult Details(int orderID)
        {

            return View();
        }

        public ActionResult Order(string address)
        {
            int? userId = Session["UserId"] as int?;
            if (userId.HasValue)
            {

                // Redirect về trang chi tiết sản phẩm hoặc trang giỏ hàng
                var carts = cart.getCartItems(userId.Value);
                order.order(carts, userId.Value, address);
            }
            return RedirectToAction("Index");
        }
    }
}