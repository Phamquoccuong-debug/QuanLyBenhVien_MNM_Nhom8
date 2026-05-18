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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyBenhVien
{
    public partial class DichVuChiDinh : UserControl
    {
        public string manv {  get; set; }
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
        public DichVuChiDinh()
        {
            InitializeComponent();
        }

        private void DichVuChiDinh_Load(object sender, EventArgs e)
        {
            LoadAllDichVu();
            textBox1.Text = manv;
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (checkedListBox2.Items.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một dịch vụ trước khi lưu!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();

                string query = "INSERT INTO KHAMBENH_DICHVU(MaKB, MaDV) VALUES (@kb, @dv)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Duyệt qua toàn bộ các dịch vụ có trong Box 2 (đã chốt chọn)
                    foreach (DichVuItem item in checkedListBox2.Items)
                    {
                        cmd.Parameters.AddWithValue("@kb", textBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@dv", item.MaDV);

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                }
            }
            MessageBox.Show("Lưu chỉ định dịch vụ thành công!");
        }
    }
}
