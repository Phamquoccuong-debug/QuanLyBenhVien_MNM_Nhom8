using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using QuanLyBenhVien.admin;

namespace QuanLyBenhVien
{
    public partial class Login : Form
    {
        string str = @"Data Source=DESKTOP-L182KS3\SQLEXPRESS;Initial Catalog=QuanLyBenhVien;Integrated Security=True";

        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void uiButton1_Click(object sender, EventArgs e)
        {

            string username = uiTextBox1.Text.Trim();
            string password = uiTextBox2.Text.Trim();

            using (SqlConnection connection = new SqlConnection(str))
            {
                connection.Open();
                string query = "select COUNT(*) form TAIKHOAN where Username = @U, Password = @P";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@U", username);
                cmd.Parameters.AddWithValue("P", password);

                cmd.ExecuteNonQuery();

                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    string query2 = "select role from TAIKHOAN where Username = @U, Password = @P";
                    SqlCommand cmd2 = new SqlCommand(query2, connection);
                    cmd.Parameters.AddWithValue("@U", username);
                    cmd.Parameters.AddWithValue("P", password);
                    using (SqlDataReader reader = cmd2.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string role = reader["role"].ToString();
                            if (role == "Lễ tân")
                            {
                                Nurse form = new Nurse();
                                form.ShowDialog();
                            }
                            else if(role =="Kế toán")
                            {
                                KeToan form = new KeToan();
                                form.ShowDialog();
                            }
                            else if(role == "Bác sĩ")
                            {
                                BacSi form = new BacSi();
                                form.ShowDialog();

                            }
                            else(role == "admin"){
                                CRUD form = new CRUD();
                                form.ShowDialog();
                            }
                        }
                    }

                }
            }
        }
    }
}
