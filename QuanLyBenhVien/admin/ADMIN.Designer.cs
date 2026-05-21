namespace QuanLyBenhVien.admin
{
    partial class CRUD
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
            this.btnKhamBenh = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btndangxuat = new System.Windows.Forms.Button();
            this.btndichvu = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnKhamBenh
            // 
            this.btnKhamBenh.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnKhamBenh.Location = new System.Drawing.Point(0, 0);
            this.btnKhamBenh.Name = "btnKhamBenh";
            this.btnKhamBenh.Size = new System.Drawing.Size(160, 79);
            this.btnKhamBenh.TabIndex = 0;
            this.btnKhamBenh.Text = "Nhân viên";
            this.btnKhamBenh.UseVisualStyleBackColor = true;
            this.btnKhamBenh.Click += new System.EventHandler(this.btnKhamBenh_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(620, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Admin";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1396, 100);
            this.panel1.TabIndex = 1;
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.panelMain);
            this.pnlContent.Controls.Add(this.pnlMenu);
            this.pnlContent.Controls.Add(this.panel1);
            this.pnlContent.Location = new System.Drawing.Point(12, 7);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1396, 721);
            this.pnlContent.TabIndex = 4;
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(160, 100);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1236, 621);
            this.panelMain.TabIndex = 5;
            // 
            // pnlMenu
            // 
            this.pnlMenu.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlMenu.Controls.Add(this.button4);
            this.pnlMenu.Controls.Add(this.button2);
            this.pnlMenu.Controls.Add(this.button1);
            this.pnlMenu.Controls.Add(this.btndangxuat);
            this.pnlMenu.Controls.Add(this.btndichvu);
            this.pnlMenu.Controls.Add(this.btnKhamBenh);
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMenu.Location = new System.Drawing.Point(0, 100);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(160, 621);
            this.pnlMenu.TabIndex = 4;
            this.pnlMenu.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlMenu_Paint);
            // 
            // button4
            // 
            this.button4.Dock = System.Windows.Forms.DockStyle.Top;
            this.button4.Location = new System.Drawing.Point(0, 335);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(160, 63);
            this.button4.TabIndex = 6;
            this.button4.Text = "Đăng Xuất";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Top;
            this.button2.Location = new System.Drawing.Point(0, 274);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(160, 61);
            this.button2.TabIndex = 4;
            this.button2.Text = "Thuốc";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(0, 210);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 64);
            this.button1.TabIndex = 3;
            this.button1.Text = "Phòng bệnh";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btndangxuat
            // 
            this.btndangxuat.Dock = System.Windows.Forms.DockStyle.Top;
            this.btndangxuat.Location = new System.Drawing.Point(0, 143);
            this.btndangxuat.Name = "btndangxuat";
            this.btndangxuat.Size = new System.Drawing.Size(160, 67);
            this.btndangxuat.TabIndex = 2;
            this.btndangxuat.Text = "Khoa - Phòng Khám";
            this.btndangxuat.UseVisualStyleBackColor = true;
            this.btndangxuat.Click += new System.EventHandler(this.btndangxuat_Click);
            // 
            // btndichvu
            // 
            this.btndichvu.Dock = System.Windows.Forms.DockStyle.Top;
            this.btndichvu.Location = new System.Drawing.Point(0, 79);
            this.btndichvu.Name = "btndichvu";
            this.btndichvu.Size = new System.Drawing.Size(160, 64);
            this.btndichvu.TabIndex = 1;
            this.btndichvu.Text = "Dịch Vụ";
            this.btndichvu.UseVisualStyleBackColor = true;
            this.btndichvu.Click += new System.EventHandler(this.btndichvu_Click);
            // 
            // CRUD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1420, 717);
            this.Controls.Add(this.pnlContent);
            this.Name = "CRUD";
            this.Text = "CRUD";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.pnlMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnKhamBenh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btndangxuat;
        private System.Windows.Forms.Button btndichvu;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Panel panelMain;
    }
}