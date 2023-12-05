using QuanAoTreEm.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;

namespace QuanAoTreEm.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        Account account;
        private readonly VN_ProvincesEntities entities = new VN_ProvincesEntities();
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(FormCollection f)
        {
            if (f["password"] != f["retypePassword"])
                return View();

            string username = f["username"];
            string password = f["password"];
            string email = f["email"];
            string fullname = f["fullname"];
            string phoneNumber = f["phoneNumber"];
            string birthday = f["birthday"];
            string address = f["address"];
            string gender = f["gender"];
            string fileName = null;

            HttpPostedFileBase file = Request.Files["profileImage"];
            if (file != null && file.ContentLength > 0)
            {
                // Lưu ảnh vào thư mục
                string uploadFolder = Server.MapPath("~/Content/Assets/Images/Users");
                Directory.CreateDirectory(uploadFolder);
                string filePath = Path.Combine(uploadFolder, Path.GetFileName(file.FileName));
                file.SaveAs(filePath);

                // Lưu đường dẫn và thông tin người dùng vào cơ sở dữ liệu (hoặc thực hiện xử lý khác)
                // ...
                fileName = Path.GetFileName(file.FileName);
            }

            account = new Account();
            account.Username = username;
            account.Password = password;
            account.Email = email;
            account.FullName = fullname;
            account.PhoneNumber = phoneNumber;
            account.DateOfBirth = DateTime.Parse(birthday);
            account.Role = "User";
            account.Gender = gender;
            account.ProfileImage = fileName;

            account.AddUser(account);

            return RedirectToAction("SignIn", "Account");
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(FormCollection f)
        {
            account = new Account();
            if (!string.IsNullOrEmpty(account.CheckSignIn(f["username"], f["password"])))
            {
                Session["UserId"] = int.Parse(account.CheckSignIn(f["username"], f["password"]).ToString());

                if (account.getRole(f["username"], f["password"]) == "User")
                {
                    return RedirectToAction("Index", "Product");
                }
                else if (account.getRole(f["username"], f["password"]) == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                ViewBag.Message = "";
            }
            else
            {
                ViewBag.Message = "Sai tên đăng nhập hoặc mật khẩu";
            }
            return View();
        }

        public ActionResult Profile()
        {
            account = new Account();
            int? userId = Session["UserId"] as int?;
            if (userId == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            account = account.getUser(userId);
            ViewBag.UserImage = account.ProfileImage;
            return View(account);
        }

        public ActionResult Avatar()
        {
            account = new Account();
            int? userId = Session["UserId"] as int?;
            if (userId != null)
            {
                account = account.getUser(userId);
                ViewBag.UserImage = account.ProfileImage;
            }
            return View();
        }

        public ActionResult SignOut()
        {
            Session["UserId"] = null;
            return RedirectToAction("SignIn", "Account");
        }

        public ActionResult Address()
        {

            return View();
        }

        [HttpGet]
        public ActionResult AddAddress()
        {
            var provinces = entities.provinces.ToList();
            return View(provinces);
        }

        [HttpGet]
        public ActionResult UpdateAddress()
        {
            return View();
        }
    }
}