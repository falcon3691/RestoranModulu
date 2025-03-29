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
        // Menu_Kullanıcı_Kullanıcıları düzenle butonu
        private void kullanıcılarıDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminKullaniciEkranı form = new AdminKullaniciEkranı();
            form.MdiParent = this;
            form.Show();
        }

        // Menu_Katlar butonu
        private void katlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminKatEkranı form = new AdminKatEkranı();
            form.MdiParent = this;
            form.Show();
        }
        // Menu_Masalar butonu
        private void masalarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminMasaEkranı form = new AdminMasaEkranı();
            form.MdiParent = this;
            form.Show();
        }
        // Menu_Rolleri Düzenle butonu
        private void rolleriDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminRollerEkranı form = new AdminRollerEkranı();
            form.MdiParent = this;
            form.Show();
        }

        // Menu_Ürünler butonu
        private void ürünlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminUrunlerEkranı form = new AdminUrunlerEkranı();
            form.MdiParent = this;
            form.Show();
        }

        private void siparişlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminSiparislerEkranı form = new AdminSiparislerEkranı();
            form.MdiParent = this;
            form.Show();
        }
    }
}
