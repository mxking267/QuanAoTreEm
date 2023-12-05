using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QuanAoTreEm.Models
{
    public class AddressModel
    {

        public string fullname { get; set; }
        public string phoneNumber { get; set; }
        public string province { get; set; }  
        public string district { get; set; }
        public string ward { get; set; }
        public string address { get; set; }
        public int addressID { get; set; }  

        public AddressModel() {
        }

        public AddressModel(string fullname, string phoneNumber, string province, string district, string ward, string address) {
            this.fullname = fullname;
            this.phoneNumber = phoneNumber;
            this.province = province;
            this. district = district;
            this.ward = ward;
            this.address = address;
        }

        
    }
}