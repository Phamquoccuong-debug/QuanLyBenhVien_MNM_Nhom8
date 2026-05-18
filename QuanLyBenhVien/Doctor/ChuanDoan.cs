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
    public partial class ChuanDoan : UserControl
    {
        string connectionString = "Data Source=TenMayCuaBan;Initial Catalog=QuanLyBenhVien;Integrated Security=True";
        public string manv {  get; set; }
        public ChuanDoan()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "update KHAMBENH set TrieuChung = @tc, ChuanDoan = @cd where MaKB = @makb";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@tc",textBox6.Text);
                cmd.Parameters.AddWithValue("@cd",textBox7.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Lưu thành công","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);

            }
        }

        private void button4_Click(object sender, EventArgs e)
        { 
            // Câu lệnh SQL lấy dữ liệu
            string query = @"SELECT BN.HotenBN, BN.MaBN, DV.TenDV, KQ.KetQua
        FROM KETQUA_LAMSANG KQ
        INNER JOIN KHAMBENH KB ON KQ.MaKB = KB.MaKB
        INNER JOIN BENHNHAN BN ON KB.MaBN = BN.MaBN
        INNER JOIN DICHVU DV ON KQ.MaDV = DV.MaDV
        WHERE KQ.MaKetQua = @MaKetQua ,MaKB = @makb";
            string query2 = "Select TrieuChung, ChuanDoan from KHAMBENH where MaKB = @makb";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKetQua", textBox1.Text);
                cmd.Parameters.AddWithValue("@makb",textBox8.Text);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) 
                        {
                            textBox2.Text = reader["HotenBN"].ToString();
                            textBox3.Text = reader["MaBN"].ToString();
                            textBox4.Text = reader["TenDV"].ToString();

                            
                            textBox5.Text = reader["KetQua"] != DBNull.Value ? reader["KetQua"].ToString() : "";
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thông tin cho mã kết quả này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@makb",textBox8.Text);
                using(SqlDataReader reader = cmd2.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        textBox6.Text = reader["TrieuChung"] != DBNull.Value ? reader["TrieuChung"].ToString() : "";
                        textBox7.Text = reader["ChuanDoan"] != DBNull.Value ? reader["ChuanDoan"].ToString() : "";
                    }
                }
            }
        }

        private void ChuanDoan_Load(object sender, EventArgs e)
        {
            textBox9.Text = manv;
        }
    }
}
