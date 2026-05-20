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
using System.IO;

namespace QuanLyBenhVien
{
    public partial class KeDonThuoc : Form
    {
        public string hoten {  get; set; }
        public string makb {  get; set; }
        public string tc {  get; set; }
        public string chuandoan {  get; set; }
        string str = @"Data Source=DESKTOP-L182KS3\SQLEXPRESS;Initial Catalog=QuanLyBenhVien;Integrated Security=True";
        public KeDonThuoc()
        {
            InitializeComponent();
        }

        private void KeDonThuoc_Load(object sender, EventArgs e)
        {
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            textBox1.Text = makb;
            textBox2.Text = hoten;
            textBox6.Text = tc;
            textBox7.Text = chuandoan;
            using(SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                string query = "select MaThuoc, TenThuoc from THUOC";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "TenThuoc";
                comboBox1.ValueMember = "MaThuoc";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            int maThuocSelected = (int)comboBox1.SelectedValue;
            int soLuongKe = Convert.ToInt32(textBox3.Text);

            string queryCheck = "SELECT SoLuongTon, GiaHienTai FROM THUOC WHERE MaThuoc = @mathuoc";
            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(queryCheck, conn);
                cmd.Parameters.AddWithValue("@mathuoc", maThuocSelected);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int tonKho = Convert.ToInt32(reader["SoLuongTon"]);
                        double donGia = Convert.ToDouble(reader["GiaHienTai"]);
                        if (tonKho < soLuongKe)
                        {
                            MessageBox.Show($"Không đủ thuốc trong kho! Hiện tại chỉ còn {tonKho} viên.",
                                            "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        double thanhTien = soLuongKe * donGia;
                        dataGridView1.Rows.Add(maThuocSelected, comboBox1.Text, soLuongKe, donGia, thanhTien, textBox4.Text);

                        
                        textBox3.Clear();
                        textBox4.Clear();
                        comboBox1.Focus(); 
                    }
                }
            
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Đơn thuốc chưa có loại thuốc nào!", "Thông báo");
                return;
            }

            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
               
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string queryDonThuoc = @"INSERT INTO DONTHUOC (MaKB) VALUES (@makb);
                                     SELECT SCOPE_IDENTITY();"; 

                    SqlCommand cmdDon = new SqlCommand(queryDonThuoc, conn, transaction);
                    cmdDon.Parameters.AddWithValue("@makb", Convert.ToInt32(textBox1.Text));
                    

                    int maDonThuocVuaTao = Convert.ToInt32(cmdDon.ExecuteScalar());

                    
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;

                        int maThuoc = Convert.ToInt32(row.Cells["MaThuoc"].Value);
                        int soLuong = Convert.ToInt32(row.Cells["SoLuong"].Value);
                        double donGia = Convert.ToDouble(row.Cells["DonGiaBan"].Value);
                        string cachDung = row.Cells["CachDung"].Value.ToString();

                        // HÀNH ĐỘNG 2: Insert vào bảng CHITIET_DONTHUOC
                        string queryChiTiet = @"INSERT INTO CT_DONTHUOC (MaDonThuoc, MaThuoc, SoLuong, DonGiaBan, CachDung) 
                                        VALUES (@madon, @mathuoc, @soluong, @dongia, @cachdung)";
                        SqlCommand cmdCT = new SqlCommand(queryChiTiet, conn, transaction);
                        cmdCT.Parameters.AddWithValue("@madon", maDonThuocVuaTao);
                        cmdCT.Parameters.AddWithValue("@mathuoc", maThuoc);
                        cmdCT.Parameters.AddWithValue("@soluong", soLuong);
                        cmdCT.Parameters.AddWithValue("@dongia", donGia);
                        cmdCT.Parameters.AddWithValue("@cachdung", cachDung);
                        cmdCT.ExecuteNonQuery();

                        
                        string queryTruKho = "UPDATE THUOC SET SoLuongTon = SoLuongTon - @soluong WHERE MaThuoc = @mathuoc";
                        SqlCommand cmdKho = new SqlCommand(queryTruKho, conn, transaction);
                        cmdKho.Parameters.AddWithValue("@soluong", soLuong);
                        cmdKho.Parameters.AddWithValue("@mathuoc", maThuoc);
                        cmdKho.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Kê đơn thuốc thành công! Hệ thống đã tự động trừ kho dược.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    
                    XuatDonThuocRaWord(maDonThuocVuaTao);
                }
                catch (Exception ex)
                {
                    
                    transaction.Rollback();
                    MessageBox.Show("Lỗi lưu đơn thuốc: " + ex.Message, "Thao tác thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void XuatDonThuocRaWord(int maDonThuoc)
        {
            string templatePath = @"C:\Users\admin\Documents\Project_C#\MauKetQua\DonThuoc.docx";
            string exportPath = $@"C:\Users\admin\Documents\Project_C#\KetQuaPhieuIn\DonThuoc_{maDonThuoc}.docx";

            File.Copy(templatePath, exportPath, true);

            using (DocX document = DocX.Load(exportPath))
            {
                // 1. Thay thế các trường thông tin hành chính đơn giản
                document.ReplaceText("[HotenBN]", textBox2.Text);
                document.ReplaceText("[MaKB]", textBox1.Text);
                document.ReplaceText("[NgayLap]", dateTimePicker1.Value.ToString());

                // 2. Tìm cái Bảng đầu tiên trong file Word để đổ danh sách thuốc vào
                Xceed.Document.NET.Table table = document.Tables[0];

                // Lấy dữ liệu chi tiết đơn thuốc từ SQL lên
                string query = @"SELECT KT.TenThuoc, KT.DonViTinh, CT.SoLuong, CT.CachDung 
                         FROM CHITIET_DONTHUOC CT 
                         JOIN KHO_THUOC KT ON CT.MaThuoc = KT.MaThuoc 
                         WHERE CT.MaDonThuoc = @madon";

                using (SqlConnection conn = new SqlConnection(str))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@madon", maDonThuoc);
                    conn.Open();
                    int stt = 1;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Tạo một dòng mới trong bảng Word
                            Row newRow = table.InsertRow();
                            newRow.Cells[0].Paragraphs[0].Append(stt.ToString());
                            newRow.Cells[1].Paragraphs[0].Append(reader["TenThuoc"].ToString());
                            newRow.Cells[2].Paragraphs[0].Append(reader["SoLuong"].ToString() + " " + reader["DonViTinh"].ToString());
                            newRow.Cells[3].Paragraphs[0].Append(reader["CachDung"].ToString());

                            stt++;
                        }
                    }
                }
                document.Save();
            }
            // Mở file word lên
            System.Diagnostics.Process.Start(exportPath);
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
    
}
