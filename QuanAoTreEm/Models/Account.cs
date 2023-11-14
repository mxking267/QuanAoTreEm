using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Web;

namespace QuanAoTreEm.Models
{
    public class Account
    {
        Database db;
        public int? UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }
        public byte[] ProfileImage { get; set; }

        // Constructor mặc định
        public Account()
        {
            db = new Database();
        }

        // Constructor với các trường bắt buộc
        public Account(string username, string password)
        {
            db = new Database();
            Username = username;
            Password = password;
        }

        // Constructor đầy đủ
        public Account(int? userId, string username, string password, string fullName, string email,
                       string phoneNumber, string address, DateTime? dateOfBirth, string gender,
                       string role, byte[] profileImage)
        {
            db = new Database();
            UserID = userId;
            Username = username;
            Password = password;
            FullName = fullName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Role = role;
            ProfileImage = profileImage;
        }

        public bool CheckSignIn(string username, string password)
        {
            string sql = $"Select * From Account Where Username = N'{username}' AND Password = '{password}'";
            return !(db.Execute(sql).Rows.Count == 0);
        }

        public void AddUser(Account account)
        {
            string sql = @"INSERT INTO Account VALUES (@Username, @Password, @FullName, @Email, @PhoneNumber, @Address, @DateOfBirth, @Gender, @Role, @ProfileImage)";

            // Tạo SqlCommand và thêm các tham số
            using (SqlCommand cmd = new SqlCommand(sql, db.conn))
            {
                db.conn.Open();
                cmd.Parameters.AddWithValue("@Username", account.Username);
                cmd.Parameters.AddWithValue("@Password", account.Password);
                cmd.Parameters.AddWithValue("@FullName", account.FullName);
                cmd.Parameters.AddWithValue("@Email", account.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", account.PhoneNumber);
                cmd.Parameters.AddWithValue("@Address", account.Address);
                cmd.Parameters.AddWithValue("@DateOfBirth", account.DateOfBirth ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Gender", account.Gender);
                cmd.Parameters.AddWithValue("@Role", account.Role);
                cmd.Parameters.AddWithValue("@ProfileImage", account.ProfileImage ?? (object)DBNull.Value);

                // Thực hiện câu truy vấn
                cmd.ExecuteNonQuery();
                db.conn.Close();
            }
        }

        public Account getUser(string username, string password)
        {
            string sql = $"Select * From Account Where Username = N'{username}' AND Password = '{password}'";

            return ConvertDataTableToAccount(db.Execute(sql));
        }

        public static Account ConvertDataTableToAccount(DataTable dataTable)
        {
            DataRow row = dataTable.Rows[0];
            Account account = new Account()
            {
                UserID = int.Parse(row["UserID"].ToString()),
                FullName = row["FullName"].ToString(),
                Password = row["Password"].ToString(),
                Username = row["Username"].ToString(),
                Email = row["Email"].ToString(),
                PhoneNumber = row["PhoneNumber"].ToString(),
                Address = row["Address"].ToString(),
                DateOfBirth = DateTime.Parse(row["DateOfBirth"].ToString()),
                Gender = row["Gender"].ToString(),
                ProfileImage = ConvertVarbinaryToByteArray(row["ProfileImage"])
            };

            return account;
        }

        private static byte[] ConvertVarbinaryToByteArray(object varbinaryObject)
        {
            if (varbinaryObject == null || varbinaryObject == DBNull.Value)
            {
                return null;
            }

            // Kiểm tra kiểu của đối tượng để đảm bảo nó là varbinary
            if (varbinaryObject is byte[] byteArray)
            {
                return byteArray;
            }

            // Trường hợp đặc biệt nếu là MemoryStream
            if (varbinaryObject is MemoryStream memoryStream)
            {
                return memoryStream.ToArray();
            }

            // Trường hợp khác có thể xử lý tùy thuộc vào kiểu đối tượng
            // ...
            if (varbinaryObject is System.Data.SqlTypes.SqlBytes sqlBytes)
            {
                return sqlBytes.Value;
            }

            return null;
        }

    }
}