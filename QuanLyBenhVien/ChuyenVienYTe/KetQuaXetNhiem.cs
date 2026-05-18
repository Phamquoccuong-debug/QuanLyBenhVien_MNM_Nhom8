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

namespace QuanLyBenhVien.ChuyenVienYTe
{
    public partial class KetQuaXetNhiem : Form
    {
        string str = @"Data Source=DESKTOP-L182KS3\SQLEXPRESS;Initial Catalog=QuanLyBenhVien;Integrated Security=True";
        public string manv;
        public string tennv;
        public KetQuaXetNhiem()
        {
            InitializeComponent();
        }

        private void KetQuaXetNhiem_Load(object sender, EventArgs e)
        {
            textBox2.Text = manv;
            textBox3.Text = tennv;
            using(SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                string query = "select MaDV, TenDV from DICHVU";
                SqlCommand sqlCommand = new SqlCommand(query, conn);
                DataTable dt = new DataTable();
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "TenDV";
                comboBox1.ValueMember = "MaDV";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using(SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                string query = "insert into KETQUA_LAMSANG(MaKB,MaDV,NgayThucHien,KetQua,NguoiThucHien) " +
                    "values(@makb,@madv,GETDATE(),@kq,@ngth)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@makb", textBox4.Text);
                cmd.Parameters.AddWithValue("@madv", comboBox1.SelectedValue);
                cmd.Parameters.AddWithValue("@kq",textBox1.Text);
                cmd.Parameters.AddWithValue("ngth",textBox3.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Tạo thành công","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Bạn có muốn thoát","Thông báo",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            textBox4.Clear();
            textBox6.Clear();
            textBox5.Clear();
        }
    }
}
