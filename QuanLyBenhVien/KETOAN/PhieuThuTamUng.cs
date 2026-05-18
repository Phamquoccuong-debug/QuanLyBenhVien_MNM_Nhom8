using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace QuanLyBenhVien
{
    public partial class PhieuThuTamUng : UserControl
    {
        public string manv {  get; set; }
        string str = @"Data Source=DESKTOP-L182KS3\SQLEXPRESS;Initial Catalog=QuanLyBenhVien;Integrated Security=True";
        public PhieuThuTamUng()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using(SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                string query = "INSERT INTO PHIEUTHU(MaKB, LoaiPhieu, SoTien, NgayLap) VALUES(@MaKB, N'Tạm ứng', @SoTienNhapVao, GETDATE())";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKB", textBox1.Text);
                cmd.Parameters.AddWithValue("SoTienNhapVao", textBox2.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thanh toán thành công","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);

            }
        }

        private void PhieuThuTamUng_Load(object sender, EventArgs e)
        {
            textBox3.Text = manv;
        }
    }
}
