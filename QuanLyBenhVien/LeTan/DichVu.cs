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

    public partial class DichVu : Form
    {
        string str = @"Data Source=DESKTOP-L182KS3\SQLEXPRESS;Initial Catalog=QuanLyBenhVien;Integrated Security=True";
        public string MaKB { get; set; }
        public class DichVuItem
        {
            public string MaDV { get; set; }
            public string TenDV { get; set; }
            public override string ToString()
            {
                return TenDV;
            }
        }
        public DichVu()
        {
            InitializeComponent();
        }
        private void DichVu_Load(object sender, EventArgs e)
        {
            textBox2.Text = MaKB;
            LoadAllDichVu();
        }

        private void LoadAllDichVu()
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                string query = "Select MaDV, TenDV from DICHVU";
                SqlDataAdapter adt = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adt.Fill(dt);

                checkedListBox1.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    checkedListBox1.Items.Add(new DichVuItem
                    {
                        MaDV = row["MaDV"].ToString(),
                        TenDV = row["TenDV"].ToString()
                    });
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<DichVuItem> dsChuyen = new List<DichVuItem>();
            foreach (DichVuItem item in checkedListBox1.CheckedItems)
            {
                dsChuyen.Add(item);
            }

            foreach (DichVuItem item in dsChuyen)
            {
                checkedListBox2.Items.Add(item, true); 
                checkedListBox1.Items.Remove(item);    
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<DichVuItem> dsHoanTac = new List<DichVuItem>();
            foreach (DichVuItem item in checkedListBox2.CheckedItems)
            {
                dsHoanTac.Add(item);
            }

            foreach (DichVuItem item in dsHoanTac)
            {
                checkedListBox1.Items.Add(item); 
                checkedListBox2.Items.Remove(item); 
            }
        }
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                string query = "Select * from DICHVU where TenDV like @dv";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@dv", "%" + textBox1.Text.Trim() + "%");

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (checkedListBox2.Items.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một dịch vụ trước khi lưu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBox2.Text.Trim(), out int maKB))
            {
                MessageBox.Show("Mã khám bệnh không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();

                
                string query = @"
                                  INSERT INTO KHAMBENH_DICHVU (MaKB, MaDV, LoaiChiTra, DonGiaBan)
                                  SELECT @kb, @dv, N'Tự nguyện', DonGia
                                  FROM DICHVU
                                  WHERE MaDV = @dv";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@kb", maKB);
                    cmd.Parameters.Add("@dv", SqlDbType.Int);

                    foreach (DichVuItem item in checkedListBox2.Items)
                    {
                        cmd.Parameters["@dv"].Value = item.MaDV;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            MessageBox.Show("Lưu dịch vụ yêu cầu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
