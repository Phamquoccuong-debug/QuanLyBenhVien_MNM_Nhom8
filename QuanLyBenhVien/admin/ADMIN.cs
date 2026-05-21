using QuanLyBenhVien.admin;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBenhVien.admin
{
    public partial class CRUD : Form
    {
        public CRUD()
        {
            InitializeComponent();
            OpenChildForm(new NhanVien());
        }
        // Thay đổi kiểu dữ liệu đầu vào thành 'Control' (để nhận cả Form lẫn UserControl)
        private Control currentChildControl = null;

        private void OpenChildForm(Control childControl)
        {
            // 1. Nếu đang có giao diện mở trước đó, giải phóng nó khỏi Panel
            if (currentChildControl != null)
            {
                // Nếu nó là một Form thì ta mới Close, còn UserControl thì chỉ cần Dispose
                if (currentChildControl is Form form)
                {
                    form.Close();
                }
                else
                {
                    currentChildControl.Dispose();
                }
            }

            currentChildControl = childControl;

            // 2. Cấu hình thuộc tính nhúng (Nếu là Form thì chỉnh thêm TopLevel và FormBorderStyle)
            if (childControl is Form childForm)
            {
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
            }

            childControl.Dock = DockStyle.Fill; // Tự động co giãn giãn khít Panel

            // 3. Giả định Panel lớn bên phải của bạn tên là uiPanel1 (hoặc panelMain)
            // Bạn nhớ sửa 'uiPanel1' thành đúng tên Panel vùng xám ở thiết kế của bạn nhé
            panelMain.Controls.Clear();          // Xóa sạch giao diện cũ trong Panel
            panelMain.Controls.Add(childControl); // Nạp giao diện mới vào

            childControl.BringToFront();
            childControl.Show();                // Hiển thị lên
        }
        private void btnKhamBenh_Click(object sender, EventArgs e)
        {
            OpenChildForm(new NhanVien()); // NhanVien là file NhanVien.cs trong thư mục admin của bạn
        }

        private void btndichvu_Click(object sender, EventArgs e)
        {
            OpenChildForm(new DichVu());
        }

        private void btndangxuat_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Khoa());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new PhongBenh());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Thuoc());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Form loginForm = Application.OpenForms["Login"];
                if (loginForm != null)
                {
                    loginForm.Show(); // Hiện lại form đăng nhập ngầm
                }
                this.Close(); // Đóng form CRUD hiện tại
            }
        }

        private void pnlMenu_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
