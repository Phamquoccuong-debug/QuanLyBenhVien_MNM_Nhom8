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
using Sunny.UI.Win32;
using QuanLyBenhVien.ChuyenVienYTe;

namespace QuanLyBenhVien
{
    public partial class Login : Form
    {
        string str = @"Data Source=DESKTOP-L182KS3\SQLEXPRESS;Initial Catalog=QuanLyBenhVien;Integrated Security=True";

        public static string maNV_DangNhap { get; set; }
        public static string hoTenNV_DangNhap { get; set; }
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

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
            string query = @"
        SELECT 
            NV.HotenNV, 
            TK.MaNV,
            TK.Role
        FROM TAIKHOAN TK
        INNER JOIN NHANVIEN NV ON TK.MaNV = NV.MaNV
        WHERE TK.Username = @Username AND TK.Password = @Password";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
     
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            
                             maNV_DangNhap = reader["MaNV"].ToString();
                             hoTenNV_DangNhap = reader["HotenNV"].ToString();
                             string role = reader["Role"].ToString();

                            MessageBox.Show($"Đăng nhập thành công!\nXin chào: {hoTenNV_DangNhap}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            if (role == "Lễ tân")

                            {

                                Nurse form = new Nurse();
                                this.Hide();
                                form.ShowDialog();
                                
                            }

                            else if (role == "Kế toán")

                            {

                                KeToan form = new KeToan();
                                form.manv = maNV_DangNhap;
                                this.Hide();
                                form.ShowDialog();
                                
                            }

                            else if (role == "Bác sĩ")

                            {

                                BacSi form = new BacSi();
                                form.manv = maNV_DangNhap;
                                this.Hide();
                                form.ShowDialog();
                                
                            }

                            else if(role == "admin")
                            {

                                CRUD form = new CRUD();
                                this.Hide();
                                form.ShowDialog();
                                

                            }
                            else if(role == "Chuyên viên y tế")
                            {
                                KetQuaXetNhiem form = new KetQuaXetNhiem();
                                form.manv = maNV_DangNhap;
                                form.tennv = hoTenNV_DangNhap;
                                this.Hide();
                                form.ShowDialog();
                                
                            }
                            
                        }
                        else // Sai tài khoản hoặc mật khẩu
                        {
                            MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        

        private void uiLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
