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
        Account account = new Account();
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
            int? userId = Session["UserId"] as int?;
            var lst = account.getAddress(userId);
            return View(lst);
        }

        [HttpGet]
        public ActionResult AddAddress()
        {
            var provinces = entities.provinces.ToList();
            return View(provinces);
        }

        [HttpPost]
        public ActionResult AddAddress(FormCollection f)
        {
            string fullname = f["fullname"].ToString();
            string phoneNumber = f["phoneNumber"].ToString();
            string provinceCode = f["province"].ToString();
            string province = entities.provinces.Where(x => x.code == provinceCode)
    .Select(x => x.name).FirstOrDefault();
            string districtCode = f["district"].ToString();
            string district = entities.districts.Where(x => x.code == districtCode)
    .Select(x => x.name).FirstOrDefault();
            string wardCode = f["ward"].ToString();
            string ward = entities.wards.Where(x => x.code == wardCode)
    .Select(x => x.name).FirstOrDefault();
            string address = f["address"].ToString();
            AddressModel model = new AddressModel(fullname, phoneNumber, province, district, ward, address);
            int? userId = Session["UserId"] as int?;
            account.addAddress(userId, model);


            return RedirectToAction("Address", "Account");

        }

        [HttpGet]
        public ActionResult UpdateAddress(int addressID)
        {
            int? userId = Session["UserId"] as int?;
            var model = account.getAddressByID(userId, addressID);
            ViewBag.provinces = entities.provinces.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateAddress(int? addressID, FormCollection f)
        {
            string fullname = f["fullname"].ToString();
            string phoneNumber = f["phoneNumber"].ToString();
            string provinceCode = f["province"].ToString();
            string province = entities.provinces.Where(x => x.code == provinceCode)
    .Select(x => x.name).FirstOrDefault();
            string districtCode = f["district"].ToString();
            string district = entities.districts.Where(x => x.code == districtCode)
    .Select(x => x.name).FirstOrDefault();
            string wardCode = f["ward"].ToString();
            string ward = entities.wards.Where(x => x.code == wardCode)
    .Select(x => x.name).FirstOrDefault();
            string address = f["address"].ToString();
            AddressModel model = new AddressModel(fullname, phoneNumber, province, district, ward, address);
            account.updateAddress(addressID, model);
            return RedirectToAction("Address", "Account");
        }

        public ActionResult DeleteAddress(int addressID)
        {
            int? userId = Session["UserId"] as int?;
            account.deleteAddress(userId, addressID);
            return RedirectToAction("Address", "Account");
        }

        [HttpGet]
        public ActionResult GetDistricts(string locationCode)
        {
            if (locationCode != null)
            {
                var districts = entities.districts
                .Where(d => d.province.code == locationCode)
                .ToList();
                return PartialView("_DistrictsPartial", districts);
            }

            return PartialView("_DistrictsPartial");

        }

        [HttpGet]
        public ActionResult GetWards(string districtCode)
        {
            if (districtCode != null)
            {
                var wards = entities.wards
                .Where(w => w.district.code == districtCode)
                .ToList();
                return PartialView("_WardsPartial", wards);
            }
            return PartialView("_WardsPartial");
        }

    }
}