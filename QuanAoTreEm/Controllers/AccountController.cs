using QuanAoTreEm.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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
        public ActionResult SignUp(FormCollection f,HttpPostedFileBase profileImage = null)
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



            byte[] profileImageBytes = null;
            if (profileImage != null && profileImage.ContentLength > 0)
            {
                using (BinaryReader reader = new BinaryReader(profileImage.InputStream))
                {
                    profileImageBytes = reader.ReadBytes(profileImage.ContentLength);
                }
            }

            account.ProfileImage = profileImageBytes;
            
            account.AddUser(account);

            return RedirectToAction("Profile", "Account");
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            account = new Account();
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(FormCollection f)
        {
            account = new Account();
            if (account.CheckSignIn(f["username"], f["password"]))
            {
                account = account.getUser(f["username"], f["password"]);
                return RedirectToAction("Profile", "Account", account);
            }
            return View();
        }
        
        public ActionResult Profile(Account account)
        {
            return View(account);
        }

       
    }
}