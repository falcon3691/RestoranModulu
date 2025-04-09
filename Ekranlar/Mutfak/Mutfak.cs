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
        public Mutfak()
        {
            InitializeComponent();
            LoadSiparisAndDetay();
        }

        public FlowLayoutPanel CreateSiparisPanel(Siparisler siparis)
        {
            DataTable dt = vtMutfak.masaBilgisiAl(siparis.masaID);
            string masaAdi = dt.Rows[0][1].ToString();
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
                Text = masaAdi,
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
                BackColor = siparis.durumu == "hazırlanıyor" ? Color.Green : siparis.durumu == "iptal" ? Color.Red : Color.Gray,
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
                Text = $"{detay.urunAdi}\nMiktar: {detay.miktar}",
                TextAlign = ContentAlignment.TopCenter,
                BackColor = detay.durumu == "bekliyor" ? Color.Green : detay.durumu == "iptal" ? Color.Red : Color.Gray,
                Tag = detay
            };

            // Masa Resmi
            /*PictureBox masaResmi = new PictureBox
            {
                Image = Image.FromFile("masa.png"), // Masa resmi yolu
                Width = 100,
                Height = 100,
                Location = new Point(10, 20),
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            // Masa Butonuna resmi ekleme
            masaButton.Controls.Add(masaResmi);*/
            return detayButton;
        }

        private void DetayButton_Click(object sender, EventArgs e)
        {
            Button tiklananButon = sender as Button;
            if (tiklananButon != null)
            {
                Detaylar detay = tiklananButon.Tag as Detaylar;
                if (detay != null)
                {
                    MessageBox.Show(detay.detayID.ToString());
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
                Siparisler siparis = new Siparisler();
                for (int j = 0; j < siparisListesi.Columns.Count; j++)
                {
                    if (j == 0)
                        siparis.siparisID = int.Parse(siparisListesi.Rows[i][j].ToString());
                    else if (j == 1)
                        siparis.masaID = int.Parse(siparisListesi.Rows[i][j].ToString());
                    else if (j == 4)
                        siparis.durumu = siparisListesi.Rows[i][j].ToString();
                    else if (j == 5)
                        siparis.olusturmaTarihi = DateTime.Parse(siparisListesi.Rows[i][j].ToString());
                }

                FlowLayoutPanel siparisPanel = CreateSiparisPanel(siparis);

                FlowLayoutPanel detayPanel = siparisPanel.Controls.OfType<FlowLayoutPanel>().FirstOrDefault();

                DataTable detaylar = vtSiparis.detayListele(siparis.siparisID);
                for (int x = 0; x < detaylar.Rows.Count; x++)
                {
                    Detaylar detay = new Detaylar();
                    for (int y = 0; y < detaylar.Columns.Count; y++)
                    {
                        if (y == 0)
                            detay.detayID = int.Parse(detaylar.Rows[x][y].ToString());
                        else if (y == 3)
                            detay.miktar = int.Parse(detaylar.Rows[x][y].ToString());
                        else if (y == 6)
                            detay.durumu = detaylar.Rows[x][y].ToString();
                        else if (y == 7)
                            detay.urunAdi = detaylar.Rows[x][y].ToString();
                    }

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
        public int siparisID;
        public int masaID;
        public string durumu;
        public DateTime olusturmaTarihi;

    }

    public class Detaylar
    {
        public int detayID;
        public int miktar;
        public string durumu;
        public string urunAdi;
    }
}
