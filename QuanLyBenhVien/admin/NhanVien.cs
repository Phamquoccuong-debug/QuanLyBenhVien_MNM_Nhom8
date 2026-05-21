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
namespace QuanLyBenhVien.admin
{
    public partial class NhanVien : UserControl
    {
        private string str = @"Data Source=laptop-e6bkathp\sqlexpress;Initial Catalog=QuanLyBenhVien;Integrated Security=True";
        public NhanVien()
        {
            InitializeComponent();
        }
        private void XoaTrongCacO()
        {
            txtMaNV.Clear();
            txtHoTen.Clear();

            // ĐÃ SỬA: Sử dụng SelectedIndex chuẩn của ComboBox thay vì hàm Clear() của TextBox
            cboChucVu.SelectedIndex = -1;

            txtTaiKhoan.Clear();
            txtMatKhau.Clear();

            if (cboKhoa.Items.Count > 0) cboKhoa.SelectedIndex = 0;
            if (cboQuyen.Items.Count > 0) cboQuyen.SelectedIndex = 0;
        }
        private void LoadComboBoxKhoa()
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                string query = "SELECT MaKhoa, TenKhoa FROM KHOA";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                try
                {
                    da.Fill(dt);
                    cboKhoa.DataSource = dt;
                    cboKhoa.DisplayMember = "TenKhoa";
                    cboKhoa.ValueMember = "MaKhoa";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải danh mục khoa: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(cboChucVu.Text) ||
                string.IsNullOrWhiteSpace(txtTaiKhoan.Text) ||
                string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin (Họ tên, Chức vụ, Tài khoản, Mật khẩu)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maKhoaSelected = Convert.ToInt32(cboKhoa.SelectedValue);
            string quyenHantext = cboQuyen.Text;

            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // BƯỚC A: Chèn thông tin vào bảng NHANVIEN và lấy ngay MaNV tự tăng
                    string queryNhanVien = @"
                        INSERT INTO NHANVIEN (HotenNV, Chucdanh, MaKhoa) 
                        VALUES (@HotenNV, @Chucdanh, @MaKhoa);
                        SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdNV = new SqlCommand(queryNhanVien, conn, transaction);
                    cmdNV.Parameters.AddWithValue("@HotenNV", txtHoTen.Text.Trim());
                    cmdNV.Parameters.AddWithValue("@Chucdanh", cboChucVu.Text.Trim());
                    cmdNV.Parameters.AddWithValue("@MaKhoa", maKhoaSelected);

                    int maNVVuaSinh = Convert.ToInt32(cmdNV.ExecuteScalar());

                    // BƯỚC B: Chèn thông tin vào bảng TAIKHOAN liên kết với MaNV
                    string queryTaiKhoan = @"
                        INSERT INTO TAIKHOAN (Username, Password, MaNV, Role) 
                        VALUES (@Username, @Password, @MaNV, @Role);";

                    SqlCommand cmdTK = new SqlCommand(queryTaiKhoan, conn, transaction);
                    cmdTK.Parameters.AddWithValue("@Username", txtTaiKhoan.Text.Trim());
                    cmdTK.Parameters.AddWithValue("@Password", txtMatKhau.Text.Trim());
                    cmdTK.Parameters.AddWithValue("@MaNV", maNVVuaSinh);
                    cmdTK.Parameters.AddWithValue("@Role", quyenHantext);

                    cmdTK.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Thêm nhân viên và tạo tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    XoaTrongCacO();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi khi thêm nhân viên: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void NhanVien_Load(object sender, EventArgs e)
        {
            LoadComboBoxKhoa();
        }
        
       

        private void cboKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim();

            if (string.IsNullOrEmpty(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập Mã nhân viên hoặc Họ tên cần tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"
                SELECT TOP 1
                    NV.MaNV, 
                    NV.HotenNV, 
                    NV.Chucdanh, 
                    NV.MaKhoa,
                    TK.Username,
                    TK.Password,
                    TK.Role
                FROM NHANVIEN NV
                LEFT JOIN TAIKHOAN TK ON NV.MaNV = TK.MaNV
                WHERE NV.MaNV = @MaNV_So OR NV.HotenNV LIKE @Ten_Chu";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                int maNV_So = -1;
                if (int.TryParse(tuKhoa, out int result))
                {
                    maNV_So = result;
                }

                cmd.Parameters.AddWithValue("@MaNV_So", maNV_So);
                cmd.Parameters.AddWithValue("@Ten_Chu", "%" + tuKhoa + "%");

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtMaNV.Text = reader["MaNV"].ToString();
                            txtHoTen.Text = reader["HotenNV"].ToString();
                            cboChucVu.Text = reader["Chucdanh"].ToString();
                            txtTaiKhoan.Text = reader["Username"].ToString();
                            txtMatKhau.Text = reader["Password"].ToString();

                            if (reader["MaKhoa"] != DBNull.Value)
                            {
                                cboKhoa.SelectedValue = Convert.ToInt32(reader["MaKhoa"]);
                            }

                            if (reader["Role"] != DBNull.Value)
                            {
                                cboQuyen.Text = reader["Role"].ToString();
                            }

                            MessageBox.Show("Đã tìm thấy thông tin nhân viên!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên nào khớp với mã hoặc tên đã nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            XoaTrongCacO();
                        }
                    }
                    txtTimKiem.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm nhân viên: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNV.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã nhân viên cần cập nhật thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(cboChucVu.Text) ||
                string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin (Họ tên, Chức vụ, Mật khẩu)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maNV = Convert.ToInt32(txtMaNV.Text.Trim());
            int maKhoaSelected = Convert.ToInt32(cboKhoa.SelectedValue);
            string quyenHantext = cboQuyen.Text;

            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // BƯỚC A: Cập nhật thông tin trong bảng NHANVIEN
                    string queryUpdateNV = @"
                        UPDATE NHANVIEN 
                        SET HotenNV = @HotenNV, Chucdanh = @Chucdanh, MaKhoa = @MaKhoa 
                        WHERE MaNV = @MaNV";

                    SqlCommand cmdNV = new SqlCommand(queryUpdateNV, conn, transaction);
                    cmdNV.Parameters.AddWithValue("@HotenNV", txtHoTen.Text.Trim());
                    cmdNV.Parameters.AddWithValue("@Chucdanh", cboChucVu.Text.Trim());
                    cmdNV.Parameters.AddWithValue("@MaKhoa", maKhoaSelected);
                    cmdNV.Parameters.AddWithValue("@MaNV", maNV);

                    int rowsNV = cmdNV.ExecuteNonQuery();

                    if (rowsNV == 0)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Không tìm thấy nhân viên có Mã số " + maNV + " trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // BƯỚC B: Cập nhật bảng TAIKHOAN
                    string queryUpdateTK = @"
                        UPDATE TAIKHOAN 
                        SET Password = @Password, Role = @Role 
                        WHERE MaNV = @MaNV";

                    SqlCommand cmdTK = new SqlCommand(queryUpdateTK, conn, transaction);
                    cmdTK.Parameters.AddWithValue("@Password", txtMatKhau.Text.Trim());
                    cmdTK.Parameters.AddWithValue("@Role", quyenHantext);
                    cmdTK.Parameters.AddWithValue("@MaNV", maNV);

                    cmdTK.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Cập nhật thông tin nhân viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    XoaTrongCacO();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNV.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã nhân viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maNV = Convert.ToInt32(txtMaNV.Text.Trim());

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa nhân viên có Mã số {maNV} cùng toàn bộ tài khoản liên quan không?\nHành động này không thể hoàn tác!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        string queryDeleteTK = "DELETE FROM TAIKHOAN WHERE MaNV = @MaNV";
                        SqlCommand cmdTK = new SqlCommand(queryDeleteTK, conn, transaction);
                        cmdTK.Parameters.AddWithValue("@MaNV", maNV);
                        cmdTK.ExecuteNonQuery();

                        string queryDeleteNV = "DELETE FROM NHANVIEN WHERE MaNV = @MaNV";
                        SqlCommand cmdNV = new SqlCommand(queryDeleteNV, conn, transaction);
                        cmdNV.Parameters.AddWithValue("@MaNV", maNV);

                        int rowsNV = cmdNV.ExecuteNonQuery();

                        if (rowsNV == 0)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Không tìm thấy nhân viên có Mã số " + maNV + " để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        transaction.Commit();
                        MessageBox.Show($"Đã xóa thành công nhân viên có Mã số {maNV} và tài khoản đi kèm!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        XoaTrongCacO();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Không thể xóa nhân viên này! Nguyên nhân có thể do nhân viên đang có dữ liệu liên quan ở các bảng Khám bệnh, Hóa đơn...\nChi tiết lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void cboChucVu_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
        


