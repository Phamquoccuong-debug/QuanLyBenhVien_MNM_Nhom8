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
    public partial class DichVu : UserControl
    {
        private string str = @"Data Source=laptop-e6bkathp\sqlexpress;Initial Catalog=QuanLyBenhVien;Integrated Security=True";
        public DichVu()
        {
            InitializeComponent();
        }
        private void DichVu_Load(object sender, EventArgs e)
        {
            LoadComboBoxLoaiDV(); // Tự động nạp danh sách các Loại dịch vụ khi mở màn hình
        }
        private void LoadComboBoxLoaiDV()
        {
            // Tùy theo CSDL của bạn, nếu bạn có bảng LOAIDICHVU riêng thì dùng câu lệnh SQL.
            // Ở đây mình nạp sẵn một số loại phổ biến thường gặp trong bệnh viện, bạn có thể chỉnh sửa lại cho khớp nhé.
            cboLoaiDV.Items.Clear();
            cboLoaiDV.Items.Add("Khám bệnh");
            cboLoaiDV.Items.Add("Xét nghiệm");
            cboLoaiDV.Items.Add("Siêu âm / X-Quang");
            cboLoaiDV.Items.Add("Phẫu thuật / Thủ thuật");

            if (cboLoaiDV.Items.Count > 0) cboLoaiDV.SelectedIndex = 0;
        }

        // Hàm bổ trợ xóa trống các ô nhập liệu sau khi thao tác thành công
        private void XoaTrongCacO()
        {
            txtMaDV.Clear();
            txtTenDV.Clear();
            txtDonGia.Clear();
            txtNoiDung.Clear();
            if (cboLoaiDV.Items.Count > 0) cboLoaiDV.SelectedIndex = 0;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim();

            if (string.IsNullOrEmpty(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập Mã dịch vụ hoặc Tên dịch vụ cần tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Câu lệnh SQL tìm kiếm: Nhập số tìm chính xác theo mã, nhập chữ tìm tương đối theo tên
            string query = @"SELECT TOP 1 MaDV, TenDV, LoaiDV, Dongia, Noidung 
                     FROM DICHVU 
                     WHERE MaDV = @MaDV_So OR TenDV LIKE @Ten_Chu";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                int maDV_So = -1;
                if (int.TryParse(tuKhoa, out int result))
                {
                    maDV_So = result;
                }

                cmd.Parameters.AddWithValue("@MaDV_So", maDV_So);
                cmd.Parameters.AddWithValue("@Ten_Chu", "%" + tuKhoa + "%");

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Điền thông tin tìm thấy lên giao diện để xem hoặc sửa đổi
                            txtMaDV.Text = reader["MaDV"].ToString();
                            txtTenDV.Text = reader["TenDV"].ToString();
                            txtDonGia.Text = Convert.ToDecimal(reader["Dongia"]).ToString("G29"); // Định dạng hiển thị số sạch gọn
                            txtNoiDung.Text = reader["Noidung"].ToString();
                            cboLoaiDV.Text = reader["LoaiDV"].ToString();

                            MessageBox.Show("Đã tìm thấy thông tin dịch vụ!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                           
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy dịch vụ nào phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            XoaTrongCacO();
                        }
                    }
                    // Tự động xóa dữ liệu ô tìm kiếm sau khi tìm thấy thành công
                    txtTimKiem.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm dịch vụ: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    


        private void cboLoaiDV_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtNoiDung_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnThemDV_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu đầu vào không được để trống (Mã DV tự tăng nên không cần check)
            if (string.IsNullOrWhiteSpace(txtTenDV.Text) || string.IsNullOrWhiteSpace(txtDonGia.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin Tên dịch vụ và Đơn giá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra định dạng giá tiền nhập vào có phải là số hay không
            if (!decimal.TryParse(txtDonGia.Text.Trim(), out decimal donGia))
            {
                MessageBox.Show("Đơn giá nhập vào phải là số hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(str))
            {
                // Cấu trúc bảng DICHVU giả định gồm các trường: TenDV, LoaiDV, Dongia, Noidung
                string query = @"INSERT INTO DICHVU (TenDV, LoaiDV, Dongia, Noidung) 
                                 VALUES (@TenDV, @LoaiDV, @Dongia, @Noidung)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenDV", txtTenDV.Text.Trim());
                cmd.Parameters.AddWithValue("@LoaiDV", cboLoaiDV.Text);
                cmd.Parameters.AddWithValue("@Dongia", donGia);
                cmd.Parameters.AddWithValue("@Noidung", txtNoiDung.Text.Trim());

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm mới dịch vụ thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    XoaTrongCacO();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm dịch vụ: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaDV.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã dịch vụ cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenDV.Text) || string.IsNullOrWhiteSpace(txtDonGia.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin Tên dịch vụ và Đơn giá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtDonGia.Text.Trim(), out decimal donGia))
            {
                MessageBox.Show("Đơn giá nhập vào phải là số hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(str))
            {
                string query = @"UPDATE DICHVU 
                                 SET TenDV = @TenDV, LoaiDV = @LoaiDV, Dongia = @Dongia, Noidung = @Noidung 
                                 WHERE MaDV = @MaDV";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDV", Convert.ToInt32(txtMaDV.Text.Trim()));
                cmd.Parameters.AddWithValue("@TenDV", txtTenDV.Text.Trim());
                cmd.Parameters.AddWithValue("@LoaiDV", cboLoaiDV.Text);
                cmd.Parameters.AddWithValue("@Dongia", donGia);
                cmd.Parameters.AddWithValue("@Noidung", txtNoiDung.Text.Trim());

                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin dịch vụ thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        XoaTrongCacO();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy dịch vụ nào có mã số vừa nhập!", "Lỗi thông tin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật dịch vụ: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXoaDV_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaDV.Text))
            {
                MessageBox.Show("Vui lòng nhập hoặc Tìm kiếm Mã dịch vụ cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maDV = Convert.ToInt32(txtMaDV.Text.Trim());

            // 2. Hiện hộp thoại hỏi xác nhận để tránh việc người dùng bấm nhầm tay
            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa dịch vụ có Mã số {maDV} không?\nHành động này không thể hoàn tác!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(str))
                {
                    // Câu lệnh SQL xóa dịch vụ theo khóa chính MaDV
                    string query = "DELETE FROM DICHVU WHERE MaDV = @MaDV";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaDV", maDV);

                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();

                        // Nếu xóa thành công (tìm thấy mã để xóa)
                        if (rows > 0)
                        {
                            MessageBox.Show($"Đã xóa thành công dịch vụ có Mã số {maDV}!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            XoaTrongCacO(); // Xóa sạch chữ trên giao diện cho đỡ nhầm lẫn
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy dịch vụ nào có mã số này trong hệ thống để xóa!", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Trường hợp dịch vụ này đang được dùng ở bảng Khám Bệnh hoặc Hóa Đơn (bị ràng buộc khóa ngoại), SQL sẽ chặn không cho xóa
                        MessageBox.Show("Không thể xóa dịch vụ này! Nguyên nhân có thể do dịch vụ đang có lịch sử sử dụng ở các bảng Khám bệnh, Hóa đơn...\nChi tiết lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
