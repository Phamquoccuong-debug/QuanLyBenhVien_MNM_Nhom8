using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xceed.Words.NET;

namespace QuanLyBenhVien
{
    public partial class ChuanDoan : UserControl
    {
        string str = @"Data Source=DESKTOP-L182KS3\SQLEXPRESS;Initial Catalog=QuanLyBenhVien;Integrated Security=True";
        public string manv {  get; set; }
        public ChuanDoan()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                string query = "update KHAMBENH set TrieuChung = @tc, ChanDoan = @cd where MaKB = @makb";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@tc",textBox6.Text);
                cmd.Parameters.AddWithValue("@cd",textBox7.Text);
                cmd.Parameters.AddWithValue("@makb",textBox8.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Lưu thành công","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            if (!int.TryParse(textBox1.Text.Trim(), out int maKetQua) ||
                !int.TryParse(textBox8.Text.Trim(), out int maKB))
            {
                MessageBox.Show("Vui lòng nhập Mã kết quả và Mã khám bệnh hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            string query = @"
        SELECT 
            BN.HotenBN, 
            BN.MaBN, 
            DV.TenDV, 
            KQ.KetQua,
            KB.TrieuChung,
            KB.ChanDoan
        FROM KETQUA_LAMSANG KQ
        INNER JOIN KHAMBENH KB ON KQ.MaKB = KB.MaKB
        INNER JOIN BENHNHAN BN ON KB.MaBN = BN.MaBN
        INNER JOIN DICHVU DV ON KQ.MaDV = DV.MaDV
        WHERE KQ.MaKetQua = @MaKetQua AND KB.MaKB = @makb";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKetQua", maKetQua);
                cmd.Parameters.AddWithValue("@makb", maKB);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Điền dữ liệu từ Query 1
                            textBox2.Text = reader["HotenBN"].ToString();
                            textBox3.Text = reader["MaBN"].ToString();
                            textBox4.Text = reader["TenDV"].ToString();
                            textBox5.Text = reader["KetQua"] != DBNull.Value ? reader["KetQua"].ToString() : "";

                            textBox6.Text = reader["TrieuChung"] != DBNull.Value ? reader["TrieuChung"].ToString() : "";
                            textBox7.Text = reader["ChanDoan"] != DBNull.Value ? reader["ChanDoan"].ToString() : "";
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thông tin cho mã này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            textBox2.Clear(); textBox3.Clear(); textBox4.Clear();
                            textBox5.Clear(); textBox6.Clear(); textBox7.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ChuanDoan_Load(object sender, EventArgs e)
        {
            textBox9.Text = manv;
            

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            KeDonThuoc form = new KeDonThuoc();
            form.makb = textBox8.Text;
            form.hoten = textBox3.Text;
            form.tc = textBox5.Text;
            form.chuandoan = textBox6.Text;
            this.Hide();
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra an toàn đầu vào trước khi kết nối CSDL
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã kết quả cần in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Đường dẫn file mẫu và file xuất ra
            string templatePath = @"C:\Users\admin\Documents\Project_C#\MauKetQua\KetQua_LamSang.docx";
            string exportFolder = @"C:\Users\admin\Documents\Project_C#\KetQuaPhieuIn";

            // Tạo thư mục nếu máy chưa có
            if (!Directory.Exists(exportFolder)) Directory.CreateDirectory(exportFolder);

            string tennv = "";

            
            using (SqlConnection conn = new SqlConnection(str))
            {
                string query1 = "select HotenNV from NHANVIEN where MaNV = @manv";
                SqlCommand cmd = new SqlCommand(query1, conn);
                cmd.Parameters.AddWithValue("@manv", manv);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                       
                        if (reader.Read())
                        {
                            tennv = reader["HotenNV"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi lấy thông tin nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Dừng lại nếu không lấy được thông tin nhân viên
                }
            }

            // 4. CÂU LỆNH SQL LẤY DỮ LIỆU BỆNH NHÂN ĐỂ IN
            string query = @"
                            SELECT BN.HotenBN, KB.MaKB, KB.ChanDoan, DV.TenDV, KQ.KetQua
                            FROM KETQUA_LAMSANG KQ
                            INNER JOIN KHAMBENH KB ON KQ.MaKB = KB.MaKB
                            INNER JOIN BENHNHAN BN ON KB.MaBN = BN.MaBN
                            INNER JOIN DICHVU DV ON KQ.MaDV = DV.MaDV
                            WHERE KQ.MaKetQua = @MaKetQua";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKetQua", textBox1.Text.Trim());

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // Nếu tìm thấy dữ liệu bệnh nhân
                        {
                            string hoTen = reader["HotenBN"].ToString();
                            string maKB = reader["MaKB"].ToString();
                            string chanDoan = reader["ChanDoan"] != DBNull.Value ? reader["ChanDoan"].ToString() : "Chưa có chẩn đoán";
                            string tenDV = reader["TenDV"].ToString();
                            string ketQua = reader["KetQua"] != DBNull.Value ? reader["KetQua"].ToString() : "Chưa có kết quả";

                            // Đường dẫn file Word mới sinh ra riêng cho bệnh nhân này
                            string fileXuatRa = Path.Combine(exportFolder, $"PhieuKQ_{maKB}_{hoTen}.docx");

                            // 5. TIẾN HÀNH ĐIỀN DATA VÀO FILE WORD
                            File.Copy(templatePath, fileXuatRa, true);

                            // Mở file mới lên để thay thế chữ
                            using (DocX document = DocX.Load(fileXuatRa))
                            {
                                document.ReplaceText("[HotenBN]", hoTen);
                                document.ReplaceText("[MaKB]", maKB);
                                document.ReplaceText("[ChanDoan]", chanDoan);
                                document.ReplaceText("[TenDV]", tenDV);
                                document.ReplaceText("[KetQua]", ketQua);
                                document.ReplaceText("[TenBacSi]", tennv); // Thay thế tên bác sĩ thực hiện dịch vụ thành công

                                document.Save();
                            }

                            MessageBox.Show("Đã xuất file Word kết quả thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            System.Diagnostics.Process.Start(fileXuatRa);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy dữ liệu cho mã kết quả này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi trong quá trình xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
