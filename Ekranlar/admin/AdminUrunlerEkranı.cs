using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminUrunlerEkranı : Form
    {
        VTUrunler vt = new VTUrunler();

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
                vt.urunEkle(adi, kategoriID, fiyati, miktar, durumu, resimYolu, aciklama);
                dataGridView1.DataSource = vt.Listele();
                temizle();
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
                vt.urunGuncelle(urunID, adi, durumu, resimYolu, aciklama, kategoriID, fiyati, miktar);
                dataGridView1.DataSource = vt.Listele();
                temizle();
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
                vt.urunSil(urunID);
                dataGridView1.DataSource = vt.Listele();
                temizle();
            }
            else
                MessageBox.Show("Ürünler listesi içinden bir ürün şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Ürünleri Filtrele butonu
        private void button4_Click(object sender, EventArgs e)
        {
            degerAtama();
            DataTable filtrelenmisListe = vt.urunFiltrele(adi, durumu, resimYolu, aciklama, kategoriID, fiyati, miktar);
            if (filtrelenmisListe.Rows.Count > 0)
                dataGridView1.DataSource = filtrelenmisListe ?? vt.Listele();
            else
            {
                MessageBox.Show("Girilen bilgilere göre bir ürün bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridView1.DataSource = vt.Listele();
            }
        }

        // Hepsini Listele butonu
        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = vt.Listele();
            temizle();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0)
            {
                try
                {
                    var satir = dataGridView1.Rows[satirNo];
                    urunID = Convert.ToInt32(satir.Cells[0].Value);
                    textBox1.Text = satir.Cells[1].Value.ToString() ?? "";
                    comboBox2.Text = satir.Cells[2].Value.ToString() ?? "";
                    textBox4.Text = satir.Cells[3].Value.ToString() ?? "";
                    textBox3.Text = satir.Cells[4].Value.ToString() ?? "";
                    textBox5.Text = satir.Cells[5].Value.ToString() ?? "";
                    comboBox1.Text = satir.Cells[6].Value.ToString() ?? "";
                    textBox2.Text = satir.Cells[7].Value.ToString() ?? "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Satır verileri alınırken bir hata oluştu: " + ex.Message);
                }
            }
        }

        public string boslukKontrolu()
        {
            string mesaj = null;
            List<string> hataMesajlari = new List<string>();
            if (string.IsNullOrEmpty(textBox1.Text))
                hataMesajlari.Add("Ad bilgisi boş bırakılamaz");
            if (string.IsNullOrEmpty(comboBox2.Text))
                hataMesajlari.Add("Kategori bilgisi boş bırakılamaz");
            if (string.IsNullOrEmpty(textBox4.Text))
                hataMesajlari.Add("Fiyat bilgisi boş bırakılamaz");
            if (hataMesajlari.Count > 0)
                mesaj = string.Join(Environment.NewLine, hataMesajlari);
            return mesaj;
        }

        public void temizle()
        {
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            comboBox1.Text = null;
            comboBox2.Text = null;

            urunID = 0;
            kategoriID = 0;
            fiyati = 0;
            miktar = 0;
            adi = null;
            resimYolu = null;
            durumu = null;
            aciklama = null;
        }

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
