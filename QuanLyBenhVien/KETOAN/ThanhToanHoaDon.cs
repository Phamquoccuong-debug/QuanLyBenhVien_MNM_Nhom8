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

namespace QuanLyBenhVien
{

    public partial class ThanhToanHoaDon : UserControl
    {
        string str = @"Data Source=DESKTOP-L182KS3\SQLEXPRESS;Initial Catalog=QuanLyBenhVien;Integrated Security=True";
        public string manv { get; set; }
        public ThanhToanHoaDon()
        {
            InitializeComponent();
        }

        private void ThanhToanHoaDon_Load(object sender, EventArgs e)
        {
            textBox5.Text = manv;
        }

        private void button3_Click(object sender, EventArgs e)
        {


            if (!int.TryParse(textBox1.Text, out int maKB))
            {
                MessageBox.Show("Vui lòng nhập Mã Khám Bệnh hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            double mucHuong = 0;
            if (textBox6.Text != null)
            {
                double.TryParse(textBox6.Text, out mucHuong);

            }

            int maNhanVienThu = int.Parse(manv);

            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction(); // Bắt đầu Transaction an toàn

                try
                {
                    string queryCalc = @"
                SELECT 
                    COUNT(*) AS SoLuongDV,
                    ISNULL(SUM(DonGiaBan * SoLuong), 0) AS TongTienDichVu,
                    ISNULL(SUM(
                        CASE 
                            -- Nếu là dịch vụ BHYT -> Bệnh nhân chỉ trả phần chênh lệch (1 - Mức hưởng)
                            WHEN LoaiChiTra = N'BHYT' THEN (DonGiaBan * SoLuong) * (1 - @MucHuong)
                            -- Nếu là Tự nguyện -> Bệnh nhân trả 100%
                            ELSE (DonGiaBan * SoLuong)
                        END
                    ), 0) AS TongTienPhaiTra
                FROM KHAMBENH_DICHVU
                WHERE MaKB = @MaKB AND TrangThai = N'Chưa thanh toán';";

                    SqlCommand cmdCalc = new SqlCommand(queryCalc, conn, transaction);
                    cmdCalc.Parameters.AddWithValue("@MaKB", maKB);
                    cmdCalc.Parameters.AddWithValue("@MucHuong", mucHuong);

                    int soLuongDV = 0;
                    double tongTienDichVu = 0;
                    double tongTienPhaiTra = 0;

                    using (SqlDataReader reader = cmdCalc.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            soLuongDV = Convert.ToInt32(reader["SoLuongDV"]);
                            tongTienDichVu = Convert.ToDouble(reader["TongTienDichVu"]);
                            tongTienPhaiTra = Convert.ToDouble(reader["TongTienPhaiTra"]);
                        }
                    }
                    if (soLuongDV == 0)
                    {
                        MessageBox.Show("Bệnh nhân này không có dịch vụ nào cần thanh toán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        transaction.Rollback();
                        return;
                    }


                    textBox4.Text = tongTienDichVu.ToString("N0");
                    textBox7.Text = tongTienPhaiTra.ToString("N0");


                    string queryInsert = @"
                INSERT INTO PHIEUTHU (MaKB, LoaiPhieu, SoTien, NgayLap, NguoiThu) 
                VALUES (@MaKB, N'Thanh toán dịch vụ', @SoTienPhaiTra, GETDATE(), @NguoiThu);
                SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdInsert = new SqlCommand(queryInsert, conn, transaction);
                    cmdInsert.Parameters.AddWithValue("@MaKB", maKB);
                    cmdInsert.Parameters.AddWithValue("@SoTienPhaiTra", tongTienPhaiTra);
                    cmdInsert.Parameters.AddWithValue("@NguoiThu", maNhanVienThu);

                    int newMaPhieu = Convert.ToInt32(cmdInsert.ExecuteScalar());


                    string queryUpdate = @"
                UPDATE KHAMBENH_DICHVU 
                SET TrangThai = N'Đã thanh toán', MaPhieu = @MaPhieu 
                WHERE MaKB = @MaKB AND TrangThai = N'Chưa thanh toán'";

                    SqlCommand cmdUpdate = new SqlCommand(queryUpdate, conn, transaction);
                    cmdUpdate.Parameters.AddWithValue("@MaKB", maKB);
                    cmdUpdate.Parameters.AddWithValue("@MaPhieu", newMaPhieu);

                    cmdUpdate.ExecuteNonQuery();

                    transaction.Commit();

                    MessageBox.Show($"Thanh toán thành công! Mã biên lai: {newMaPhieu}\nSố tiền đã thu: {tongTienPhaiTra:N0} VNĐ", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Có lỗi thì hủy toàn bộ thao tác
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string query = "SELECT DV.MaDV,DV.TenDV,KBDV.SoLuong, KBDV.DonGiaBan,KBDV.TrangThai FROM KHAMBENH_DICHVU KBDV INNER JOIN DICHVU DV ON KBDV.MaDV = DV.MaDV WHERE KBDV.MaKB = @MaKB ";
            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKB", textBox1.Text);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                try
                {
                    conn.Open();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải danh sách dịch vụ: " + ex.Message);
                }
            }
            TinhTongTienDichVu_SQL(int.Parse(textBox1.Text));
        }
        private void TinhTongTienDichVu_SQL(int maKB)
        {
            // Câu lệnh SQL sử dụng hàm SUM để tính tổng thành tiền
            string query = @"
        SELECT SUM(DonGiaBan) AS TongTien 
        FROM KHAMBENH_DICHVU 
        WHERE MaKB = @MaKB";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKB", maKB);

                try
                {
                    conn.Open();
                    // Sử dụng ExecuteScalar vì câu lệnh chỉ trả về đúng 1 ô dữ liệu duy nhất
                    object result = cmd.ExecuteScalar();

                    // Kiểm tra xem kết quả có bị NULL không (Trường hợp lượt khám chưa chọn dịch vụ nào)
                    if (result != DBNull.Value && result != null)
                    {
                        double tongTien = Convert.ToDouble(result);

                        // Đổ dữ liệu vào TextBox kèm định dạng dấu chấm phân cách hàng nghìn cho đẹp (Ví dụ: 350.000)
                        textBox4.Text = tongTien.ToString("N0");
                    }
                    else
                    {
                        textBox4.Text = "0";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tính tổng tiền: " + ex.Message);
                }
            }
        }
    }
}
