using RestoranModulu.Ekranlar.admin;
using System;
using System.Windows.Forms;
namespace RestoranModulu
{
    public partial class GirisEkranı : Form
    {

        VeriTabani vt = new VeriTabani();
        public GirisEkranı()
        {
            InitializeComponent();
        }

        // Admin butonu
        private void button1_Click(object sender, EventArgs e)
        {
            if (boslukKontrolu())
            {
                if (vt.KullaniciListele("Kullanicilar", textBox1.Text, textBox2.Text) != null)
                {
                    AdminAnaEkranı form = new AdminAnaEkranı();
                    this.Hide();
                    form.ShowDialog();
                    this.Close();
                }
            }/*
            AdminAnaEkranı form = new AdminAnaEkranı();
            this.Hide();
            form.ShowDialog();
            this.Close();*/
        }
        // Kasa butonu
        private void button2_Click(object sender, EventArgs e)
        {

        }
        // Garson butonu
        private void button3_Click(object sender, EventArgs e)
        {

        }
        // Çıkma butonu
        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public bool boslukKontrolu()
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Kullanıcı adı boş bırakılamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Parola boş bırakılamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
