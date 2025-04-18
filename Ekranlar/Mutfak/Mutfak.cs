using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.Mutfak
{
    public partial class Mutfak : Form
    {
        VTMutfak vtMutfak = new VTMutfak();
        VTSiparisler vtSiparis = new VTSiparisler();

        // Kullanıcı ID ileride kullanılmak istenebilir.
        int kullaniciID = 0;

        public Mutfak(int kullaniciID)
        {
            InitializeComponent();
            LoadSiparisAndDetay();
            this.kullaniciID = kullaniciID;
        }

        public FlowLayoutPanel CreateSiparisPanel(Siparisler siparis)
        {
            FlowLayoutPanel siparisPanel = new FlowLayoutPanel
            {
                Width = 660,
                Height = 350,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                Tag = siparis
            };

            // Siparişin ait olduğunu masanın adı
            Label lblMasaAdi = new Label
            {
                Text = siparis.MasaAdi,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(30, 5),
                AutoSize = true
            };

            // Siparişin hazırlanma durumu (Daire)
            PictureBox durumGosterge = new PictureBox
            {
                Width = 20,
                Height = 20,
                Location = new Point(5, 5),
                BackColor = siparis.Durumu == "belkliyor" ? Color.Gray : siparis.Durumu == "hazırlanıyor" ? Color.LightGreen : siparis.Durumu == "iptal" ? Color.Red : Color.Gray,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Sipariş edilen ürünlerin alanı
            FlowLayoutPanel urunPanel = new FlowLayoutPanel
            {
                Width = 700,
                Height = 260,
                Location = new Point(5, 90),
                AutoScroll = true
            };

            // Kat Paneline öğeleri ekleme
            siparisPanel.Controls.Add(durumGosterge);
            siparisPanel.Controls.Add(lblMasaAdi);
            siparisPanel.Controls.Add(urunPanel);

            return siparisPanel;
        }

        public Button CreateDetayButton(Detaylar detay)
        {
            Button detayButton = new Button
            {
                Width = 120,
                Height = 120,
                Text = $"{detay.UrunAdi}\nMiktar: {detay.Miktar}",
                TextAlign = ContentAlignment.TopCenter,
                BackColor = detay.Durumu == "bekliyor" ? Color.Gray : detay.Durumu == "hazırlanıyor" ? Color.LightGreen : detay.Durumu == "iptal" ? Color.Red : Color.Gray,
                Tag = detay
            };

            // Ürün Resmi
            /*PictureBox urunResmi = new PictureBox
            {
                Image = Image.FromFile("urun.png"),
                Width = 100,
                Height = 100,
                Location = new Point(10, 20),
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            // Detay Butonuna resmi ekleme
            detayButton.Controls.Add(urunResmi);*/
            return detayButton;
        }

        private void DetayButton_Click(object sender, EventArgs e)
        {
            if (sender is Button tiklananButon)
            {
                if (tiklananButon.Tag is Detaylar detay)
                {
                    MessageBox.Show(detay.UrunAdi.ToString());
                    // Detay butonuna tıklanınca, ekranda o ürüne ait not gözükecek.
                    // MessageBox ile gösterilip, "Tamam" tıklanmazsa 10 saniye sonra otomatik kapanacak şekilde ayarlanabilir.
                }
            }
        }

        public void LoadSiparisAndDetay()
        {
            DataTable siparisListesi = vtMutfak.siparisleriListe();
            for (int i = 0; i < siparisListesi.Rows.Count; i++)
            {
                var satirSiparis = siparisListesi.Rows[i];
                Siparisler siparis = new Siparisler(
                    Convert.ToInt32(satirSiparis["siparisID"]),
                    Convert.ToInt32(satirSiparis["masaID"]),
                    satirSiparis["durumu"].ToString() ?? "",
                    DateTime.Parse(satirSiparis["olusturmaTarihi"].ToString())
                );
                FlowLayoutPanel siparisPanel = CreateSiparisPanel(siparis);
                FlowLayoutPanel detayPanel = siparisPanel.Controls.OfType<FlowLayoutPanel>().FirstOrDefault();

                DataTable detaylar = vtSiparis.detayListele(siparis.SiparisID);
                for (int x = 0; x < detaylar.Rows.Count; x++)
                {
                    var satirDetay = detaylar.Rows[x];
                    Detaylar detay = new Detaylar(
                        Convert.ToInt32(satirDetay["siparisDetayID"]),
                        Convert.ToInt32(satirDetay["miktar"]),
                        satirDetay["durumu"].ToString() ?? "",
                        satirDetay["urunAdi"].ToString() ?? ""
                    );
                    Button detayButton = CreateDetayButton(detay);
                    detayButton.Click += DetayButton_Click;
                    detayPanel.Controls.Add(detayButton);
                }
                flowLayoutPanel1.Controls.Add(siparisPanel);
            }
        }
    }

    public class Siparisler
    {
        public int SiparisID { get; }
        public int MasaID { get; }
        public string Durumu { get; }
        public string MasaAdi { get; }
        public DateTime OlusturmaTarihi { get; }

        public Siparisler(int siparisID, int masaID, string durumu, DateTime olusturmaTarihi)
        {
            this.SiparisID = siparisID;
            this.MasaID = masaID;
            this.Durumu = durumu;
            this.OlusturmaTarihi = olusturmaTarihi;
        }
    }

    public class Detaylar
    {
        public int DetayID { get; }
        public int Miktar { get; }
        public string Durumu { get; }
        public string UrunAdi { get; }

        public Detaylar(int detayID, int miktar, string durumu, string urunAdi)
        {
            this.DetayID = detayID;
            this.Miktar = miktar;
            this.Durumu = durumu;
            this.UrunAdi = urunAdi;
        }
    }
}
