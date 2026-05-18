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
    public partial class NhapVien : UserControl
    {
        string str = @"Data Source=DESKTOP-L182KS3\SQLEXPRESS;Initial Catalog=QuanLyBenhVien;Integrated Security=True";
        public NhapVien()
        {
            InitializeComponent();
        }

        private void NhapVien_Load(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn một khoa!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(str))
            {
                try
                {
                    conn.Open();
                    
                    string query = "SELECT * " +
                                   "FROM PHONGBENH WHERE MaKhoa = @makhoa";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        
                        cmd.Parameters.AddWithValue("@makhoa", comboBox1.SelectedValue.ToString());

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("Khoa này hiện chưa có phòng bệnh nào!");
                            dataGridView2.DataSource = null; 
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải danh sách phòng: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          
            // Kiểm tra xem người dùng có click hợp lệ vào dòng dữ liệu hay không (tránh click vào tiêu đề cột e.RowIndex = -1)
            if (e.RowIndex >= 0)
            { 
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                
                if (row.Cells[0].Value != null)
                {
                    string maPhongBenh = row.Cells[0].Value.ToString();
                    textBox1.Text = maPhongBenh;

                    // 3. Tiến hành truy vấn lấy danh sách giường trống dựa vào maPhongBenh vừa lấy được
                    LoadGiuongTrong(maPhongBenh);
                }
            }
        
        }

        // Hàm phụ để tải giường bệnh lên dataGridView2
        private void LoadGiuongTrong(string maPhong)
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * " +
                                   "FROM GIUONGBENH " +
                                   "WHERE MaPhongBenh = @mapb AND TrangThai = N'Trống'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@mapb", maPhong);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Đổ dữ liệu vào dataGridView2 (Danh sách giường trống)
                        dataGridView2.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải danh sách giường: " + ex.Message);
                }
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];


                if (row.Cells[0].Value != null)
                {
                    maskedTextBox1.Text = row.Cells[0].Value.ToString();
                    maskedTextBox2.Text = row.Cells[1].Value.ToString();


                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    
}
