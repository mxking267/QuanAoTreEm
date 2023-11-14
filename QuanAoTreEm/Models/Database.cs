using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QuanAoTreEm.Models
{
    public class Database
    {
        public SqlConnection conn;
        public Database()
        {
            conn = new SqlConnection("Data Source=DESKTOP-LED4IU8\\SQLEXPRESS;Initial Catalog=QL_BanQuanAoTreEm;Integrated Security=True");
        }

        public DataTable Execute(string sql)
        {
            DataTable dataTable = new DataTable();

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    conn.Open();
                    adapter.Fill(dataTable);
                }
            }
            conn.Close();
            return dataTable;
        }

        public void ExecuteNonQuery(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }


    }
}