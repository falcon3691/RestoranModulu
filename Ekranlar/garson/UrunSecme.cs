using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.garson
{
    public partial class UrunSecme : Form
    {

        VTUrunler vtUrun = new VTUrunler();
        VTSiparisler vtSiparis = new VTSiparisler();

        int siparisID, masaID, kullaniciID = 0;

        List<SiparisDetay> secilenUrunler = new List<SiparisDetay>();

        public UrunSecme(int masaID, int kullaniciID)
        {
            InitializeComponent();

            urunleriOlustur(vtUrun.Listele());
            kategorileriAyarla(vtUrun.kategoriListele());
            this.masaID = masaID;
            this.kullaniciID = kullaniciID;

        }

        // Geri Dön butonu
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = vtSiparis.Listele(masaID);
            siparisID = Convert.ToInt32(dt.Rows[0][0].ToString());
            if (siparisID > 0)
            {
                vtSiparis.siparisSil(siparisID);
                this.Close();
            }
        }

        // Sipariş Ver butonu
        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = vtSiparis.Listele(masaID);
            siparisID = Convert.ToInt32(dt.Rows[0]["siparisID"]);
            if (siparisID > 0)
            {
                foreach (var urun in secilenUrunler)
                {
                    int urunID = Convert.ToInt32(urun.UrunID);
                    string urunAdi = urun.UrunAdi ?? "";
                    int miktar = Convert.ToInt32(urun.Adet);
                    int birimFiyat = Convert.ToInt32(urun.BirimFiyat);
                    int toplamFiyat = Convert.ToInt32(urun.ToplamFiyat);
                    string detayNot = urun.Not;
                    vtSiparis.siparisDetayEkle(siparisID, urunID, miktar, birimFiyat, toplamFiyat, "bekliyor", urunAdi, detayNot);
                }
                string not = textBox2.Text;
                if (!string.IsNullOrEmpty(not))
                {
                    vtSiparis.siparisGuncelle(siparisID, Convert.ToInt32(label5.Text), "bekliyor", not);
                }
                else
                    vtSiparis.siparisGuncelle(siparisID, Convert.ToInt32(label5.Text), "bekliyor", null);
                this.Close();
            }
        }

        public void urunleriOlustur(DataTable urunler)
        {
            foreach (DataRow satir in urunler.Rows)
            {
                Urun urun = new Urun(
                    Convert.ToInt32(satir["urunID"]),
                    satir["adi"].ToString() ?? "",
                    Convert.ToInt32(satir["kategoriID"]),
                    Convert.ToInt32(satir["fiyati"]),
                    Convert.ToInt32(satir["miktar"]),
                    satir["durumu"].ToString() ?? "",
                    satir["resimYolu"].ToString() ?? "",
                    satir["aciklama"].ToString() ?? ""
                );

                Button urunButton = CreateUrunButton(urun);
                urunButton.Click += UrunButton_Click;
                flowLayoutPanel1.Controls.Add(urunButton);
            }
        }

        public Button CreateUrunButton(Urun urun)
        {
            Button urunButton = new Button
            {
                Width = 120,
                Height = 120,
                Text = $"{urun.UrunAdi}\nFiyatı: {urun.Fiyat}",
                TextAlign = ContentAlignment.TopCenter,
                Tag = urun
            };
            return urunButton;
        }

        private void UrunButton_Click(object sender, EventArgs e)
        {
            Button tiklananButon = sender as Button;
            if (tiklananButon != null)
            {
                Urun urun = tiklananButon.Tag as Urun;
                if (urun != null)
                {
                    satirEkle(urun);
                }
            }
        }

        public void satirEkle(Urun urun)
        {
            bool bulundu = false;

            foreach (var detay in secilenUrunler)
            {
                if (detay.UrunAdi == urun.UrunAdi)
                {
                    detay.Adet += 1;
                    bulundu = true;
                    break;
                }
            }

            if (!bulundu)
            {
                secilenUrunler.Add(new SiparisDetay(urun.UrunID, urun.UrunAdi, 1, urun.Fiyat));
            }

            decimal toplam = secilenUrunler.Sum(x => x.ToplamFiyat);
            label5.Text = toplam.ToString();

            flowLayoutPanel2.Controls.Clear();
            foreach (var detay in secilenUrunler)
            {
                Panel panel = CreateSiparisDetayPanel(detay);
                flowLayoutPanel2.Controls.Add(panel);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string adi = textBox1.Text;
            flowLayoutPanel1.Controls.Clear();
            if (string.IsNullOrEmpty(adi))
                urunleriOlustur(vtUrun.Listele());
            else
            {
                DataTable filtrelenmisListe = vtUrun.urunFiltrele(adi, null, null, null);
                if (filtrelenmisListe.Rows.Count > 0)
                    urunleriOlustur(filtrelenmisListe ?? vtUrun.Listele());
                else
                {
                    MessageBox.Show("Girilen bilgilere göre bir ürün bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    urunleriOlustur(vtUrun.Listele());
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Enabled)
            {
                if (comboBox1.SelectedItem is DataRowView row)
                {
                    int kategoriID = Convert.ToInt32(row["kategoriID"]);
                    flowLayoutPanel1.Controls.Clear();
                    DataTable filtrelenmisListe = vtUrun.urunFiltrele(null, null, null, null, kategoriID);
                    if (filtrelenmisListe.Rows.Count > 0)
                        urunleriOlustur(filtrelenmisListe ?? vtUrun.Listele());
                    else
                    {
                        MessageBox.Show("Girilen bilgilere göre bir ürün bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        urunleriOlustur(vtUrun.Listele());
                    }
                }
            }
        }

        public Panel CreateSiparisDetayPanel(SiparisDetay detay)
        {
            Panel panel = new Panel
            {
                Width = 500,
                Height = 40,
                BorderStyle = BorderStyle.FixedSingle,
                Tag = detay
            };

            // Ürün Adı
            Label lblUrunAdi = new Label
            {
                Text = detay.UrunAdi,
                Width = 130,
                Location = new Point(10, 10),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Birim Fiyat
            Label lblBirimFiyat = new Label
            {
                Text = $"{detay.BirimFiyat}",
                Width = 80,
                Location = new Point(125, 10),
                TextAlign = ContentAlignment.MiddleCenter
            };


            // "-" Butonu
            Button btnAzalt = new Button
            {
                Text = "-",
                Width = 25,
                Height = 23,
                Location = new Point(210, 8),
                Tag = panel
            };
            btnAzalt.Click += BtnAzalt_Click;

            // Adet
            Label lblAdet = new Label
            {
                Text = detay.Adet.ToString(),
                Width = 30,
                Location = new Point(240, 10),
                TextAlign = ContentAlignment.MiddleCenter,
                Name = "lblAdet"
            };

            // "+" Butonu
            Button btnArtir = new Button
            {
                Text = "+",
                Width = 25,
                Height = 23,
                Location = new Point(275, 8),
                Tag = panel
            };
            btnArtir.Click += BtnArtir_Click;

            // Toplam Fiyat
            Label lblToplamFiyat = new Label
            {
                Text = (detay.BirimFiyat * detay.Adet).ToString(),
                Width = 80,
                Location = new Point(310, 10),
                TextAlign = ContentAlignment.MiddleCenter,
                Name = "lblToplamFiyat"
            };

            // Not Butonu
            Button btnNot = new Button
            {
                Text = "Not",
                Width = 45,
                Height = 23,
                Location = new Point(400, 8),
                Tag = detay
            };

            btnNot.Click += (s, e) =>
            {
                DetayNotEkleme form = new DetayNotEkleme(detay.Not);
                this.SuspendLayout();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    detay.Not = form.NotIcerigi;
                }
                this.ResumeLayout();
            };

            panel.Controls.Add(lblUrunAdi);
            panel.Controls.Add(lblBirimFiyat);
            panel.Controls.Add(lblAdet);
            panel.Controls.Add(lblToplamFiyat);
            panel.Controls.Add(btnAzalt);
            panel.Controls.Add(btnArtir);
            panel.Controls.Add(btnNot);

            return panel;
        }

        private void BtnAzalt_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Panel panel = btn.Tag as Panel;
            var detay = panel.Tag as SiparisDetay;

            if (detay.Adet > 1)
            {
                detay.Adet--;
                var lblAdet = panel.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblAdet");
                var lblToplam = panel.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblToplamFiyat");

                lblAdet.Text = detay.Adet.ToString();
                lblToplam.Text = (detay.BirimFiyat * detay.Adet).ToString();
            }
            else
            {
                secilenUrunler.Remove(detay);
                panel.Dispose();
            }

            decimal toplam = secilenUrunler.Sum(x => x.ToplamFiyat);
            label5.Text = toplam.ToString();
        }

        private void BtnArtir_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Panel panel = btn.Tag as Panel;
            var detay = panel.Tag as SiparisDetay;

            detay.Adet++;
            var lblAdet = panel.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblAdet");
            var lblToplam = panel.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblToplamFiyat");

            lblAdet.Text = detay.Adet.ToString();
            lblToplam.Text = (detay.BirimFiyat * detay.Adet).ToString();

            decimal toplam = secilenUrunler.Sum(x => x.ToplamFiyat);
            label5.Text = toplam.ToString();
        }

        public void kategorileriAyarla(DataTable dtKategori)
        {
            DataRow yeniSatir = dtKategori.NewRow();
            yeniSatir["kategoriID"] = 0;
            yeniSatir["adi"] = "Bir kategori seçin";
            dtKategori.Rows.InsertAt(yeniSatir, 0);

            comboBox1.DataSource = dtKategori;
            comboBox1.DisplayMember = "adi";
            comboBox1.ValueMember = "kategoriID";
            comboBox1.Enabled = true;
        }
    }
    public class Urun
    {
        public int UrunID { get; }

        public string UrunAdi { get; }

        public int KategoriID { get; }

        public int Fiyat { get; }

        public int Miktar { get; }

        public string Durumu { get; }

        public string ResimYolu { get; }

        public string Aciklama { get; }

        public Urun(int urunID, string urunAdi, int kategoriID, int fiyat, int miktar, string durumu, string resimYolu, string aciklama)
        {
            this.UrunID = urunID;
            this.UrunAdi = urunAdi;
            this.KategoriID = kategoriID;
            this.Fiyat = fiyat;
            this.Miktar = miktar;
            this.Durumu = durumu;
            this.ResimYolu = resimYolu;
            this.Aciklama = aciklama;
        }
    }
    public class SiparisDetay
    {
        public int UrunID { get; set; }
        public string UrunAdi { get; set; }
        public int Adet { get; set; }
        public string Not { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal ToplamFiyat => BirimFiyat * Adet;

        public SiparisDetay(int urunID, string urunAdi, int adet, decimal birimFiyat)
        {
            UrunID = urunID;
            UrunAdi = urunAdi;
            Adet = adet;
            BirimFiyat = birimFiyat;
        }
    }
}
