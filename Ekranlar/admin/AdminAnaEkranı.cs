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

        // Menü_Siparişler butonu
        private void siparişlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminSiparislerEkranı form = new AdminSiparislerEkranı();
            form.MdiParent = this;
            form.Show();
        }

        // Menü_Menü butonu
        private void menüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminMenuEkranı form = new AdminMenuEkranı();
            form.MdiParent = this;
            form.Show();
        }

        // Menu_Ürünler_Ürünleri Düzenle butonu
        private void ürünleriDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminUrunlerEkranı form = new AdminUrunlerEkranı();
            form.MdiParent = this;
            form.Show();
        }

        // Menü_Ürünler_Kategorileri Düzenle butonu
        private void kategorileriDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminKategoriEkranı form = new AdminKategoriEkranı();
            form.MdiParent = this;
            form.Show();
        }

        private void mutfakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mutfak.Mutfak form = new Mutfak.Mutfak();
            form.MdiParent = this;
            form.Show();
        }
    }
}
