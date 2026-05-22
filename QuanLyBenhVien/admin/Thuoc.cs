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
    public partial class Thuoc : UserControl
    {
        private string str = @"Data Source=laptop-e6bkathp\sqlexpress;Initial Catalog=QuanLyBenhVien;Integrated Security=True";

        
        public Thuoc()
        {
            InitializeComponent();
        }
        private void Thuoc_Load(object sender, EventArgs e)
        {
            LoadDuLieuComboBox();
        }
        private void LoadDuLieuComboBox()
        {
            // 1. Nạp danh mục Đơn vị tính thông dụng cho thuốc
            cboDonVi.Items.Clear();
            cboDonVi.Items.Add("Viên");
            cboDonVi.Items.Add("Vỉ");
            cboDonVi.Items.Add("Hộp");
            cboDonVi.Items.Add("Chai");
            cboDonVi.Items.Add("Ống");
            cboDonVi.Items.Add("Gói");
            if (cboDonVi.Items.Count > 0) cboDonVi.SelectedIndex = 0; // Chọn sẵn mục đầu tiên

          
        }
        // Hàm bổ trợ xóa trống các ô nhập liệu trên giao diện
        private void XoaTrongCacO()
        {
            txtMa.Clear();
            txtTen.Clear();
            txtGia.Clear();
            txtSoLuong.Clear(); // Xóa sạch chữ trong ô số lượng TextBox

            if (cboDonVi.Items.Count > 0) cboDonVi.SelectedIndex = 0;
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim();

            if (string.IsNullOrEmpty(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập Mã thuốc hoặc Tên thuốc cần tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Câu lệnh SQL lấy đầy đủ thông tin bao gồm SoLuongTon từ Database mới
            string query = @"SELECT TOP 1 MaThuoc, TenThuoc, DonViTinh, GiaHienTai, SoLuongTon 
                             FROM THUOC 
                             WHERE MaThuoc = @MaThuoc_So OR TenThuoc LIKE @Ten_Chu";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                int maThuoc_So = -1;
                if (int.TryParse(tuKhoa, out int result))
                {
                    maThuoc_So = result;
                }

                cmd.Parameters.AddWithValue("@MaThuoc_So", maThuoc_So);
                cmd.Parameters.AddWithValue("@Ten_Chu", "%" + tuKhoa + "%");

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Đổ dữ liệu tìm thấy lên giao diện màn hình
                            txtMa.Text = reader["MaThuoc"].ToString();
                            txtTen.Text = reader["TenThuoc"].ToString();
                            txtGia.Text = Convert.ToDecimal(reader["GiaHienTai"]).ToString("G29");

                            cboDonVi.Text = reader["DonViTinh"].ToString(); // ComboBox hiển thị Đơn vị tính
                            txtSoLuong.Text = reader["SoLuongTon"].ToString(); // TextBox hiển thị Số lượng tồn

                            MessageBox.Show("Đã tìm thấy thông tin thuốc!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thuốc nào phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            XoaTrongCacO();
                        }
                    }
                    txtTimKiem.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm thuốc: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text) || string.IsNullOrWhiteSpace(cboDonVi.Text) || string.IsNullOrWhiteSpace(txtGia.Text) || string.IsNullOrWhiteSpace(txtSoLuong.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin Tên thuốc, Đơn vị tính, Số lượng và Giá tiền!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtGia.Text.Trim(), out decimal giaHienTai))
            {
                MessageBox.Show("Giá hiện tại của thuốc phải ở định dạng số hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra bảo vệ ép kiểu: Người dùng phải gõ chữ số nguyên lớn hơn hoặc bằng 0 vào ô Số lượng
            if (!int.TryParse(txtSoLuong.Text.Trim(), out int soLuongTon) || soLuongTon < 0)
            {
                MessageBox.Show("Số lượng thuốc phải là số nguyên hợp lệ và không được âm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(str))
            {
                string query = @"INSERT INTO THUOC (TenThuoc, DonViTinh, GiaHienTai, SoLuongTon) 
                                 VALUES (@TenThuoc, @DonViTinh, @GiaHienTai, @SoLuongTon)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenThuoc", txtTen.Text.Trim());
                cmd.Parameters.AddWithValue("@DonViTinh", cboDonVi.Text.Trim());
                cmd.Parameters.AddWithValue("@GiaHienTai", giaHienTai);
                cmd.Parameters.AddWithValue("@SoLuongTon", soLuongTon);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm mới thuốc thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    XoaTrongCacO();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm thuốc mới: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMa.Text))
            {
                MessageBox.Show("Vui lòng tìm kiếm thông tin thuốc cần cập nhật trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTen.Text) || string.IsNullOrWhiteSpace(cboDonVi.Text) || string.IsNullOrWhiteSpace(txtGia.Text) || string.IsNullOrWhiteSpace(txtSoLuong.Text))
            {
                MessageBox.Show("Không được để trống thông tin khi cập nhật thuốc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtGia.Text.Trim(), out decimal giaHienTai))
            {
                MessageBox.Show("Giá hiện tại của thuốc phải là số hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtSoLuong.Text.Trim(), out int soLuongTon) || soLuongTon < 0)
            {
                MessageBox.Show("Số lượng cập nhật phải là số nguyên hợp lệ và không được âm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(str))
            {
                string query = @"UPDATE THUOC 
                                 SET TenThuoc = @TenThuoc, DonViTinh = @DonViTinh, GiaHienTai = @GiaHienTai, SoLuongTon = @SoLuongTon 
                                 WHERE MaThuoc = @MaThuoc";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaThuoc", Convert.ToInt32(txtMa.Text.Trim()));
                cmd.Parameters.AddWithValue("@TenThuoc", txtTen.Text.Trim());
                cmd.Parameters.AddWithValue("@DonViTinh", cboDonVi.Text.Trim());
                cmd.Parameters.AddWithValue("@GiaHienTai", giaHienTai);
                cmd.Parameters.AddWithValue("@SoLuongTon", soLuongTon);

                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin thuốc thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        XoaTrongCacO();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy mã thuốc tương ứng để cập nhật!", "Lỗi thông tin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật dữ liệu thuốc: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMa.Text))
            {
                MessageBox.Show("Vui lòng tìm kiếm thông tin thuốc cần xóa trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maThuoc = Convert.ToInt32(txtMa.Text.Trim());

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa thuốc có Mã số {maThuoc} không?\nNếu loại thuốc này đã từng có trong các đơn thuốc cũ, hệ thống sẽ chặn hành động xóa để giữ lịch sử!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(str))
                {
                    string query = "DELETE FROM THUOC WHERE MaThuoc = @MaThuoc";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaThuoc", maThuoc);

                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show($"Đã xóa thành công mã thuốc số {maThuoc} khỏi hệ thống!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            XoaTrongCacO();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thuốc tương ứng để xóa!", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể xóa thuốc này! Nguyên nhân do loại thuốc này đang nằm trong dữ liệu Toa thuốc/Đơn thuốc của bệnh nhân.\nChi tiết lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
    

