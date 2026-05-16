using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.Expressions;
using System.Windows.Forms;

namespace QuanLyBenhVien
{
    public partial class ThongTin : UserControl
    {
        string str = @"Data Source=DESKTOP-L182KS3\SQLEXPRESS;Initial Catalog=QuanLyBenhVien;Integrated Security=True";
        
        //public static class DuLieu
        //{
        //    public static string MaBN { get; set; }
        //}
        
        // Them benh nhan vao trong database
        public void ThemBenhNhan()
        {
            string TenBN = txtTenBN.Text;
            string CCCD = txtCCCD.Text;
            string GT = radioButton1.Checked ? "Nam" : "Nữ";
            string NS = dtNgaySinh.Text;
            string DC = txtDiachi.Text;
            string DT = txtDienThoai.Text;

            string TenNN = txtTenNN.Text;
            string QH = txtQH.Text;
            string dc = txtDC.Text;
            string dt = txtDT.Text;

            string BHYT = txtBHYT.Text;
            string NC = dtNgayCap.Text;
            string NHH = dtNgayHet.Text;
            string MH = comboBox1.Text;

            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                // thêm vào bảng bệnh nhân
                string query1 = "insert into BENHNHAN(MaBN, HotenBN, Ngaysinh, Gioitinh, Diachi, Dienthoai) " +
                    "values (MaBN = @cccd, HotenBN = @ten, Ngaysinh = @ns, Gioitinh = @gt, @Diachi = @dc, Dienthoai = @dt)";
                SqlCommand cmd = new SqlCommand(query1, conn);
                cmd.Parameters.AddWithValue("@cc", CCCD);
                cmd.Parameters.AddWithValue("@ten", TenBN);
                cmd.Parameters.AddWithValue("@ns", NS);
                cmd.Parameters.AddWithValue("@gt", GT);
                cmd.Parameters.AddWithValue("@dc", DC);
                cmd.Parameters.AddWithValue("@dt", DT);
                cmd.ExecuteNonQuery();

                // them thông tin người nhà
                string query2 = "insert into NGUOINHA(HotenNN, QuanHe, Diachi, Dienthoai, MaBN) " +
                    "(values HotenNN = @tennn, QuanHe = @qh, Diachi = @dc, Dienthoai = @dt, MaBN = @cc)";
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@tennn", TenNN);
                cmd2.Parameters.AddWithValue("@qh", QH);
                cmd2.Parameters.AddWithValue("@dc", dc);
                cmd2.Parameters.AddWithValue("@dt", dt);
                cmd2.Parameters.AddWithValue("@cc", CCCD);

                //Thêm thông tin thẻ BHYT
                if (BHYT != null)
                {
                    string query3 = "insert into BHYT(MaBHYT,NgayCap,NgayHetHan,MucHuong, MaBN) " +
                        "values (MaBHYT = @bh, NgayCap = @nc, NgayHetHan = @nhh, MucHuong = @mh,MaBN = @cccd)";

                    SqlCommand cmd3 = new SqlCommand(query3, conn);
                    cmd3.Parameters.AddWithValue("@bh", BHYT);
                    cmd3.Parameters.AddWithValue("@nc", NC);
                    cmd3.Parameters.AddWithValue("@nhh", NHH);
                    cmd3.Parameters.AddWithValue("@mh", MH);
                    cmd3.Parameters.AddWithValue("@cccd", CCCD);
                    cmd3.ExecuteNonQuery();
                }


                MessageBox.Show("Thêm bệnh nhân thành công","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);

            }

        }
        public ThongTin()
        {
            InitializeComponent();
        }

        private void ThongTin_Load(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public static class DuLieu
        {
            public static string MaBN { get; set; }
            public static string BHYT {  get; set; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DuLieu.BHYT = txtBHYT.Text;
            DuLieu.MaBN = txtCCCD.Text;
            PhieuKhamBenh form = new PhieuKhamBenh();
            form.mabn = txtCCCD.Text;
            form.bhyt = txtBHYT.Text;
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ThemBenhNhan();
        }

        private void btCNBN_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                string gt = radioButton1.Checked ? "Nam" : "Nữ";
                conn.Open();
                string query = "update BENHNHAN set HotenBN = @ten, Ngaysinh = @ns, Gioitinh = @gt, @Diachi = @dc, Dienthoai = @dt where MaBN = @cccd";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@cc", txtCCCD.Text);
                cmd.Parameters.AddWithValue("@ten", txtTenBN.Text);
                cmd.Parameters.AddWithValue("@ns", dtNgaySinh.Text);
                cmd.Parameters.AddWithValue("@gt", gt);
                cmd.Parameters.AddWithValue("@dc", txtDiachi.Text);
                cmd.Parameters.AddWithValue("@dt", txtDienThoai.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void btnCNNN_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                string query2 = "update NGUOINHA set HotenNN = @tennn, QuanHe = @qh, Diachi = @dc, Dienthoai = @dt where MaBN = @cc)";
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@tennn", txtTenNN);
                cmd2.Parameters.AddWithValue("@qh", txtQH);
                cmd2.Parameters.AddWithValue("@dc", txtDC);
                cmd2.Parameters.AddWithValue("@dt", txtDT);
                cmd2.Parameters.AddWithValue("@cc", txtCCCD);
                cmd2.ExecuteNonQuery();
                MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCNBHYT_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                string query3 = "update BHYT " +
                            "set MaBHYT = @bh, NgayCap = @nc, NgayHetHan = @nhh, MucHuong = @m where MaBN = @cccd";

                SqlCommand cmd3 = new SqlCommand(query3, conn);
                cmd3.Parameters.AddWithValue("@bh",txtBHYT );
                cmd3.Parameters.AddWithValue("@nc", dtNgayCap);
                cmd3.Parameters.AddWithValue("@nhh", dtNgayHet);
                cmd3.Parameters.AddWithValue("@mh", comboBox1.Text);
                cmd3.ExecuteNonQuery();
                MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
