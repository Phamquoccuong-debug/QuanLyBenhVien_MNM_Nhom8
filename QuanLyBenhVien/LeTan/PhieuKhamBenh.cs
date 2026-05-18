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
    public partial class PhieuKhamBenh : Form
    {
        string str = @"Data Source=DESKTOP-L182KS3\SQLEXPRESS;Initial Catalog=QuanLyBenhVien;Integrated Security=True";
        public string mabn {  get; set; }
        public static string makb {  get; set; }
        public string bhyt {  get; set; }
        public PhieuKhamBenh()
        {
            InitializeComponent();
        }

        private void PhieuKhamBenh_Load(object sender, EventArgs e)
        {
            txtCCCD.Text = mabn;
            txtBHYT.Text = bhyt;
            using(SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                string query = "Select * from KHOA";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = "TenKhoa";
                comboBox2.ValueMember = "TenKhoa";
                

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using(SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                string query = "insert into KHAMBENH(MaBN,MaBHYT,NgayVao) " +
                    "values (@mabn, @bhyt,@nv)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@mabn",txtCCCD.Text);
                cmd.Parameters.AddWithValue("@bhyt", txtBHYT.Text);
                cmd.Parameters.AddWithValue("@nv", dateTimePicker1.Text);
                cmd.ExecuteNonQuery();

                string query2 = "select MaKB from KHAMBENH where MaBN = @mabn and NgayVao = @nv";
                SqlCommand cmd2 = new SqlCommand( query2, conn);
                cmd2.Parameters.AddWithValue("@mabn",txtCCCD.Text);
                cmd2.Parameters.AddWithValue("@nv", dateTimePicker1.Text);
                using(SqlDataReader reader = cmd2.ExecuteReader())
                {
                    if (reader.Read())
                    {
                         makb = reader["MaKB"].ToString();

                    }
                }
                
            }

            DichVu form = new DichVu();
            form.MaKB = makb;
            form.ShowDialog();
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            
            if (comboBox1.SelectedIndex == -1) return;

            string maKhoaDaChon = comboBox2.SelectedValue.ToString();

           
            string query = "SELECT MaNV, HotenNV FROM NHANVIEN WHERE Role = N'Bác sĩ' AND MaKhoa = @MaKhoa";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKhoa", maKhoaDaChon);

                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dtBacSi = new DataTable();
                    da.Fill(dtBacSi);
                    comboBox1.DataSource = dtBacSi;
                    comboBox1.DisplayMember = "HotenNV";
                    comboBox1.ValueMember = "MaNV";     

                   
                    comboBox2.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải danh sách bác sĩ: " + ex.Message);
                }
            }
        }
    
    }
}
