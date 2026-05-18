namespace QuanLyBenhVien
{
    partial class KeToan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Phiếu thu tạm ứng");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Thanh toán hóa đơn");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Đăng xuất");
            this.pnlMain = new Sunny.UI.UIPanel();
            this.uiNavMenu1 = new Sunny.UI.UINavMenu();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.pnlMain.Location = new System.Drawing.Point(252, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlMain.MinimumSize = new System.Drawing.Size(1, 1);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1227, 779);
            this.pnlMain.TabIndex = 3;
            this.pnlMain.Text = "uiPanel1";
            this.pnlMain.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.pnlMain.Click += new System.EventHandler(this.pnlMain_Click);
            // 
            // uiNavMenu1
            // 
            this.uiNavMenu1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.uiNavMenu1.Dock = System.Windows.Forms.DockStyle.Left;
            this.uiNavMenu1.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.uiNavMenu1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.uiNavMenu1.FullRowSelect = true;
            this.uiNavMenu1.HotTracking = true;
            this.uiNavMenu1.ItemHeight = 50;
            this.uiNavMenu1.Location = new System.Drawing.Point(0, 0);
            this.uiNavMenu1.Name = "uiNavMenu1";
            treeNode1.Name = "PhieuThuTamUng";
            treeNode1.Text = "Phiếu thu tạm ứng";
            treeNode2.Name = "ThanhToanHoaDon";
            treeNode2.Text = "Thanh toán hóa đơn";
            treeNode3.Name = "DangXuat";
            treeNode3.Text = "Đăng xuất";
            this.uiNavMenu1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.uiNavMenu1.ShowLines = false;
            this.uiNavMenu1.ShowPlusMinus = false;
            this.uiNavMenu1.ShowRootLines = false;
            this.uiNavMenu1.Size = new System.Drawing.Size(252, 779);
            this.uiNavMenu1.TabIndex = 2;
            this.uiNavMenu1.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.uiNavMenu1.MenuItemClick += new Sunny.UI.UINavMenu.OnMenuItemClick(this.uiNavMenu1_MenuItemClick);
            // 
            // KeToan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1479, 779);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.uiNavMenu1);
            this.Name = "KeToan";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.KeToan_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIPanel pnlMain;
        private Sunny.UI.UINavMenu uiNavMenu1;
    }
}