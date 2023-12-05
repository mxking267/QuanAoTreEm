using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QuanAoTreEm.Models
{
    public class UserAddress
    {
         Database db;
        public UserAddress()
        {
             db = new Database();
        }

        public List<AddressModel> getAddress(int userID)
        {
            string sql = "Select * From UserAdress Where UserID =" + userID;
            DataTable dt = db.Execute(sql);
            List<AddressModel> lst = new List<AddressModel>();
            foreach (DataRow dr in dt.Rows)
            {
                AddressModel model = new AddressModel();
                model.fullname = dr["FullName"].ToString();
                model.phoneNumber = dr["PhoneNumber"].ToString();
                model.address = dr["DiaChi"].ToString();
                model.ward = dr["XaPhuong"].ToString();
                model.district = dr["QuanHuyen"].ToString();
                model.province = dr["TinhTP"].ToString();
                lst.Add(model);
            }
            return lst;
        }
    }
}