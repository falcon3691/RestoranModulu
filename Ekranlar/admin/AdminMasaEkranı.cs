using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminMasaEkranı : Form
    {
        VTMasa vt = new VTMasa();

        // Masa değişken bilgileri.
        string adi, masaDurumu, aciklama = null;
        int masaID, sandalyeSayisi, katID = 0;

        public AdminMasaEkranı(string formAdi = null, int katID = 0)
        {
            InitializeComponent();
            // Bu ekran direkt olarak açılmaz, AdminKatEkranı üzerinden açılırsa, yapılacak özel işlemler
            if (formAdi == "AdminKatEkranı")
            {
                this.katID = katID;

                // Panel 4 ve Panel 6 gösterilir ve etkinleştirilir.
                panel4.Parent = this;
                panel6.Parent = this;

                panel4.Visible = true;
                panel6.Visible = true;
                panel4.Enabled = true;
                panel6.Enabled = true;


                // Panel 1, Panel 3 ve Panel 5 gizlenir ve etkisiz hale getirilir.
                panel1.Visible = false;
                panel3.Visible = false;
                panel5.Visible = false;
                panel1.Enabled = false;
                panel3.Enabled = false;
                panel5.Enabled = false;

                // Boşta olan ve kata ait olan masaların listelenmesi.
                dataGridView2.DataSource = vt.Listele(true);
                dataGridView3.DataSource = vt.Listele(true, katID);
            }
            else
                dataGridView1.DataSource = vt.Listele();
        }

        // Yeni Masa Ekle butonu
        private void button1_Click(object sender, System.EventArgs e)
        {
            string mesaj = boslukKontrolu();
            if (mesaj == null)
            {
                degerAtama();
                // Ürün ekleme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.masaEkle(adi, sandalyeSayisi, masaDurumu, katID, aciklama))
                {
                    if (katID != 0)
                    {
                        dataGridView2.DataSource = vt.Listele(true);
                        dataGridView3.DataSource = vt.Listele(true, katID);
                    }
                    else
                    {
                        dataGridView1.DataSource = vt.Listele();
                        temizle();

                    }
                }

            }
            else
                MessageBox.Show(mesaj, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Masayı Güncelle butonu
        private void button2_Click(object sender, System.EventArgs e)
        {
            if (masaID > 0)
            {
                degerAtama();
                // Masa güncelleme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.masaGuncelle(masaID, adi, sandalyeSayisi, masaDurumu, aciklama, katID))
                {
                    dataGridView1.DataSource = vt.Listele();
                    temizle();
                }
            }
            else
                MessageBox.Show("Masalar listesi içinden bir masa şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Masayı Kata Ekle butonu
        private void button7_Click(object sender, System.EventArgs e)
        {
            if (masaID > 0 && katID > 0)
            {
                // Masayı kata ekleme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.masayiKataEkle(masaID, katID))
                {
                    dataGridView1.DataSource = vt.Listele();
                    temizle();
                }
            }
            else
                MessageBox.Show("Masalar listesi içinden bir masa şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Masayı Sil butonu
        private void button3_Click(object sender, EventArgs e)
        {
            if (masaID > 0)
            {
                degerAtama();
                // Masa silme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.masaSil(masaID))
                {
                    dataGridView1.DataSource = vt.Listele();
                    temizle();
                }
            }
            else
                MessageBox.Show("Masalar listesi içinden bir masa şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Masaları Filtrele butonu
        private void button6_Click(object sender, EventArgs e)
        {
            degerAtama();
            if (vt.masaFiltrele(0, adi, sandalyeSayisi, masaDurumu, aciklama, katID) != null)
                dataGridView1.DataSource = vt.masaFiltrele(0, adi, sandalyeSayisi, masaDurumu, aciklama, katID);
            else
                dataGridView1.DataSource = vt.Listele();
        }

        // Boş Masaları Listele butonu
        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = vt.Listele(true);
            temizle();
        }

        // Hepsini Listele butonu
        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = vt.Listele();
            temizle();
        }

        // Boşta Olan Masalar listesi içinde seçilen masa, verilen katID değerine sahip kata eklenir.
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0)
            {
                if (katID != 0)
                {
                    masaID = int.Parse(dataGridView2.Rows[satirNo].Cells[0].Value.ToString());
                    adi = dataGridView2.Rows[satirNo].Cells[1].Value.ToString();
                    sandalyeSayisi = int.Parse(dataGridView2.Rows[satirNo].Cells[2].Value.ToString());
                    masaDurumu = dataGridView2.Rows[satirNo].Cells[3].Value.ToString();
                    aciklama = dataGridView2.Rows[satirNo].Cells[4].Value.ToString();

                    if (vt.masaGuncelle(masaID, adi, sandalyeSayisi, masaDurumu, aciklama, katID))
                    {
                        dataGridView2.DataSource = vt.Listele(true);
                        dataGridView3.DataSource = vt.Listele(true, katID);
                    }
                }
            }
        }

        //Kata ait masalar listesinden seçilen masa, listeden çıkartılır.
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0)
            {
                masaID = int.Parse(dataGridView3.Rows[satirNo].Cells[0].Value.ToString());
                adi = dataGridView3.Rows[satirNo].Cells[1].Value.ToString();
                sandalyeSayisi = int.Parse(dataGridView3.Rows[satirNo].Cells[2].Value.ToString());
                masaDurumu = dataGridView3.Rows[satirNo].Cells[3].Value.ToString();
                aciklama = dataGridView3.Rows[satirNo].Cells[4].Value.ToString();

                if (vt.masaGuncelle(masaID, adi, sandalyeSayisi, masaDurumu, aciklama, 0))
                {
                    dataGridView2.DataSource = vt.Listele(true);
                    dataGridView3.DataSource = vt.Listele(true, katID);
                }
            }

        }

        // Masaları Temizle butonu
        private void button8_Click(object sender, EventArgs e)
        {
            if (katID > 0)
            {
                // Masayı kata ekleme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.masalariTemizle(katID))
                {
                    dataGridView2.DataSource = vt.Listele(true);
                    dataGridView3.DataSource = vt.Listele(true, katID);
                }
            }
        }

        // dataGridView listesinde bulunan bir satıra tıklanınca, o satır içinde bulunan bilgiler ile textBox ve comboBox'ları doldurur.
        // Ayriyeten masa üstünde işlem yapmak için masaID değerini alır.
        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0)
            {
                masaID = int.Parse(dataGridView1.Rows[satirNo].Cells[0].Value.ToString());
                textBox1.Text = dataGridView1.Rows[satirNo].Cells[1].Value.ToString();
                textBox2.Text = dataGridView1.Rows[satirNo].Cells[2].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[satirNo].Cells[3].Value.ToString();
                if (panel3.Visible == true)
                {
                    textBox3.Text = dataGridView1.Rows[satirNo].Cells[4].Value.ToString();
                    textBox4.Text = dataGridView1.Rows[satirNo].Cells[5].Value.ToString();
                }
                else if (panel4.Visible == true)
                {
                    textBox5.Text = dataGridView1.Rows[satirNo].Cells[4].Value.ToString();
                }
            }
        }

        // Veri tablosunda NULL değer alamayan sütunlar için gerekli boşluk kontrolleri yapılır.
        public string boslukKontrolu()
        {
            string mesaj;
            List<string> hataMesajlari = new List<string>();
            if (string.IsNullOrEmpty(textBox1.Text))
                hataMesajlari.Add("Ad bilgisi boş bırakılamaz");
            if (string.IsNullOrEmpty(textBox2.Text))
                hataMesajlari.Add("Sandalye sayısı belirtilmeli");
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

            // Kullanıcı değişkenlerinin temizlenmesi.
            masaID = 0;
            katID = 0;
            sandalyeSayisi = 0;
            adi = null;
            masaDurumu = null;
            aciklama = null;
        }

        // Ürün değişkenlerine değer atama işlemleri
        public void degerAtama()
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                adi = textBox1.Text;
            else adi = null;
            if (!string.IsNullOrEmpty(comboBox1.Text))
                masaDurumu = comboBox1.Text;
            else masaDurumu = null;
            if (!string.IsNullOrEmpty(textBox2.Text))
                sandalyeSayisi = int.Parse(textBox2.Text);
            else sandalyeSayisi = 0;
            if (panel3.Visible == true)
            {
                if (!string.IsNullOrEmpty(textBox4.Text))
                    katID = int.Parse(textBox4.Text);
                else katID = 0;
                if (!string.IsNullOrEmpty(textBox3.Text))
                    aciklama = textBox3.Text;
                else aciklama = null;
            }
            else if (panel4.Visible == true)
            {
                if (!string.IsNullOrEmpty(textBox5.Text))
                    aciklama = textBox5.Text;
                else aciklama = null;
            }
        }
    }
}
