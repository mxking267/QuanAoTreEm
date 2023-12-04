using QuanAoTreEm.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanAoTreEm.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        Cart cart;
        public ActionResult Index()
        {
            cart = new Cart();
            int? userId = Session["UserId"] as int?;

            if (userId.HasValue)
            {

                // Redirect về trang chi tiết sản phẩm hoặc trang giỏ hàng
                return View(cart.getCartItems(userId.Value));
            }
            else
            {
                // Nếu userId không tồn tại trong session, có thể chuyển hướng đến trang đăng nhập
                return RedirectToAction("SignIn", "Account");
            }
            
        }

        public ActionResult AddToCart(int productID,decimal price)
        {
            cart = new Cart();
            int? userId = Session["UserId"] as int?;

            if (userId.HasValue)
            {
                // Gọi hàm xử lý thêm vào giỏ hàng với userId, productId, quantity, và price
                cart.AddToCart(userId.Value, productID, 1, price);

                // Redirect về trang chi tiết sản phẩm hoặc trang giỏ hàng
                return RedirectToAction("Detail", "Product", new { productId = productID });
            }
            else
            {
                // Nếu userId không tồn tại trong session, có thể chuyển hướng đến trang đăng nhập
                return RedirectToAction("SignIn", "Account");
            }
        }
        public ActionResult DeleteItem(int productID)
        {
            cart = new Cart();
            int? userId = Session["UserId"] as int?;
            cart.DeleteCartItem(userId.Value, productID);
            return RedirectToAction("Index", "Cart");
        }

        public ActionResult Update()
        {

            return RedirectToAction("Index", "Cart");
        }

        public ActionResult IncrementQuantity(int productId)
        {
            cart = new Cart();
            // Xử lý tăng giá trị quantity trong database
            // Cập nhật giá trị mới trong database
            int userId =Convert.ToInt32(Session["UserId"]);
            // Redirect về trang hiện tại sau khi xử lý
            cart.Change(productId, userId, 1);
            return RedirectToAction("Index");
        }

        public ActionResult DecrementQuantity(int productId)
        {
            cart = new Cart();
            // Xử lý giảm giá trị quantity trong database
            // Cập nhật giá trị mới trong database
            int userId = Convert.ToInt32(Session["UserId"]);
            // Redirect về trang hiện tại sau khi xử lý
            if(cart.getCartItems(userId).Find(product => product.ProductID == productId).Quantity > 1)
                cart.Change(productId, userId, -1);
  
            return RedirectToAction("Index");
        }

        public ActionResult BadgeCart()
        {
            cart = new Cart();
            int userId = Convert.ToInt32(Session["UserId"]);
            ViewBag.Count = cart.getCartItems(userId).Count;
            return View();
        }
    }
}