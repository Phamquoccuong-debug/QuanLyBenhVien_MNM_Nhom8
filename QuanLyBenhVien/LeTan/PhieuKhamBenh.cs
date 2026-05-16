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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using(SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                string query = "insert into KHAMBENH(MaBN,MaBHYT,NgayVao) " +
                    "values (MaBN = @mabn, MaBHYT=@bhyt,NgayVao = @nv)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@mabn",txtCCCD.Text);
                cmd.Parameters.AddWithValue("@bhyt", txtBHYT);
                cmd.Parameters.AddWithValue("@nv", dateTimePicker1.Text);
                cmd.ExecuteNonQuery();

                string query2 = "select MaKB from KHAMBENH where MaBN = @mabn, NgayVao = @nv";
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
    }
}
