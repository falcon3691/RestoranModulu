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
        DateTime siparisOlusturmaTarihi = DateTime.Now.AddDays((-1));

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

        public void urunleriOlustur(DataTable urunler)
        {
            for (int i = 0; i < urunler.Rows.Count; i++)
            {
                // Yeni bir Urun objesi oluşturulur ve özellikleri belirlenmek üzere döngüye sokulur.
                Urun urun = new Urun();
                for (int j = 0; j < urunler.Columns.Count; j++)
                {
                    if (j == 0)
                        urun.setUrunID(int.Parse(urunler.Rows[i][j].ToString()));       // Veri tabanından "urunID" değeri alınır ve Ürünün urunID özelliği olarak atanır.
                    else if (j == 1)
                        urun.setUrunAdi(urunler.Rows[i][j].ToString());                 // Veri tabanından "urunadi" değeri alınır ve Ürünün urunAdi özelliği olarak atanır.
                    else if (j == 2)
                        urun.setKategoriID(int.Parse(urunler.Rows[i][j].ToString()));   // Veri tabanından "kategoriID" değeri alınır ve Ürünün kategoriID özelliği olarak atanır.
                    else if (j == 3)
                        urun.setFiyat(int.Parse(urunler.Rows[i][j].ToString()));        // Veri tabanından "fiyat" değeri alınır ve Ürünün fiyat özelliği olarak atanır.
                    else if (j == 4)
                        urun.setMiktar(int.Parse(urunler.Rows[i][j].ToString()));       // Veri tabanından "miktar" değeri alınır ve Ürünün miktar özelliği olarak atanır.
                    else if (j == 5)
                        urun.setDurumu(urunler.Rows[i][j].ToString());                  // Veri tabanından "durumu" değeri alınır ve Ürünün durumu özelliği olarak atanır.
                    else if (j == 6)
                        urun.setResimYolu(urunler.Rows[i][j].ToString());               // Veri tabanından "resimYolu" değeri alınır ve Ürünün resimYolu özelliği olarak atanır.
                    else if (j == 7)
                        urun.setAciklama(urunler.Rows[i][j].ToString());                // Veri tabanından "aciklama" değeri alınır ve Ürünün aciklama özelliği olarak atanır.
                }

                // Oluşturulan ürünün özellikleri ile yeni bir Ürün butonu oluşturulur.
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
                Text = $"{urun.getUrunAdi()}\nFiyatı: {urun.getFiyat()}",
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

        // Geri Dön butonu
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = vtSiparis.Listele(masaID);
            siparisID = int.Parse(dt.Rows[0][0].ToString());
            if (siparisID > 0)
            {
                if (vtSiparis.siparisSil(siparisID))
                {
                    this.Close();
                }
            }
            else
                Console.WriteLine("Sipariş ID değeri alınamadı");
        }

        // Sipariş Ver butonu
        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = vtSiparis.Listele(masaID);
            siparisID = int.Parse(dt.Rows[0][0].ToString());
            if (siparisID > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    int urunID = int.Parse(row.Cells["urunID"].Value.ToString());
                    string urunAdi = row.Cells["urunAdi"].Value.ToString();
                    int miktar = int.Parse(row.Cells["adet"].Value.ToString());
                    int birimFiyat = int.Parse(row.Cells["birimFiyat"].Value.ToString());
                    int toplamFiyat = int.Parse(row.Cells["toplamFiyat"].Value.ToString());
                    if (vtSiparis.siparisDetayEkle(siparisID, urunID, miktar, birimFiyat, toplamFiyat, "ödenmedi", urunAdi))
                    {
                        if (vtSiparis.siparisGuncelle(siparisID, int.Parse(label5.Text), "bekliyor"))
                        {

                            this.Close();
                        }
                    }
                }
            }
            else
                Console.WriteLine("Sipariş ID değeri alınamadı");
        }
        public void satirEkle(Urun urun)
        {
            bool found = false;

            // DataGridView içindeki satırları kontrol et
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["urunAdi"].Value != null && row.Cells["urunAdi"].Value.ToString() == urun.getUrunAdi())
                {
                    // Aynı isme sahip bir satır bulundu, ikinci sütundaki sayıyı artır
                    int mevcutSayi = Convert.ToInt32(row.Cells["adet"].Value);
                    row.Cells["adet"].Value = mevcutSayi + 1;
                    row.Cells["toplamFiyat"].Value = (mevcutSayi + 1) * int.Parse(row.Cells["birimFiyat"].Value.ToString());
                    found = true;
                    label5.Text = toplamFiyatHesapla(dataGridView1).ToString();
                    break;
                }
            }
            if (!found)
            {
                int rowIndex = dataGridView1.Rows.Add();
                dataGridView1.Rows[rowIndex].Cells["urunID"].Value = urun.getUrunID();
                dataGridView1.Rows[rowIndex].Cells["urunAdi"].Value = urun.getUrunAdi();
                dataGridView1.Rows[rowIndex].Cells["adet"].Value = 1; // İlk başta 1 olarak eklenir
                dataGridView1.Rows[rowIndex].Cells["birimFiyat"].Value = urun.getFiyat();
                dataGridView1.Rows[rowIndex].Cells["toplamFiyat"].Value = urun.getFiyat();
            }
            label5.Text = toplamFiyatHesapla(dataGridView1).ToString();
        }

        // Ürün Adı ile filtreleme.
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string adi = textBox1.Text;
            flowLayoutPanel1.Controls.Clear();
            if (string.IsNullOrEmpty(adi))
                urunleriOlustur(vtUrun.Listele());
            else
            {
                if (vtUrun.urunFiltrele(adi) != null)
                    urunleriOlustur(vtUrun.urunFiltrele(adi));
                else
                    urunleriOlustur(vtUrun.Listele());
            }
        }

        // Kategori ile filtreleme.
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int kategoriID = int.Parse(comboBox1.Text);
            flowLayoutPanel1.Controls.Clear();
            if (vtUrun.urunFiltrele(null, kategoriID) != null)
                urunleriOlustur(vtUrun.urunFiltrele(null, kategoriID));
            else
                urunleriOlustur(vtUrun.Listele());
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0)
            {
                int yeniMiktar = int.Parse(dataGridView1.Rows[satirNo].Cells["adet"].Value.ToString()) - 1;

                if (yeniMiktar == 0)
                {
                    dataGridView1.Rows.RemoveAt(satirNo);
                    label5.Text = toplamFiyatHesapla(dataGridView1).ToString();
                }
                else
                {
                    dataGridView1.Rows[satirNo].Cells["adet"].Value = yeniMiktar;
                    dataGridView1.Rows[satirNo].Cells["toplamFiyat"].Value = yeniMiktar * int.Parse(dataGridView1.Rows[satirNo].Cells["birimFiyat"].Value.ToString());
                    label5.Text = toplamFiyatHesapla(dataGridView1).ToString();
                }

            }

        }

        public int toplamFiyatHesapla(DataGridView dt)
        {
            int sonuc = 0;
            foreach (DataGridViewRow row in dt.Rows)
            {
                // Yeni satır veya geçersiz satırlar varsa atla
                if (row.IsNewRow) continue;

                // Hücre null değilse ve içinde geçerli bir sayı varsa ekle
                if (row.Cells["toplamFiyat"].Value != null &&
                    int.TryParse(row.Cells["toplamFiyat"].Value.ToString(), out int deger))
                {
                    sonuc += deger;
                }
            }
            return sonuc;
        }

    }
    public class Urun
    {
        private int urunID;
        public int getUrunID() { return this.urunID; }
        public void setUrunID(int urunID) { this.urunID = urunID; }

        private string urunAdi;
        public string getUrunAdi() { return this.urunAdi; }
        public void setUrunAdi(string urunAdi) { this.urunAdi = urunAdi; }

        private int kategoriID;
        public int getKategoriID() { return this.kategoriID; }
        public void setKategoriID(int kategoriID) { this.kategoriID = kategoriID; }

        private int fiyat;
        public int getFiyat() { return this.fiyat; }
        public void setFiyat(int fiyat) { this.fiyat = fiyat; }

        private int miktar;
        public int getMiktar() { return this.miktar; }
        public void setMiktar(int miktar) { this.miktar = miktar; }

        private string durumu;
        public string getDurumu() { return this.durumu; }
        public void setDurumu(string durumu) { this.durumu = durumu; }

        private string resimYolu;
        public string getResimYolu() { return this.resimYolu; }
        public void setResimYolu(string resimYolu) { this.resimYolu = resimYolu; }

        private string aciklama;
        public string getAciklama() { return this.aciklama; }
        public void setAciklama(string aciklama) { this.aciklama = aciklama; }

    }

}
