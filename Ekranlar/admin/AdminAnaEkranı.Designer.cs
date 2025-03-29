namespace RestoranModulu.Ekranlar.admin
{
    partial class AdminAnaEkranı
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.katlarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.masalarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kullanıcılarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.siparişlerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ürünlerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menüToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mutfakToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.geriDönToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rolleriDüzenleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kullanıcılarıDüzenleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.katlarToolStripMenuItem,
            this.masalarToolStripMenuItem,
            this.kullanıcılarToolStripMenuItem,
            this.siparişlerToolStripMenuItem,
            this.ürünlerToolStripMenuItem,
            this.menüToolStripMenuItem,
            this.mutfakToolStripMenuItem,
            this.geriDönToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 36);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // katlarToolStripMenuItem
            // 
            this.katlarToolStripMenuItem.Name = "katlarToolStripMenuItem";
            this.katlarToolStripMenuItem.Size = new System.Drawing.Size(62, 24);
            this.katlarToolStripMenuItem.Text = "Katlar";
            // 
            // masalarToolStripMenuItem
            // 
            this.masalarToolStripMenuItem.Name = "masalarToolStripMenuItem";
            this.masalarToolStripMenuItem.Size = new System.Drawing.Size(75, 24);
            this.masalarToolStripMenuItem.Text = "Masalar";
            // 
            // kullanıcılarToolStripMenuItem
            // 
            this.kullanıcılarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rolleriDüzenleToolStripMenuItem,
            this.kullanıcılarıDüzenleToolStripMenuItem});
            this.kullanıcılarToolStripMenuItem.Name = "kullanıcılarToolStripMenuItem";
            this.kullanıcılarToolStripMenuItem.Size = new System.Drawing.Size(121, 32);
            this.kullanıcılarToolStripMenuItem.Text = "Kullanıcılar";
            // 
            // siparişlerToolStripMenuItem
            // 
            this.siparişlerToolStripMenuItem.Name = "siparişlerToolStripMenuItem";
            this.siparişlerToolStripMenuItem.Size = new System.Drawing.Size(84, 24);
            this.siparişlerToolStripMenuItem.Text = "Siparişler";
            // 
            // ürünlerToolStripMenuItem
            // 
            this.ürünlerToolStripMenuItem.Name = "ürünlerToolStripMenuItem";
            this.ürünlerToolStripMenuItem.Size = new System.Drawing.Size(71, 24);
            this.ürünlerToolStripMenuItem.Text = "Ürünler";
            // 
            // menüToolStripMenuItem
            // 
            this.menüToolStripMenuItem.Name = "menüToolStripMenuItem";
            this.menüToolStripMenuItem.Size = new System.Drawing.Size(60, 24);
            this.menüToolStripMenuItem.Text = "Menü";
            // 
            // mutfakToolStripMenuItem
            // 
            this.mutfakToolStripMenuItem.Name = "mutfakToolStripMenuItem";
            this.mutfakToolStripMenuItem.Size = new System.Drawing.Size(69, 24);
            this.mutfakToolStripMenuItem.Text = "Mutfak";
            // 
            // geriDönToolStripMenuItem
            // 
            this.geriDönToolStripMenuItem.Name = "geriDönToolStripMenuItem";
            this.geriDönToolStripMenuItem.Size = new System.Drawing.Size(82, 24);
            this.geriDönToolStripMenuItem.Text = "Geri Dön";
            this.geriDönToolStripMenuItem.Click += new System.EventHandler(this.geriDönToolStripMenuItem_Click);
            // 
            // rolleriDüzenleToolStripMenuItem
            // 
            this.rolleriDüzenleToolStripMenuItem.Name = "rolleriDüzenleToolStripMenuItem";
            this.rolleriDüzenleToolStripMenuItem.Size = new System.Drawing.Size(271, 32);
            this.rolleriDüzenleToolStripMenuItem.Text = "Rolleri düzenle";
            // 
            // kullanıcılarıDüzenleToolStripMenuItem
            // 
            this.kullanıcılarıDüzenleToolStripMenuItem.Name = "kullanıcılarıDüzenleToolStripMenuItem";
            this.kullanıcılarıDüzenleToolStripMenuItem.Size = new System.Drawing.Size(271, 32);
            this.kullanıcılarıDüzenleToolStripMenuItem.Text = "Kullanıcıları düzenle";
            this.kullanıcılarıDüzenleToolStripMenuItem.Click += new System.EventHandler(this.kullanıcılarıDüzenleToolStripMenuItem_Click);
            // 
            // AdminAnaEkranı
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AdminAnaEkranı";
            this.Text = "AdminAnaEkranı";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem katlarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem masalarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kullanıcılarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem siparişlerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ürünlerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menüToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mutfakToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem geriDönToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rolleriDüzenleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kullanıcılarıDüzenleToolStripMenuItem;
    }
}