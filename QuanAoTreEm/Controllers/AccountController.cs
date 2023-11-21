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
            account.Address = address;
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
                return RedirectToAction("Profile", "Account");
            }
            return View();
        }

        public ActionResult Profile()
        {
            account = new Account();
            int? userId = Session["UserId"] as int?;
            account = account.getUser(userId);
            return View(account);
        }


    }
}