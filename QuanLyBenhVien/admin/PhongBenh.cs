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
    public partial class PhongBenh : UserControl
    {
        private string str = @"Data Source=laptop-e6bkathp\sqlexpress;Initial Catalog=QuanLyBenhVien;Integrated Security=True";

      
        public PhongBenh()
        {
            InitializeComponent();
        }
        private void XoaTrongCacO()
        {
            txtMa.Clear();
            txtTen.Clear();
            txtGiuong.Clear();
            txtGia.Clear();
            txtMaK.Clear();
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim();

            if (string.IsNullOrEmpty(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập Mã phòng hoặc Tên phòng bệnh cần tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tìm chính xác theo Mã phòng (số) hoặc tương đối theo Tên phòng (chữ)
            string query = @"SELECT TOP 1 MaPhongBenh , TenPhong, SoGiuongToiDa, GiaNgay, MaKhoa 
                             FROM PHONGBENH 
                             WHERE MaPhongBenh = @MaPhong_So OR TenPhong LIKE @Ten_Chu";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                int maPhong_So = -1;
                if (int.TryParse(tuKhoa, out int result))
                {
                    maPhong_So = result;
                }

                cmd.Parameters.AddWithValue("@MaPhong_So", maPhong_So);
                cmd.Parameters.AddWithValue("@Ten_Chu", "%" + tuKhoa + "%");

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Đổ dữ liệu lên giao diện
                            txtMa.Text = reader["MaPhongBenh"].ToString();
                            txtTen.Text = reader["TenPhong"].ToString();
                            txtGiuong.Text = reader["SoGiuongToiDa"].ToString();
                            txtGia.Text = Convert.ToDecimal(reader["GiaNgay"]).ToString("G29");
                            txtMaK.Text = reader["MaKhoa"].ToString();

                            MessageBox.Show("Đã tìm thấy thông tin phòng bệnh!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy phòng bệnh nào phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            XoaTrongCacO();
                        }
                    }

                    // Dù tìm thấy hay không, chạy xong là tự động dọn sạch ô tìm kiếm
                    txtTimKiem.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMa.Text))
            {
                MessageBox.Show("Vui lòng tìm kiếm phòng bệnh cần cập nhật trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTen.Text) || string.IsNullOrWhiteSpace(txtGiuong.Text) ||
                string.IsNullOrWhiteSpace(txtGia.Text) || string.IsNullOrWhiteSpace(txtMaK.Text))
            {
                MessageBox.Show("Vui lòng không để trống thông tin khi cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtGiuong.Text.Trim(), out int soGiuong) || !decimal.TryParse(txtGia.Text.Trim(), out decimal giaNgay))
            {
                MessageBox.Show("Số giường và Giá ngày phải là số hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(str))
            {
                string query = @"UPDATE PHONGBENH 
                                 SET TenPhong = @TenPhong, SoGiuongToiDa = @SoGiuong, GiaNgay = @GiaNgay, MaKhoa = @MaKhoa 
                                 WHERE MaPhong = @MaPhong";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPhong", Convert.ToInt32(txtMa.Text.Trim()));
                cmd.Parameters.AddWithValue("@TenPhong", txtTen.Text.Trim());
                cmd.Parameters.AddWithValue("@SoGiuong", soGiuong);
                cmd.Parameters.AddWithValue("@GiaNgay", giaNgay);
                cmd.Parameters.AddWithValue("@MaKhoa", Convert.ToInt32(txtMaK.Text.Trim()));

                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin phòng bệnh thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        XoaTrongCacO();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy phòng bệnh nào có mã số vừa nhập!", "Lỗi thông tin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text) || string.IsNullOrWhiteSpace(txtGiuong.Text) ||
                string.IsNullOrWhiteSpace(txtGia.Text) || string.IsNullOrWhiteSpace(txtMaK.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ tất cả các thông tin phòng bệnh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ép kiểu kiểm tra số giường và giá tiền
            if (!int.TryParse(txtGiuong.Text.Trim(), out int soGiuong) || !decimal.TryParse(txtGia.Text.Trim(), out decimal giaNgay))
            {
                MessageBox.Show("Số giường và Giá ngày phải nhập vào định dạng số hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(str))
            {
                // Giả định bảng PHONGBENH gồm các trường: TenPhong, SoGiuongToiDa, GiaNgay, MaKhoa
                string query = @"INSERT INTO PHONGBENH (TenPhong, SoGiuongToiDa, GiaNgay, MaKhoa) 
                                 VALUES (@TenPhong, @SoGiuong, @GiaNgay, @MaKhoa)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenPhong", txtTen.Text.Trim());
                cmd.Parameters.AddWithValue("@SoGiuong", soGiuong);
                cmd.Parameters.AddWithValue("@GiaNgay", giaNgay);
                cmd.Parameters.AddWithValue("@MaKhoa", Convert.ToInt32(txtMaK.Text.Trim()));

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm mới phòng bệnh thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    XoaTrongCacO();
                }
                catch (Exception ex)
                {
                    // Bắt lỗi nếu nhập Mã khoa bậy bạ không tồn tại bên bảng KHOA
                    MessageBox.Show("Lỗi khi thêm phòng bệnh! Hãy chắc chắn rằng Mã khoa bạn nhập là chính xác.\nChi tiết lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMa.Text))
            {
                MessageBox.Show("Vui lòng tìm kiếm phòng bệnh cần xóa trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maPhong = Convert.ToInt32(txtMa.Text.Trim());

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa phòng bệnh có Mã số {maPhong} không?\nNếu phòng đang có bệnh nhân nội trú, hệ thống sẽ chặn hành động này!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(str))
                {
                    string query = "DELETE FROM PHONGBENH WHERE MaPhong = @MaPhong";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaPhong", maPhong);

                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show($"Đã xóa thành công phòng bệnh số {maPhong}!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            XoaTrongCacO();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy phòng bệnh tương ứng để xóa!", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể xóa phòng bệnh này! Nguyên nhân có thể do đang có hồ sơ bệnh nhân nằm nội trú tại phòng này.\nChi tiết lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
    
}
