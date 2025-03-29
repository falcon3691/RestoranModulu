using System;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminAnaEkranı : Form
    {
        public AdminAnaEkranı()
        {
            InitializeComponent();
        }
        // Menu_Geri Dön butonu
        private void geriDönToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GirisEkranı form = new GirisEkranı();
            this.Hide();
            form.ShowDialog();
            this.Close();
        }
        // Menu_Kullanıcı_Kullanıcıları düzenle butonu
        private void kullanıcılarıDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminKullaniciEkranı form = new AdminKullaniciEkranı();
            form.MdiParent = this;
            form.Show();
        }
    }
}
