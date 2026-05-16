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
    public partial class KeToan : Form
    {
        public KeToan()
        {
            InitializeComponent();
        }

        private void pnlMain_Click(object sender, EventArgs e)
        {

        }

        private void uiNavMenu1_MenuItemClick(TreeNode node, Sunny.UI.NavMenuItem item, int pageIndex)
        {
            pnlMain.Controls.Clear();

            // 2. Kiểm tra xem người dùng nhấn vào task nào (dựa vào Text hoặc Index)
            switch (node.Text)
            {
                case "Thanh toán":
                    ThanhToanHoaDon uc = new ThanhToanHoaDon();
                    uc.Dock = DockStyle.Fill;
                    pnlMain.Controls.Add(uc); // Đưa giao diện bệnh nhân vào bên phải
                    break;

                case "Xếp giường":
                    NhapVien uckho = new NhapVien();
                    uckho.Dock = DockStyle.Fill;
                    pnlMain.Controls.Add(uckho);
                    break;
            }
        }
    }
}
