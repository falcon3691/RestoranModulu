using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminKullaniciEkranı : Form
    {
        // Kullanıcı bilgileri değişkenleri.
        int kullaniciID = 0;
        string adiSoyadi, telefon, eMail, kullaniciAdi, parola, rolID, durumu, aciklama = null;

        VTKullanicilar vt = new VTKullanicilar();

        public AdminKullaniciEkranı()
        {
            InitializeComponent();
            // "Kullanicilar" tablosundaki verileri listeler.
            dataGridView1.DataSource = vt.Listele();
            temizle();
        }

        // Yeni Kullanıcı Ekle butonu
        private void button1_Click(object sender, System.EventArgs e)
        {
            string mesaj = boslukKontrolu();
            if (mesaj == null)
            {
                degerAtama();
                // Kullanıcı ekleme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.KullaniciEkle(adiSoyadi, kullaniciAdi, parola, int.Parse(rolID), byte.Parse(durumu), aciklama, telefon, eMail))
                {
                    dataGridView1.DataSource = vt.Listele();
                    temizle();
                }

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
                // Kullanıcı güncelleme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.KullaniciGuncelle(kullaniciID, adiSoyadi, kullaniciAdi, parola, int.Parse(rolID), byte.Parse(durumu), aciklama, telefon, eMail))
                {
                    dataGridView1.DataSource = vt.Listele();
                    temizle();
                }
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
                // Kullanıcı silme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.KullaniciSil(kullaniciID))
                {
                    dataGridView1.DataSource = vt.Listele();
                    temizle();
                }
            }
            else
                MessageBox.Show("Kullanıcılar listesi içinden bir kullanıcı şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        // dataGridView listesinde bulunan bir satıra tıklanınca, o satır içinde bulunan bilgiler ile textBox ve comboBox'ları doldurur.
        // Ayriyeten kullanıcı üstünde işlem yapmak için kullaniciID değerini alır.
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < (dataGridView1.Rows.Count - 1))
            {
                kullaniciID = int.Parse(dataGridView1.Rows[satirNo].Cells[0].Value.ToString());
                textBox2.Text = dataGridView1.Rows[satirNo].Cells[1].Value.ToString();
                textBox1.Text = dataGridView1.Rows[satirNo].Cells[2].Value.ToString();
                textBox3.Text = dataGridView1.Rows[satirNo].Cells[3].Value.ToString();
                textBox4.Text = dataGridView1.Rows[satirNo].Cells[4].Value.ToString();
                textBox5.Text = dataGridView1.Rows[satirNo].Cells[5].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[satirNo].Cells[6].Value.ToString();
                comboBox2.Text = dataGridView1.Rows[satirNo].Cells[7].Value.ToString();
                textBox6.Text = dataGridView1.Rows[satirNo].Cells[8].Value.ToString();
            }
        }

        // Kullanıcıları Filtrele butonu
        private void button5_Click(object sender, EventArgs e)
        {
            degerAtama();
            if (vt.kullaniciFiltrele(adiSoyadi, kullaniciAdi, parola, rolID, durumu, aciklama, telefon, eMail) != null)
                dataGridView1.DataSource = vt.kullaniciFiltrele(adiSoyadi, kullaniciAdi, parola, rolID, durumu, aciklama, telefon, eMail);
            else
                dataGridView1.DataSource = vt.Listele();

        }

        // Hepsini Listele butonu
        private void button6_Click(object sender, System.EventArgs e)
        {
            dataGridView1.DataSource = vt.Listele();
            temizle();
        }

        // Hem ekranda ki alanları temizler hem de kullanılan değişkenlerin değerlerini standart konuma getirir.
        public void temizle()
        {
            // Ekranın temizlenmesi.
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            textBox6.Text = null;
            comboBox1.Text = null;
            comboBox2.Text = null;

            // Kullanıcı değişkenlerinin temizlenmesi.
            kullaniciID = 0;
            adiSoyadi = null;
            telefon = null;
            eMail = null;
            kullaniciAdi = null;
            parola = null;
            rolID = null;
            durumu = null;
            aciklama = null;
        }

        // Kullanıcı değişkenlerine değer atama işlemleri
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
                rolID = comboBox1.Text;
            else rolID = null;
            if (!string.IsNullOrEmpty(comboBox2.Text))
                durumu = comboBox2.Text;
            else durumu = null;
        }

        // Veri tablosunda NULL değer alamayan sütunlar için gerekli boşluk kontrolleri yapılır.
        public string boslukKontrolu()
        {
            string mesaj;
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
                return mesaj;
            }

            return null;
        }

    }
}
