using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.Expressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyBenhVien
{
    public partial class ThongTin : UserControl
    {
        string str = @"Data Source=DESKTOP-L182KS3\SQLEXPRESS;Initial Catalog=QuanLyBenhVien;Integrated Security=True";

        public void ThemBenhNhan()
        {
            string TenBN = txtTenBN.Text.Trim();
            string CCCD = txtCCCD.Text.Trim();
            string GT = radioButton1.Checked ? "Nam" : "Nữ";
            string NS = dtNgaySinh.Text;
            string DC = txtDiachi.Text.Trim();
            string DT = txtDienThoai.Text.Trim();

            string TenNN = txtTenNN.Text.Trim();
            string QH = txtQH.Text.Trim();
            string dc = txtDC.Text.Trim();
            string dt = txtDT.Text.Trim();

            string BHYT = txtBHYT.Text.Trim();
            string NC = dtNgayCap.Text;
            string NHH = dtNgayHet.Text;
            string MH = comboBox1.Text;

            // Ràng buộc cơ bản: Không được để trống CCCD/MaBN
            if (string.IsNullOrEmpty(CCCD))
            {
                MessageBox.Show("Vui lòng nhập số CCCD/Mã bệnh nhân!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();

                string queryCheck = "SELECT COUNT(*) FROM BENHNHAN WHERE MaBN = @cccd";
                using (SqlCommand cmdCheck = new SqlCommand(queryCheck, conn))
                {
                    cmdCheck.Parameters.AddWithValue("@cccd", CCCD);
                    int count = Convert.ToInt32(cmdCheck.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show($"Bệnh nhân có số CCCD '{CCCD}' đã tồn tại trong hệ thống!", "Cảnh báo trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; 
                    }
                }

                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    
                    string query1 = "insert into BENHNHAN(MaBN, HotenBN, Ngaysinh, Gioitinh, Diachi, Dienthoai) " +
                                    "values (@cccd,@ten, @ns, @gt,@dc,@dt)";
                    SqlCommand cmd = new SqlCommand(query1, conn, transaction);
                    cmd.Parameters.AddWithValue("@cccd", CCCD);
                    cmd.Parameters.AddWithValue("@ten", TenBN);
                    cmd.Parameters.AddWithValue("@ns", NS);
                    cmd.Parameters.AddWithValue("@gt", GT);
                    cmd.Parameters.AddWithValue("@dc", DC);
                    cmd.Parameters.AddWithValue("@dt", DT);
                    cmd.ExecuteNonQuery();

                    // 2. Thêm thông tin người nhà (Đã sửa lỗi quên ExecuteNonQuery)
                    if (!String.IsNullOrEmpty(TenNN))
                    {
                        string query2 = "insert into NGUOINHA(HotenNN, QuanHe, Diachi, Dienthoai, MaBN) " +
                                        "values (@tennn,@qh,@dc,@dt,@cccd)";
                        SqlCommand cmd2 = new SqlCommand(query2, conn, transaction);
                        cmd2.Parameters.AddWithValue("@tennn", TenNN);
                        cmd2.Parameters.AddWithValue("@qh", QH);
                        cmd2.Parameters.AddWithValue("@dc", dc);
                        cmd2.Parameters.AddWithValue("@dt", dt);
                        cmd2.Parameters.AddWithValue("@cccd", CCCD);

                        cmd2.ExecuteNonQuery();
                    }

                    if (!String.IsNullOrEmpty(BHYT))
                    {
                        string query3 = "insert into BHYT(MaBHYT,NgayCap,NgayHetHan,MucHuong, MaBN) " +
                                        "values (@bh, @nc,@nhh,@mh, @cccd)";

                        SqlCommand cmd3 = new SqlCommand(query3, conn, transaction);
                        cmd3.Parameters.AddWithValue("@bh", BHYT);
                        cmd3.Parameters.AddWithValue("@nc", NC);
                        cmd3.Parameters.AddWithValue("@nhh", NHH);
                        cmd3.Parameters.AddWithValue("@mh", MH);
                        cmd3.Parameters.AddWithValue("@cccd", CCCD);
                        cmd3.ExecuteNonQuery();
                    }

                    
                    transaction.Commit();
                    MessageBox.Show("Thêm bệnh nhân thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    
                    transaction.Rollback();
                    MessageBox.Show("Đã xảy ra lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public ThongTin()
        {
            InitializeComponent();
        }

        private void ThongTin_Load(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public static class DuLieu
        {
            public static string MaBN { get; set; }
            public static string BHYT {  get; set; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DuLieu.BHYT = txtBHYT.Text;
            DuLieu.MaBN = txtCCCD.Text;
            PhieuKhamBenh form = new PhieuKhamBenh();
            form.mabn = txtCCCD.Text;
            form.bhyt = txtBHYT.Text;
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ThemBenhNhan();
        }

        private void btCNBN_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                string gt = radioButton1.Checked ? "Nam" : "Nữ";
                conn.Open();

                string query = "update BENHNHAN set HotenBN = @ten, Ngaysinh = @ns, Gioitinh = @gt, Diachi = @dc, Dienthoai = @dt where MaBN = @cccd";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@cccd", txtCCCD.Text);
                cmd.Parameters.AddWithValue("@ten", txtTenBN.Text);
                cmd.Parameters.AddWithValue("@ns", dtNgaySinh.Text);
                cmd.Parameters.AddWithValue("@gt", gt);
                cmd.Parameters.AddWithValue("@dc", txtDiachi.Text);
                cmd.Parameters.AddWithValue("@dt", txtDienThoai.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCNNN_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(txtTenNN.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập họ tên người nhà!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();

                // Sử dụng câu lệnh SQL tích hợp kiểm tra: IF EXISTS ... UPDATE ... ELSE ... INSERT
                string query = @"
            IF EXISTS (SELECT 1 FROM NGUOINHA WHERE MaBN = @cc)
            BEGIN
                -- Nếu đã tồn tại người nhà của MaBN này, tiến hành UPDATE
                UPDATE NGUOINHA 
                SET HotenNN = @tennn, QuanHe = @qh, Diachi = @dc, Dienthoai = @dt 
                WHERE MaBN = @cc
            END
            ELSE
            BEGIN
                -- Nếu chưa có, tiến hành INSERT mới
                INSERT INTO NGUOINHA (HotenNN, QuanHe, Diachi, Dienthoai, MaBN)
                VALUES (@tennn, @qh, @dc, @dt, @cc)
            END";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@tennn", txtTenNN.Text.Trim());
                cmd.Parameters.AddWithValue("@qh", txtQH.Text.Trim());
                cmd.Parameters.AddWithValue("@dc", txtDC.Text.Trim());
                cmd.Parameters.AddWithValue("@dt", txtDT.Text.Trim());
                cmd.Parameters.AddWithValue("@cc", txtCCCD.Text.Trim());

                
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Cập nhật thông tin người nhà thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật. Vui lòng kiểm tra lại mã bệnh nhân!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCNBHYT_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(txtBHYT.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập số thẻ BHYT!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                string query = @"
            IF EXISTS (SELECT 1 FROM BHYT WHERE MaBN = @cccd)
            BEGIN
                -- Đã tồn tại thông tin BHYT cho bệnh nhân này, tiến hành cập nhật
                UPDATE BHYT 
                SET MaBHYT = @bh, NgayCap = @nc, NgayHetHan = @nhh, MucHuong = @mh 
                WHERE MaBN = @cccd
            END
            ELSE
            BEGIN
                -- Bệnh nhân này trước đó chưa nhập BHYT, tiến hành thêm mới
                INSERT INTO BHYT (MaBHYT, NgayCap, NgayHetHan, MucHuong, MaBN)
                VALUES (@bh, @nc, @nhh, @mh, @cccd)
            END";

                SqlCommand cmd = new SqlCommand(query, conn);

                
                cmd.Parameters.AddWithValue("@bh", txtBHYT.Text.Trim());
                cmd.Parameters.AddWithValue("@nc", dtNgayCap.Text);
                cmd.Parameters.AddWithValue("@nhh", dtNgayHet.Text);
                cmd.Parameters.AddWithValue("@mh", comboBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@cccd", txtCCCD.Text.Trim());

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Cập nhật thông tin thẻ BHYT thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không thể thực hiện. Vui lòng kiểm tra lại mã bệnh nhân!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnTK_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                
                string query = @"
        SELECT 
            bn.MaBN AS [Mã BN (CCCD)],
            bn.HotenBN AS [Họ tên BN],
            bn.Ngaysinh AS [Ngày sinh],
            bn.Gioitinh AS [Giới tính],
            bn.Diachi AS [Địa chỉ BN],
            bn.Dienthoai AS [SĐT BN],
            
            bh.MaBHYT AS [Mã BHYT],
            bh.NgayCap AS [Ngày cấp BHYT],
            bh.NgayHetHan AS [Ngày hết hạn BHYT],
            bh.MucHuong AS [Mức hưởng %],
            
            nn.HotenNN AS [Họ tên Người nhà],
            nn.QuanHe AS [Quan hệ],
            nn.Diachi AS [Địa chỉ NN],
            nn.Dienthoai AS [SĐT NN]
        FROM BENHNHAN bn
        LEFT JOIN BHYT bh ON bn.MaBN = bh.MaBN
        LEFT JOIN NGUOINHA nn ON bn.MaBN = nn.MaBN
        WHERE bn.MaBN = @mabn";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@mabn", textBox10.Text.Trim());

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                
                txtCCCD.Text = row.Cells["Mã BN (CCCD)"].Value?.ToString() ?? "";
                txtTenBN.Text = row.Cells["Họ tên BN"].Value?.ToString() ?? "";

                if (row.Cells["Ngày sinh"].Value != DBNull.Value && row.Cells["Ngày sinh"].Value != null)
                {
                    dtNgaySinh.Value = Convert.ToDateTime(row.Cells["Ngày sinh"].Value);
                }

                string gt = row.Cells["Giới tính"].Value?.ToString() ?? "Nam";
                if (gt == "Nam") radioButton1.Checked = true;
                else radioButton2.Checked = true;

                txtDiachi.Text = row.Cells["Địa chỉ BN"].Value?.ToString() ?? "";
                txtDienThoai.Text = row.Cells["SĐT BN"].Value?.ToString() ?? "";

                
                txtBHYT.Text = row.Cells["Mã BHYT"].Value?.ToString() ?? "";

                if (row.Cells["Ngày cấp BHYT"].Value != DBNull.Value && row.Cells["Ngày cấp BHYT"].Value != null)
                    dtNgayCap.Value = Convert.ToDateTime(row.Cells["Ngày cấp BHYT"].Value);

                if (row.Cells["Ngày hết hạn BHYT"].Value != DBNull.Value && row.Cells["Ngày hết hạn BHYT"].Value != null)
                    dtNgayHet.Value = Convert.ToDateTime(row.Cells["Ngày hết hạn BHYT"].Value);

                comboBox1.Text = row.Cells["Mức hưởng %"].Value?.ToString() ?? "";

                
                txtTenNN.Text = row.Cells["Họ tên Người nhà"].Value?.ToString() ?? "";
                txtQH.Text = row.Cells["Quan hệ"].Value?.ToString() ?? "";
                txtDC.Text = row.Cells["Địa chỉ NN"].Value?.ToString() ?? "";
                txtDT.Text = row.Cells["SĐT NN"].Value?.ToString() ?? "";
            }
        }
    }
}
