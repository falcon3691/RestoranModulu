using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminKullaniciEkranı : Form
    {
        int kullaniciID, rolID = 0;
        string adiSoyadi, telefon, eMail, kullaniciAdi, parola, durumu, aciklama = null;

        VTKullanicilar vt = new VTKullanicilar();

        public AdminKullaniciEkranı()
        {
            InitializeComponent();
            dataGridView1.DataSource = vt.Listele();
        }

        // Yeni Kullanıcı Ekle butonu
        private void button1_Click(object sender, System.EventArgs e)
        {
            string mesaj = boslukKontrolu();
            if (mesaj == null)
            {
                degerAtama();
                vt.KullaniciEkle(adiSoyadi, kullaniciAdi, parola, rolID, durumu, aciklama, telefon, eMail);
                dataGridView1.DataSource = vt.Listele();
                temizle();

            }
            else
                MessageBox.Show(mesaj, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Kullanıcıyı Güncelle butonu
        private void button3_Click(object sender, System.EventArgs e)
        {
            if (kullaniciID > 0)
            {
                degerAtama();
                vt.KullaniciGuncelle(kullaniciID, adiSoyadi, kullaniciAdi, parola, durumu, aciklama, telefon, eMail, rolID);
                dataGridView1.DataSource = vt.Listele();
                temizle();
            }
            else
                MessageBox.Show("Kullanıcılar listesi içinden bir kullanıcı şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Kullanıcıyı Sil butonu
        private void button4_Click(object sender, System.EventArgs e)
        {
            if (kullaniciID > 0)
            {
                degerAtama();
                vt.KullaniciSil(kullaniciID);
                dataGridView1.DataSource = vt.Listele();
                temizle();
            }
            else
                MessageBox.Show("Kullanıcılar listesi içinden bir kullanıcı şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        // Kullanıcıları Filtrele butonu
        private void button5_Click(object sender, EventArgs e)
        {
            degerAtama();
            DataTable filtrelenmisListe = vt.kullaniciFiltrele(adiSoyadi, rolID, durumu, telefon, eMail);
            if (filtrelenmisListe.Rows.Count > 0)
                dataGridView1.DataSource = filtrelenmisListe ?? vt.Listele();
            else
            {
                MessageBox.Show("Girilen bilgilere göre bir kullanıcı bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridView1.DataSource = vt.Listele();
            }
        }

        // Hepsini Listele butonu
        private void button6_Click(object sender, System.EventArgs e)
        {
            dataGridView1.DataSource = vt.Listele();
            temizle();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < dataGridView1.Rows.Count)
            {
                var satir = dataGridView1.Rows[satirNo];

                try
                {
                    kullaniciID = Convert.ToInt32(satir.Cells[0].Value);
                    textBox2.Text = satir.Cells[1].Value?.ToString() ?? "";
                    textBox1.Text = satir.Cells[2].Value?.ToString() ?? "";
                    textBox3.Text = satir.Cells[3].Value?.ToString() ?? "";
                    textBox4.Text = satir.Cells[4].Value?.ToString() ?? "";
                    textBox5.Text = satir.Cells[5].Value?.ToString() ?? "";
                    comboBox1.Text = satir.Cells[6].Value?.ToString() ?? "";
                    comboBox2.Text = satir.Cells[7].Value?.ToString() ?? "";
                    textBox6.Text = satir.Cells[8].Value?.ToString() ?? "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Satır verileri alınırken bir hata oluştu: " + ex.Message);
                }
            }
        }

        public void temizle()
        {
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            textBox6.Text = null;
            comboBox1.Text = null;
            comboBox2.Text = null;

            kullaniciID = 0;
            rolID = 0;
            adiSoyadi = null;
            telefon = null;
            eMail = null;
            kullaniciAdi = null;
            parola = null;
            durumu = null;
            aciklama = null;
        }

        public void degerAtama()
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
                adiSoyadi = textBox2.Text;
            else adiSoyadi = null;
            if (!string.IsNullOrEmpty(textBox1.Text))
                telefon = textBox1.Text;
            else telefon = null;
            if (!string.IsNullOrEmpty(textBox3.Text))
                eMail = textBox3.Text;
            else eMail = null;
            if (!string.IsNullOrEmpty(textBox4.Text))
                kullaniciAdi = textBox4.Text;
            else kullaniciAdi = null;
            if (!string.IsNullOrEmpty(textBox5.Text))
                parola = textBox5.Text;
            else parola = null;
            if (!string.IsNullOrEmpty(textBox6.Text))
                aciklama = textBox6.Text;
            else aciklama = null;
            if (!string.IsNullOrEmpty(comboBox1.Text))
                rolID = int.Parse(comboBox1.Text);
            else rolID = 0;
            if (!string.IsNullOrEmpty(comboBox2.Text))
                durumu = comboBox2.Text;
            else durumu = null;
        }

        public string boslukKontrolu()
        {
            string mesaj = null;
            List<string> hataMesajlari = new List<string>();
            if (string.IsNullOrEmpty(textBox2.Text))
                hataMesajlari.Add("Ad bilgisi boş bırakılamaz");
            if (string.IsNullOrEmpty(textBox4.Text))
                hataMesajlari.Add("Kullanıcı adı bilgisi boş bırakılamaz");
            if (string.IsNullOrEmpty(textBox5.Text))
                hataMesajlari.Add("Parola bilgisi boş bırakılamaz");
            if (string.IsNullOrEmpty(comboBox1.Text))
                hataMesajlari.Add("Rol bilgisi boş bırakılamaz");
            if (hataMesajlari.Count > 0)
            {
                mesaj = string.Join(Environment.NewLine, hataMesajlari);
            }

            return mesaj;
        }
    }
}
