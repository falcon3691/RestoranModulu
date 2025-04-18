using RestoranModulu.Ekranlar.admin;
using RestoranModulu.Ekranlar.garson;
using RestoranModulu.Ekranlar.Mutfak;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
namespace RestoranModulu
{
    public partial class GirisEkranı : Form
    {
        VTGiris vt = new VTGiris();

        public GirisEkranı()
        {
            InitializeComponent();
        }

        // Admin butonu
        private void button1_Click(object sender, EventArgs e)
        {
            string mesaj = boslukKontrolu();
            if (mesaj == null)
            {
                DataTable dt = vt.KullaniciListele(textBox1.Text, textBox2.Text, 1);
                if (dt.Rows.Count > 0)
                {
                    AdminAnaEkranı form = new AdminAnaEkranı();
                    this.Hide();
                    form.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı.");
                }
            }
            else
                MessageBox.Show(mesaj, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Kasa butonu
        private void button2_Click(object sender, EventArgs e)
        {
            string mesaj = boslukKontrolu();
            if (mesaj == null)
            {
                DataTable dt = vt.KullaniciListele(textBox1.Text, textBox2.Text, 2);
                if (dt.Rows.Count > 0)
                {
                    int kullaniciID = Convert.ToInt32(dt.Rows[0]["kullaniciID"]);
                    KatEkranı form = new KatEkranı(kullaniciID, 2);
                    this.Hide();
                    form.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı.");
                }
            }
            else
                MessageBox.Show(mesaj, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Garson butonu
        private void button3_Click(object sender, EventArgs e)
        {
            string mesaj = boslukKontrolu();
            if (mesaj == null)
            {
                DataTable dt = vt.KullaniciListele(textBox1.Text, textBox2.Text, 3);
                if (dt.Rows.Count > 0)
                {
                    int kullaniciID = Convert.ToInt32(dt.Rows[0]["kullaniciID"]);
                    KatEkranı form = new KatEkranı(kullaniciID, 3);
                    this.Hide();
                    form.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı.");
                }
            }
            else
                MessageBox.Show(mesaj, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Mutfak butonu
        private void button5_Click(object sender, EventArgs e)
        {
            string mesaj = boslukKontrolu();
            if (mesaj == null)
            {
                DataTable dt = vt.KullaniciListele(textBox1.Text, textBox2.Text, 4);
                if (dt.Rows.Count > 0)
                {
                    int kullaniciID = Convert.ToInt32(dt.Rows[0]["kullaniciID"]);
                    Mutfak form = new Mutfak(kullaniciID);
                    this.Hide();
                    form.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı.");
                }
            }
            else
                MessageBox.Show(mesaj, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Çıkma butonu
        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public string boslukKontrolu()
        {
            string mesaj = null;

            List<string> hataListesi = new List<string>();
            if (string.IsNullOrEmpty(textBox1.Text))
                hataListesi.Add("Kullanıcı adı boş bırakılamaz.");
            if (string.IsNullOrEmpty(textBox2.Text))
                hataListesi.Add("Kullanıcı adı boş bırakılamaz.");

            if (hataListesi.Count > 0)
                mesaj = string.Join("\n", hataListesi);

            return mesaj;
        }

    }
}
