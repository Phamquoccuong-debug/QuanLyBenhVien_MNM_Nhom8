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

namespace QuanLyBenhVien.admin
{
    public partial class Khoa : UserControl
    {
        private string str = @"Data Source=laptop-e6bkathp\sqlexpress;Initial Catalog=QuanLyBenhVien;Integrated Security=True";

        
        public Khoa()
        {
            InitializeComponent();
        }
        private void XoaTrongCacO()
        {
            txtMaKhoa.Clear();
            txtTenKhoa.Clear();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim();

            if (string.IsNullOrEmpty(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập Mã khoa hoặc Tên khoa cần tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"SELECT TOP 1 MaKhoa, TenKhoa 
                             FROM KHOA 
                             WHERE MaKhoa = @MaKhoa_So OR TenKhoa LIKE @Ten_Chu";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                int maKhoa_So = -1;
                if (int.TryParse(tuKhoa, out int result))
                {
                    maKhoa_So = result;
                }

                cmd.Parameters.AddWithValue("@MaKhoa_So", maKhoa_So);
                cmd.Parameters.AddWithValue("@Ten_Chu", "%" + tuKhoa + "%");

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Đổ dữ liệu lên các ô nhập liệu
                            txtMaKhoa.Text = reader["MaKhoa"].ToString();
                            txtTenKhoa.Text = reader["TenKhoa"].ToString();

                            MessageBox.Show("Đã tìm thấy thông tin khoa!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy khoa nào phù hợp với từ khóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            XoaTrongCacO();
                        }
                    }
                    // Tự động xóa dữ liệu ô tìm kiếm sau khi tìm thấy thành công
                    txtTimKiem.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm khoa: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenKhoa.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên khoa cần thêm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(str))
            {
                string query = "INSERT INTO KHOA (TenKhoa) VALUES (@TenKhoa)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenKhoa", txtTenKhoa.Text.Trim());

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm khoa mới thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    XoaTrongCacO();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm khoa: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKhoa.Text))
            {
                MessageBox.Show("Vui lòng tìm kiếm Khoa cần cập nhật trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenKhoa.Text))
            {
                MessageBox.Show("Tên khoa không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(str))
            {
                string query = "UPDATE KHOA SET TenKhoa = @TenKhoa WHERE MaKhoa = @MaKhoa";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKhoa", Convert.ToInt32(txtMaKhoa.Text.Trim()));
                cmd.Parameters.AddWithValue("@TenKhoa", txtTenKhoa.Text.Trim());

                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin khoa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        XoaTrongCacO();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy khoa nào có mã số vừa nhập!", "Lỗi thông tin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật khoa: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pnlContent_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKhoa.Text))
            {
                MessageBox.Show("Vui lòng tìm kiếm Mã khoa cần xóa trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maKhoa = Convert.ToInt32(txtMaKhoa.Text.Trim());

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa khoa có Mã số {maKhoa} không?\nLưu ý: Nếu khoa này đang có nhân viên phụ trách, lệnh xóa sẽ bị hệ thống chặn lại để bảo vệ dữ liệu!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(str))
                {
                    string query = "DELETE FROM KHOA WHERE MaKhoa = @MaKhoa";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaKhoa", maKhoa);

                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show($"Đã xóa thành công khoa có Mã số {maKhoa}!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            XoaTrongCacO();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy khoa nào có mã số này để xóa!", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể xóa khoa này! Nguyên nhân do đang có nhân viên hoặc phòng bệnh thuộc về khoa này.\nChi thể lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
