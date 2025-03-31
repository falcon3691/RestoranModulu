using RestoranModulu.Ekranlar.admin;
using RestoranModulu.Ekranlar.garson;
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
            /* if (boslukKontrolu())
             {
                 if (vt.KullaniciListele(textBox1.Text, textBox2.Text, 1) != null)
                 {
                     AdminAnaEkranı form = new AdminAnaEkranı();
                     this.Hide();
                     form.ShowDialog();
                     this.Close();
                 }
             }*/
            AdminAnaEkranı form = new AdminAnaEkranı();
            this.Hide();
            form.ShowDialog();
            this.Close();
        }
        // Kasa butonu
        private void button2_Click(object sender, EventArgs e)
        {
            /* if (boslukKontrolu())
             {
                 if (vt.KullaniciListele(textBox1.Text, textBox2.Text, 2) != null)
                 {
                     KatEkranı form = new KatEkranı();
                     this.Hide();
                     form.ShowDialog();
                     this.Close();
                 }
             }*/
            KatEkranı form = new KatEkranı();
            this.Hide();
            form.ShowDialog();
            this.Close();
        }
        // Garson butonu
        private void button3_Click(object sender, EventArgs e)
        {
            /* if (boslukKontrolu())
             {
                 if (vt.KullaniciListele(textBox1.Text, textBox2.Text, 3) != null)
                 {
                     KatEkranı form = new KatEkranı();
                     this.Hide();
                     form.ShowDialog();
                     this.Close();
                 }
             }*/
            KatEkranı form = new KatEkranı();
            this.Hide();
            form.ShowDialog();
            this.Close();
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
