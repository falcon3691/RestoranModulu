using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.garson
{
    public partial class UrunSecme : Form
    {

        VTUrunler vtUrun = new VTUrunler();
        VTSiparisler vtSiparis = new VTSiparisler();

        int siparisID, masaID, kullaniciID = 0;

        public UrunSecme(int masaID, int kullaniciID)
        {
            InitializeComponent();

            urunleriOlustur(vtUrun.Listele());

            dataGridView1.Columns.Add("urunID", "Ürün ID");
            dataGridView1.Columns["urunID"].Visible = false;
            dataGridView1.Columns.Add("urunAdi", "Ürün Adı");
            dataGridView1.Columns.Add("adet", "Adet");
            dataGridView1.Columns.Add("birimFiyat", "Birim Fiyatı");
            dataGridView1.Columns.Add("toplamFiyat", "Toplam Fiyatı");

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
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    int urunID = Convert.ToInt32(row.Cells["urunID"].Value);
                    string urunAdi = row.Cells["urunAdi"].Value.ToString() ?? "";
                    int miktar = Convert.ToInt32(row.Cells["adet"].Value);
                    int birimFiyat = Convert.ToInt32(row.Cells["birimFiyat"].Value);
                    int toplamFiyat = Convert.ToInt32(row.Cells["toplamFiyat"].Value);

                    vtSiparis.siparisDetayEkle(siparisID, urunID, miktar, birimFiyat, toplamFiyat, "bekliyor", urunAdi);
                }
                vtSiparis.siparisGuncelle(siparisID, Convert.ToInt32(label5.Text), "bekliyor");
                this.Close();
            }
        }

        public void urunleriOlustur(DataTable urunler)
        {
            for (int i = 0; i < urunler.Rows.Count; i++)
            {
                var satir = urunler.Rows[i];
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
            bool found = false;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["urunAdi"].Value != null && row.Cells["urunAdi"].Value.ToString() == urun.UrunAdi)
                {
                    int mevcutSayi = Convert.ToInt32(row.Cells["adet"].Value);
                    row.Cells["adet"].Value = mevcutSayi + 1;
                    row.Cells["toplamFiyat"].Value = (mevcutSayi + 1) * Convert.ToInt32(row.Cells["birimFiyat"].Value);
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                int rowIndex = dataGridView1.Rows.Add();
                var satir = dataGridView1.Rows[rowIndex];
                satir.Cells["urunID"].Value = urun.UrunID;
                satir.Cells["urunAdi"].Value = urun.UrunAdi;
                satir.Cells["adet"].Value = 1;
                satir.Cells["birimFiyat"].Value = urun.Fiyat;
                satir.Cells["toplamFiyat"].Value = urun.Fiyat;
            }
            label5.Text = toplamFiyatHesapla(dataGridView1).ToString();
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
            int kategoriID = int.Parse(comboBox1.Text);
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0)
            {
                var satir = dataGridView1.Rows[satirNo];
                int yeniMiktar = Convert.ToInt32(satir.Cells["adet"].Value) - 1;

                if (yeniMiktar == 0)
                {
                    dataGridView1.Rows.Remove(satir);
                    label5.Text = toplamFiyatHesapla(dataGridView1).ToString();
                }
                else
                {
                    satir.Cells["adet"].Value = yeniMiktar;
                    satir.Cells["toplamFiyat"].Value = yeniMiktar * Convert.ToInt32(satir.Cells["birimFiyat"].Value);
                    label5.Text = toplamFiyatHesapla(dataGridView1).ToString();
                }
            }
        }

        public int toplamFiyatHesapla(DataGridView dt)
        {
            int sonuc = 0;
            foreach (DataGridViewRow row in dt.Rows)
            {
                if (row.Cells["toplamFiyat"].Value != null)
                {
                    sonuc += Convert.ToInt32(row.Cells["toplamFiyat"].Value);
                }
            }
            return sonuc;
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

}
