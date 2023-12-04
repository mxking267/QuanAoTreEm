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
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }
        public string ProfileImage { get; set; }

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
                       string phoneNumber, DateTime? dateOfBirth, string gender,
                       string role, string profileImage)
        {
            db = new Database();
            UserID = userId;
            Username = username;
            Password = password;
            FullName = fullName;
            Email = email;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Role = role;
            ProfileImage = profileImage;
        }

        public string CheckSignIn(string username, string password)
        {
            string sql = $"Select UserID From Account Where Username = N'{username}' AND Password = '{password}'";
            DataTable dt = db.Execute(sql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
                return null;
        }

        public void AddUser(Account account)
        {
            string sql = @"INSERT INTO Account VALUES (@Username, @Password, @FullName, @Email, @PhoneNumber, @DateOfBirth, @Gender, @Role, @ProfileImage)";

            // Tạo SqlCommand và thêm các tham số
            using (SqlCommand cmd = new SqlCommand(sql, db.conn))
            {
                db.conn.Open();
                cmd.Parameters.AddWithValue("@Username", account.Username);
                cmd.Parameters.AddWithValue("@Password", account.Password);
                cmd.Parameters.AddWithValue("@FullName", account.FullName);
                cmd.Parameters.AddWithValue("@Email", account.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", account.PhoneNumber);
                cmd.Parameters.AddWithValue("@DateOfBirth", account.DateOfBirth ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Gender", account.Gender);
                cmd.Parameters.AddWithValue("@Role", account.Role);
                cmd.Parameters.AddWithValue("@ProfileImage", account.ProfileImage ?? (object)DBNull.Value);

                // Thực hiện câu truy vấn
                cmd.ExecuteNonQuery();
                db.conn.Close();
            }
        }

        public Account getUser(int? userID)
        {
            string sql = $"Select * From Account Where UserID = {userID}";

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
                DateOfBirth = DateTime.Parse(row["DateOfBirth"].ToString()),
                Gender = row["Gender"].ToString(),
                ProfileImage = row["ProfileImage"].ToString()
            };

            return account;
        }

        public string getRole(string userName, string userPassword)
        {
            string sql = $"Select Role From Account Where Username = N'{userName}' AND Password = '{userPassword}'";
            return db.Execute(sql).Rows[0]["Role"].ToString();
        }

        public string getRole(int userID)
        {
            string sql = $"Select Role From Account Where UserID = {userID}";
            return db.Execute(sql).Rows[0]["Role"].ToString();
        }
    }
}