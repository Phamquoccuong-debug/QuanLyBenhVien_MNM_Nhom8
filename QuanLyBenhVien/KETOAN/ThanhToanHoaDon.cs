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
        public string manv {  get; set; }
        public ThanhToanHoaDon()
        {
            InitializeComponent();
        }

        private void ThanhToanHoaDon_Load(object sender, EventArgs e)
        {

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

            int maNhanVienThu = 1; // Giả sử ID của nhân viên đang đăng nhập là 1
            string connectionString = "Data Source=TenMayCuaBan;Initial Catalog=QuanLyBenhVien;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
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
    }
}
