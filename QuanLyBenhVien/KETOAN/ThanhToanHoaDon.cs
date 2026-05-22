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
using Xceed.Document.NET;
using Xceed.Words.NET;

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
        public static string ChuyenSoThanhChu(double number)
        {
            if (number == 0) return "Không đồng chẵn";
            if (number < 0) return "Âm " + ChuyenSoThanhChu(Math.Abs(number));

            string[] unitNumbers = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] placeValues = { "", "nghìn", "triệu", "tỷ", "nghìn tỷ", "triệu tỷ" };

            // Thuật toán làm tròn và bóc tách chuỗi số
            string sNumber = Math.Round(number).ToString();
            string res = "";
            int len = sNumber.Length;
            int pool = (len + 2) / 3;

            
            return "Hệ thống tự động dịch số tiền sang chữ";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            // 1. KIỂM TRA ĐIỀU KIỆN ĐẦU VÀO
            if (!int.TryParse(textBox1.Text, out int maKB))
            {
                MessageBox.Show("Vui lòng nhập Mã Khám Bệnh hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double tongChiPhi = 0, tienTamUng = 0, thucThuCuoiCung = 0;

            // SỬA LỖI: Đọc đúng hộp TextBox (textBox4 là Tổng chi phí từ button1)
            if (!string.IsNullOrEmpty(textBox4.Text)) double.TryParse(textBox4.Text.Replace(".", ""), out tongChiPhi);
            if (!string.IsNullOrEmpty(textBox8.Text)) double.TryParse(textBox8.Text.Replace(".", ""), out tienTamUng);
            if (!string.IsNullOrEmpty(textBox9.Text)) double.TryParse(textBox9.Text.Replace(".", ""), out thucThuCuoiCung);

            if (tongChiPhi == 0)
            {
                MessageBox.Show("Không có chi phí nào cần tất toán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maNhanVienThu = int.Parse(manv);
            int newMaPhieu = 0;
            string ngayLapPhieu = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            // 2. XỬ LÝ LƯU CƠ SỞ DỮ LIỆU (DATABASE)
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string loaiPhieu = thucThuCuoiCung >= 0 ? "Thanh toán dịch vụ" : "Hoàn tiền";
                    double soTienLuuKho = Math.Abs(thucThuCuoiCung);

                    string queryInsert = @"
                INSERT INTO PHIEUTHU (MaKB, LoaiPhieu, SoTien, NgayLap, NguoiThu) 
                VALUES (@MaKB, @LoaiPhieu, @SoTien, GETDATE(), @NguoiThu);
                SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdInsert = new SqlCommand(queryInsert, conn, transaction);
                    cmdInsert.Parameters.AddWithValue("@MaKB", maKB);
                    cmdInsert.Parameters.AddWithValue("@LoaiPhieu", loaiPhieu);
                    cmdInsert.Parameters.AddWithValue("@SoTien", soTienLuuKho);
                    cmdInsert.Parameters.AddWithValue("@NguoiThu", maNhanVienThu);

                    newMaPhieu = Convert.ToInt32(cmdInsert.ExecuteScalar());

                    string queryUpdate = @"
                UPDATE KHAMBENH_DICHVU 
                SET TrangThai = N'Đã thanh toán'
                WHERE MaKB = @MaKB AND TrangThai = N'Chưa thanh toán'";

                    SqlCommand cmdUpdate = new SqlCommand(queryUpdate, conn, transaction);
                    cmdUpdate.Parameters.AddWithValue("@MaKB", maKB);
                    
                    cmdUpdate.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi hệ thống CSDL: " + ex.Message, "Thao tác thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // 3. XỬ LÝ XUẤT FILE WORD VÀ IN ẤN (AN TOÀN TUYỆT ĐỐI)
            try
            {
                string templatePath = @"C:\Users\admin\Documents\Project_C#\MauKetQua\PhieuThu.docx";
                string exportPath = $@"C:\Users\admin\Documents\Project_C#\KetQuaPhieuIn\BienLai_{newMaPhieu}.docx";

                System.IO.File.Copy(templatePath, exportPath, true);

                using (DocX document = DocX.Load(exportPath))
                {
                    // Thay thế thông tin hành chính đơn giản
                    document.ReplaceText("[MaBienLai]", newMaPhieu.ToString());
                    document.ReplaceText("[NgayLap]", ngayLapPhieu);
                    document.ReplaceText("[MaKB]", textBox1.Text);
                    document.ReplaceText("[HotenBN]", textBox2.Text);
                    document.ReplaceText("[MaBN]", textBox3.Text);

                    // Thay thế thông tin tài chính
                    document.ReplaceText("[TongChiPhi]", tongChiPhi.ToString("N0") + " đ");
                    document.ReplaceText("[TienTamUng]", tienTamUng.ToString("N0") + " đ");

                    string textHienThiThucThu = thucThuCuoiCung >= 0 ? "Thu thêm: " : "Hoàn trả: ";
                    document.ReplaceText("[ThucThu]", textHienThiThucThu + Math.Abs(thucThuCuoiCung).ToString("N0") + " đ");

                    document.ReplaceText("[NguoiThu]", "Nhân viên mã số: " + manv);

                    // XỬ LÝ ĐỔ CHI TIẾT BẢNG DỊCH VỤ VÀO WORD
                    if (dataGridView1.Rows.Count > 0 && document.Tables.Count > 0)
                    {
                        Xceed.Document.NET.Table tableInWord = document.Tables[0];
                        int stt = 1;

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.IsNewRow) continue;

                            // SỬA LỖI: Kiểm tra null an toàn trước khi ép kiểu tránh gây lỗi văng khối catch
                            string tenDV = row.Cells[1].Value?.ToString() ?? "Dịch vụ không rõ";

                            int soLuong = 1;
                            if (row.Cells[2].Value != null) int.TryParse(row.Cells[2].Value.ToString(), out soLuong);

                            double donGia = 0;
                            if (row.Cells[3].Value != null) double.TryParse(row.Cells[3].Value.ToString(), out donGia);

                            double thanhTien = soLuong * donGia;

                            Row newRow = tableInWord.InsertRow();
                            newRow.Cells[0].Paragraphs[0].Append(stt.ToString());
                            newRow.Cells[1].Paragraphs[0].Append(tenDV);
                            newRow.Cells[2].Paragraphs[0].Append(soLuong.ToString());
                            newRow.Cells[3].Paragraphs[0].Append(donGia.ToString("N0"));
 
                            stt++;
                        }
                    }

                    document.Save();
                }

                // 4. HIỂN THỊ THÀNH CÔNG VÀ LÀM SẠCH MÀN HÌNH MÁY TÍNH
                MessageBox.Show($"Thanh toán thành công! Mã hóa đơn: {newMaPhieu}.\nĐang khởi động file in hóa đơn...",
                                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                System.Diagnostics.Process.Start(exportPath);

                // Làm sạch toàn bộ các TextBox đúng thứ tự cấu hình của bạn
                dataGridView1.DataSource = null;
                textBox2.Clear(); textBox3.Clear(); textBox4.Clear(); textBox6.Clear();
                textBox7.Text = "0"; textBox8.Clear(); textBox9.Clear(); textBox10.Text = "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thanh toán thành công trên CSDL nhưng gặp lỗi khi xuất file Word: " + ex.Message,
                                "Cảnh báo in ấn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string queryGrid = @"
                               SELECT DV.MaDV, DV.TenDV, 
                               KBDV.SoLuong, KBDV.DonGiaBan, 
                               KBDV.TrangThai 
                               FROM KHAMBENH_DICHVU KBDV 
                               INNER JOIN DICHVU DV ON KBDV.MaDV = DV.MaDV 
                               WHERE KBDV.MaKB = @MaKB and TrangThai = N'Chưa thanh toán'";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(queryGrid, conn);
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
            string queryInfo = @"
                                SELECT 
                                BN.MaBN, 
                                BN.HotenBN, 
                                ISNULL(bh.MucHuong, 0) AS MucHuong,
                                ISNULL(DV.TongTienDV, 0) AS TongTienDichVu,
                                ISNULL(
                                        CASE 
                                            WHEN DATEDIFF(day, KB.NgayVao, GETDATE()) = 0 THEN 1
                                            ELSE DATEDIFF(day, KB.NgayVao, GETDATE())
                                        END * PB.GiaNgay, 0) AS TongTienPhong,
                                ISNULL(TU.TongTamUng, 0) AS TongTienTamUng

                                FROM BENHNHAN BN
                                INNER JOIN KHAMBENH KB ON BN.MaBN = KB.MaBN
                                LEFT JOIN BHYT bh ON BN.MaBN = bh.MaBN
                                LEFT JOIN GIUONGBENH GB ON KB.MaGiuong = GB.MaGiuong
                                LEFT JOIN PHONGBENH PB ON GB.MaPhongBenh = PB.MaPhongBenh
    
                                 -- Truy vấn con 1: Tính tổng tiền dịch vụ chưa trả
                                LEFT JOIN (
                                SELECT MaKB, SUM(DonGiaBan * SoLuong) AS TongTienDV
                                FROM KHAMBENH_DICHVU
                                WHERE TrangThai = N'Chưa thanh toán'
                                GROUP BY MaKB) DV ON KB.MaKB = DV.MaKB

                                -- Truy vấn con 2: Tính tổng tiền tạm ứng
                                LEFT JOIN (
                                SELECT MaKB, SUM(SoTien) AS TongTamUng
                                FROM PHIEUTHU
                                WHERE LoaiPhieu = N'Tạm ứng'
                                GROUP BY MaKB) TU ON KB.MaKB = TU.MaKB
                                WHERE KB.MaKB = @MaKB";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(queryInfo, conn);
                cmd.Parameters.AddWithValue("@MaKB", textBox1.Text);
                try
                {
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            textBox2.Text = dr["HotenBN"].ToString();
                            textBox3.Text = dr["MaBN"].ToString();

                            double mucHuong = Convert.ToDouble(dr["MucHuong"]);
                            textBox6.Text = mucHuong.ToString();

                            double tongTienDichVu = Convert.ToDouble(dr["TongTienDichVu"]);
                            double tongTienPhong = Convert.ToDouble(dr["TongTienPhong"]);
                            double tongTienTamUng = Convert.ToDouble(dr["TongTienTamUng"]);

                            // Tính toán tài chính cuối cùng
                            double tongChiPhiGoc = tongTienDichVu + tongTienPhong;
                            double tongBenhNhanPhaiTra = tongChiPhiGoc * (1 - mucHuong);
                            double thucThuCuoiCung = tongBenhNhanPhaiTra - tongTienTamUng;

                            // Đổ lên giao diện TextBox của bạn
                            textBox4.Text = tongChiPhiGoc.ToString("N0");      // Tổng chi phí (Dịch vụ + Tiền phòng)
                            textBox8.Text = tongTienTamUng.ToString("N0");     // Tiền bệnh nhân đã tạm ứng trước đó
                            textBox9.Text = thucThuCuoiCung.ToString("N0");     // Số tiền thực thu (Dương: thu thêm, Âm: trả lại)
                            textBox7.Text = tongChiPhiGoc.ToString();
                            if (thucThuCuoiCung < 0)
                            {
                                textBox10.Text = "Số tiền nhà trường/bệnh viện phải trả lại:";
                                textBox7.ForeColor = Color.Green;
                            }
                            else
                            {
                                textBox10.Text = "Số tiền bệnh nhân cần nộp thêm:";
                                textBox7.ForeColor = Color.Red;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xử lý tính toán nội trú: " + ex.Message);
                }
            }

        }
        private void TinhTongTienDichVu_SQL(int maKB)
        {
        
            string query = @"
        SELECT SUM(DonGiaBan) AS TongTien 
        FROM KHAMBENH_DICHVU 
        WHERE MaKB = @MaKB and TrangThai = N'Chưa thanh toán'";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKB", maKB);

                try
                {
                    conn.Open();
                    
                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        double tongTien = Convert.ToDouble(result);

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
