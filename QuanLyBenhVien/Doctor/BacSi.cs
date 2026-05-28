using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBenhVien
{
    public partial class BacSi : Form
    {
        public string manv {  get; set; }
        public BacSi()
        {
            InitializeComponent();
        }

        private void uiNavMenu1_MenuItemClick(TreeNode node, Sunny.UI.NavMenuItem item, int pageIndex)
        {
            // 1. Xóa giao diện cũ đang hiện ở bên phải
            pnlMain.Controls.Clear();

            // 2. Kiểm tra xem người dùng nhấn vào task nào (dựa vào Text hoặc Index)
            switch (node.Text)
            {
                case "Khám bệnh":
                    ChuanDoan uc = new ChuanDoan();
                    uc.manv = manv;
                    uc.Dock = DockStyle.Fill;
                    pnlMain.Controls.Add(uc); // Đưa giao diện bệnh nhân vào bên phải
                    break;

                case "Chỉ định dịch vụ":
                    DichVuChiDinh ucd = new DichVuChiDinh();
                    ucd.manv = manv;
                    ucd.Dock = DockStyle.Fill;
                    pnlMain.Controls.Add(ucd);
                    break;

                case "Đăng xuất":
                    this.Close();
                    break;
            }
        }

        private void BacSi_Load(object sender, EventArgs e)
        {

        }

        private void pnlMain_Click(object sender, EventArgs e)
        {

        }
    }
}
