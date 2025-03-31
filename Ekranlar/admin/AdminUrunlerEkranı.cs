using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminUrunlerEkranı : Form
    {
        VTUrunler vt = new VTUrunler();

        // Ürün bilgileri değişkenleri
        int urunID, kategoriID, fiyati, miktar = 0;
        string adi, durumu, resimYolu, aciklama = null;


        public AdminUrunlerEkranı()
        {
            InitializeComponent();
            dataGridView1.DataSource = vt.Listele();
        }

        // Yeni Ürün Ekle butonu
        private void button1_Click(object sender, System.EventArgs e)
        {
            string mesaj = boslukKontrolu();
            if (mesaj == null)
            {
                degerAtama();
                // Ürün ekleme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.urunEkle(adi, kategoriID, fiyati, miktar, durumu, resimYolu, aciklama))
                {
                    dataGridView1.DataSource = vt.Listele();
                    temizle();
                }

            }
            else
                MessageBox.Show(mesaj, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Ürünü Güncelle butonu
        private void button2_Click(object sender, EventArgs e)
        {
            if (urunID > 0)
            {
                degerAtama();
                // Ürün güncelleme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.urunGuncelle(urunID, adi, kategoriID, fiyati, miktar, durumu, resimYolu, aciklama))
                {
                    dataGridView1.DataSource = vt.Listele();
                    temizle();
                }
            }
            else
                MessageBox.Show("Ürünler listesi içinden bir ürün şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Ürünü Sil butonu
        private void button3_Click(object sender, EventArgs e)
        {
            if (urunID > 0)
            {
                degerAtama();
                // Ürün silme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.urunSil(urunID))
                {
                    dataGridView1.DataSource = vt.Listele();
                    temizle();
                }
            }
            else
                MessageBox.Show("Ürünler listesi içinden bir ürün şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Ürünleri Filtrele butonu
        private void button4_Click(object sender, EventArgs e)
        {
            degerAtama();
            if (vt.urunFiltrele(adi, kategoriID, fiyati, miktar, durumu, resimYolu, aciklama) != null)
                dataGridView1.DataSource = vt.urunFiltrele(adi, kategoriID, fiyati, miktar, durumu, resimYolu, aciklama);
            else
                dataGridView1.DataSource = vt.Listele();
        }

        // Hepsini Listele butonu
        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = vt.Listele();
            temizle();
        }

        // dataGridView listesinde bulunan bir satıra tıklanınca, o satır içinde bulunan bilgiler ile textBox ve comboBox'ları doldurur.
        // Ayriyeten ürün üstünde işlem yapmak için urunID değerini alır.
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            int satirNo = e.RowIndex;
            if (satirNo >= 0)
            {
                urunID = int.Parse(dataGridView1.Rows[satirNo].Cells[0].Value.ToString());
                textBox1.Text = dataGridView1.Rows[satirNo].Cells[1].Value.ToString();
                comboBox2.Text = dataGridView1.Rows[satirNo].Cells[2].Value.ToString();
                textBox4.Text = dataGridView1.Rows[satirNo].Cells[3].Value.ToString();
                textBox3.Text = dataGridView1.Rows[satirNo].Cells[4].Value.ToString();
                textBox5.Text = dataGridView1.Rows[satirNo].Cells[5].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[satirNo].Cells[6].Value.ToString();
                textBox2.Text = dataGridView1.Rows[satirNo].Cells[7].Value.ToString();
            }
        }

        // Veri tablosunda NULL değer alamayan sütunlar için gerekli boşluk kontrolleri yapılır.
        public string boslukKontrolu()
        {
            string mesaj;
            List<string> hataMesajlari = new List<string>();
            if (string.IsNullOrEmpty(textBox1.Text))
                hataMesajlari.Add("Ad bilgisi boş bırakılamaz");
            if (string.IsNullOrEmpty(comboBox2.Text))
                hataMesajlari.Add("Kategori bilgisi boş bırakılamaz");
            if (string.IsNullOrEmpty(textBox4.Text))
                hataMesajlari.Add("Fiyat bilgisi boş bırakılamaz");
            if (hataMesajlari.Count > 0)
            {
                mesaj = string.Join(Environment.NewLine, hataMesajlari);
                return mesaj;
            }

            return null;
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
            comboBox1.Text = null;
            comboBox2.Text = null;

            // Kullanıcı değişkenlerinin temizlenmesi.
            urunID = 0;
            kategoriID = 0;
            fiyati = 0;
            miktar = 0;
            adi = null;
            resimYolu = null;
            durumu = null;
            aciklama = null;
        }

        // Ürün değişkenlerine değer atama işlemleri
        public void degerAtama()
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                adi = textBox1.Text;
            else adi = null;
            if (!string.IsNullOrEmpty(comboBox2.Text))
                kategoriID = int.Parse(comboBox2.Text);
            else kategoriID = 0;
            if (!string.IsNullOrEmpty(textBox4.Text))
                fiyati = int.Parse(textBox4.Text);
            else fiyati = 0;
            if (!string.IsNullOrEmpty(textBox3.Text))
                miktar = int.Parse(textBox3.Text);
            else miktar = 0;
            if (!string.IsNullOrEmpty(textBox5.Text))
                resimYolu = textBox5.Text;
            else resimYolu = null;
            if (!string.IsNullOrEmpty(comboBox1.Text))
                durumu = comboBox1.Text;
            else durumu = null;
            if (!string.IsNullOrEmpty(textBox2.Text))
                aciklama = textBox2.Text;
            else aciklama = null;
        }
    }
}
