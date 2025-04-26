using RestoranModulu.Ekranlar.Kasa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.garson
{
    public partial class MasaDetay : Form
    {
        VTMasa vtMasa = new VTMasa();
        VTSiparisler vtSiparis = new VTSiparisler();

        // Masa değişken bilgileri.
        string adi, masaDurumu, aciklama, siparisDurumu = null;
        int masaID, sandalyeSayisi, kullaniciID = 0;

        List<int> toplamFiyatlar = new List<int>();
        public MasaDetay(int masaID, int kullaniciID, int rolID)
        {
            InitializeComponent();
            degerAtama(masaID);
            ekranDoldurma();
            this.kullaniciID = kullaniciID;
            urunleriListele(masaID);
            if (rolID == 2)
                button3.Visible = true;
            /*if (dataGridView1.Columns.Contains("siparisDetayID"))
            {
                dataGridView1.Columns["siparisDetayID"].Visible = false;
            }*/
        }

        // Ödeme Yap butonu
        private void button3_Click(object sender, System.EventArgs e)
        {
            Odeme form = new Odeme(masaID);
            this.SuspendLayout();
            form.ShowDialog();
            urunleriListele(masaID);
        }

        // Masa Birleştir butonu
        private void button4_Click(object sender, EventArgs e)
        {
            // masaID değeri ile masa grup işlemlerinin yapılacağı ekrana geçiş yapılır.
            MasaGrup form = new MasaGrup(this.masaID, this.adi);
            this.SuspendLayout();
            form.ShowDialog();
        }

        // Sipariş Ver butonu
        private void button1_Click(object sender, System.EventArgs e)
        {
            vtSiparis.siparisEkle(masaID, kullaniciID);
            UrunSecme form = new UrunSecme(masaID, kullaniciID);
            this.SuspendLayout();
            form.ShowDialog();
            urunleriListele(masaID);

        }

        // Geri Dön butonu
        private void button2_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        public void degerAtama(int masaID)
        {
            DataTable dt = vtMasa.masaFiltrele(masaID, null, 0, null, 0);
            if (dt != null)
            {
                var satir = dt.Rows[0];
                this.masaID = Convert.ToInt32(satir[0]);
                this.adi = satir[1].ToString() ?? "";
                this.sandalyeSayisi = Convert.ToInt32(satir[2]);
                this.masaDurumu = satir[3].ToString() ?? "";
                this.aciklama = satir[4].ToString() ?? "";
            }
        }

        public void urunleriListele(int masaID)
        {
            int toplamTutar = 0;
            DataTable dt = new DataTable();
            foreach (DataRow siparis in vtSiparis.Listele2(masaID).Rows)
            {
                // Ödenmemiş siparişlerin listesi oluşturuluyor.
                dt.Merge(vtSiparis.detayListele(Convert.ToInt32(siparis[0]), true));
            }

            foreach (DataRow row in dt.Rows)
            {
                string urunAdi = row["urunAdi"].ToString() ?? "";
                int birimFiyat = Convert.ToInt32(row["birimFiyat"]);
                int miktar = Convert.ToInt32(row["miktar"]);
                string durumu = row["durumu"].ToString() ?? "";
                string detayNot = row["detayNot"].ToString() ?? "";
                toplamTutar += Convert.ToInt32(row["toplamFiyat"]);
                SiparisDetay1 detay = new SiparisDetay1(urunAdi, miktar, birimFiyat, detayNot, durumu);

                Panel panel = CreateSiparisDetayPanel(detay);
                flowLayoutPanel1.Controls.Add(panel);
            }

            label5.Text = toplamTutar.ToString();

        }

        public Panel CreateSiparisDetayPanel(SiparisDetay1 detay)
        {
            Panel panel = new Panel
            {
                Width = 450,
                Height = 40,
                BorderStyle = BorderStyle.FixedSingle,
                Tag = detay
            };

            // Ürün Adı
            Label lblUrunAdi = new Label
            {
                Text = detay.UrunAdi,
                Width = 100,
                Location = new Point(10, 10),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Birim Fiyat
            Label lblBirimFiyat = new Label
            {
                Text = "Birim Fiyatı: " + detay.BirimFiyat.ToString(),
                Width = 80,
                Location = new Point(115, 10),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Adet
            Label lblAdet = new Label
            {
                Text = "Adeti: " + detay.Adet.ToString(),
                Width = 50,
                Location = new Point(200, 10),
                TextAlign = ContentAlignment.MiddleCenter,
                Name = "lblAdet"
            };


            // Toplam Fiyat
            Label lblToplamFiyat = new Label
            {
                Text = "Toplam Fiyat: " + (detay.BirimFiyat * detay.Adet).ToString(),
                Width = 130,
                Location = new Point(255, 10),
                TextAlign = ContentAlignment.MiddleCenter,
                Name = "lblToplamFiyat"
            };

            if (!string.IsNullOrEmpty(detay.Not.ToString()))
            {
                // Not Butonu
                Button btnNot = new Button
                {
                    Text = "Not",
                    Width = 45,
                    Height = 23,
                    Location = new Point(390, 8),
                    Tag = detay
                };

                btnNot.Click += (s, e) =>
                {
                    DetayNotEkleme form = new DetayNotEkleme(detay.Not, true);
                    this.SuspendLayout();
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        detay.Not = form.NotIcerigi;
                    }
                    this.ResumeLayout();
                };
                panel.Controls.Add(btnNot);
            }

            panel.Controls.Add(lblUrunAdi);
            panel.Controls.Add(lblBirimFiyat);
            panel.Controls.Add(lblAdet);
            panel.Controls.Add(lblToplamFiyat);

            return panel;
        }
        public void ekranDoldurma()
        {
            label1.Text = adi ?? "";
            label3.Text = sandalyeSayisi.ToString() ?? "0";
            if (masaDurumu == "boş")
                pictureBox1.BackColor = Color.LightGreen;
            else if (masaDurumu == "dolu")
                pictureBox1.BackColor = Color.Red;
            else if (masaDurumu == "rezerve")
                pictureBox1.BackColor = Color.Yellow;

            DataTable dt = vtSiparis.enSonSiparisiBul(masaID);
            if (dt.Rows.Count > 0)
            {
                siparisDurumu = dt.Rows[0][0].ToString() ?? "";
                if (siparisDurumu == "bekliyor")
                    pictureBox2.BackColor = Color.Red;
                else if (siparisDurumu == "hazırlanıyor")
                    pictureBox2.BackColor = Color.Yellow;
                else if (siparisDurumu == "tamamlandı")
                    pictureBox2.BackColor = Color.LightGreen;
                else if (siparisDurumu == "ödendi")
                    pictureBox2.BackColor = Color.LightGray;
            }
        }
    }

    public class SiparisDetay1
    {
        public string UrunAdi { get; set; }
        public int Adet { get; set; }
        public string Not { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal ToplamFiyat => BirimFiyat * Adet;
        public string Durum { get; set; }

        public SiparisDetay1(string urunAdi, int adet, decimal birimFiyat, string detayNot, string durum)
        {
            UrunAdi = urunAdi;
            Adet = adet;
            BirimFiyat = birimFiyat;
            Not = detayNot;
            Durum = durum;
        }
    }
}
