using System;
using System.Collections.Generic;
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
        List<Siparisler> gosterilenSiparisler = new List<Siparisler>();
        List<Detaylar> gosterilenDetaylar = new List<Detaylar>();

        public Mutfak(int kullaniciID)
        {
            InitializeComponent();
            LoadSiparisAndDetay();
            this.kullaniciID = kullaniciID;
        }

        public FlowLayoutPanel CreateSiparisPanel(Siparisler siparis)
        {
            FlowLayoutPanel pnlSiparis = new FlowLayoutPanel();
            pnlSiparis.Name = $"flpSiparis_{siparis.SiparisID}";
            pnlSiparis.Width = 700;
            pnlSiparis.Height = 260;
            pnlSiparis.BorderStyle = BorderStyle.FixedSingle;
            pnlSiparis.Margin = new Padding(10);
            pnlSiparis.BackColor = Color.LightGray;
            pnlSiparis.AutoScroll = true;
            pnlSiparis.WrapContents = true;
            pnlSiparis.Tag = siparis.SiparisID;

            // Masa adı
            Label lblMasa = new Label();
            lblMasa.Text = siparis.MasaAdi;
            lblMasa.AutoSize = true;
            lblMasa.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblMasa.Location = new Point(10, 10);
            pnlSiparis.Controls.Add(lblMasa);

            // Sipariş durumu
            Label lblDurum = new Label();
            lblDurum.Text = "Durum: " + siparis.Durumu;
            lblDurum.AutoSize = true;
            lblDurum.Location = new Point(10, 35);
            pnlSiparis.Controls.Add(lblDurum);

            // Sipariş zamanı
            Label lblZaman = new Label();
            lblZaman.Text = "Zaman: " + siparis.OlusturmaTarihi.ToString("HH:mm:ss");
            lblZaman.AutoSize = true;
            lblZaman.Location = new Point(10, 60);
            pnlSiparis.Controls.Add(lblZaman);

            // Not varsa not butonu
            if (!string.IsNullOrEmpty(siparis.SiparisNotu))
            {
                Button btnNot = new Button();
                btnNot.Text = "Notu Gör";
                btnNot.Location = new Point(10, 85);
                btnNot.Click += (s, e) => MessageBox.Show(siparis.SiparisNotu, "Sipariş Notu");
                pnlSiparis.Controls.Add(btnNot);
            }

            // İptal butonu
            Button btnIptal = new Button();
            btnIptal.Text = "İptal Et";
            btnIptal.Location = new Point(100, 85);
            btnIptal.Click += (s, e) => SiparisiIptalEt(siparis.SiparisID);
            pnlSiparis.Controls.Add(btnIptal);

            // Siparişi Tamamla butonu (başta görünmez)
            Button btnTamamla = new Button();
            btnTamamla.Text = "Siparişi Tamamla";
            btnTamamla.Name = $"btnTamamla_{siparis.SiparisID}";
            btnTamamla.Size = new Size(150, 30);
            btnTamamla.Location = new Point(500, 30);
            btnTamamla.Visible = false;
            btnTamamla.Click += (s, e) => SiparisiTamamla(s, e, siparis.SiparisID);
            pnlSiparis.Controls.Add(btnTamamla);
            btnTamamla.Tag = new { SiparisPanel = pnlSiparis };

            // İç sipariş detayları paneli
            FlowLayoutPanel flpDetaylar = new FlowLayoutPanel();
            flpDetaylar.Location = new Point(10, 115);
            flpDetaylar.Size = new Size(430, 200);
            flpDetaylar.AutoScroll = true;
            flpDetaylar.Name = $"flpDetaylar_{siparis.SiparisID}";
            pnlSiparis.Controls.Add(flpDetaylar);

            //pnlSiparis.Tag = new { SiparisID = siparis.SiparisID, DetayPanel = flpDetaylar };
            return pnlSiparis;
        }

        private void SiparisiTamamla(object sender, EventArgs e, int siparisID)
        {
            var tagInfo = (sender as Button).Tag;
            var siparisPanel = tagInfo.GetType().GetProperty("SiparisPanel").GetValue(tagInfo) as FlowLayoutPanel;
            var detayPanel = siparisPanel.Controls.OfType<FlowLayoutPanel>().FirstOrDefault(panel => (string)panel.Name == $"flpDetaylar_{siparisID}");
            var panel1 = detayPanel.Controls.OfType<Panel>().ToList();

            vtSiparis.siparisGuncelle(Convert.ToInt32(siparisPanel.Tag), 0, "tamamlandı", null);
            foreach (Control kontrol in panel1)
                vtSiparis.siparisDetayGuncelle(Convert.ToInt32(kontrol.Name.ToString().Remove(0, 9)), "tamamlandı");

            var siparis = gosterilenSiparisler.FirstOrDefault(s => s.SiparisID == siparisID);
            if (siparis != null)
                gosterilenSiparisler.Remove(siparis);

            gosterilenDetaylar.RemoveAll(d => d.SiparisID == siparisID);

            LoadSiparisAndDetay();
        }


        public void SiparisiIptalEt(int siparisID)
        {
            DialogResult sonuc = MessageBox.Show("Bu siparişi iptal etmek istiyor musunuz?", "İptal Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sonuc != DialogResult.Yes)
                return;

            vtSiparis.siparisSil(siparisID);
            var siparis = gosterilenSiparisler.FirstOrDefault(s => s.SiparisID == siparisID);
            if (siparis != null)
                gosterilenSiparisler.Remove(siparis);

            // Aynı siparişe ait detayları da sil
            gosterilenDetaylar.RemoveAll(d => d.SiparisID == siparisID);
            LoadSiparisAndDetay();
        }

        public Panel CreateDetayPanel(Detaylar detay, FlowLayoutPanel siparisPanel)
        {
            Panel pnlDetay = new Panel();
            pnlDetay.Name = $"flpDetay_{detay.DetayID}";
            pnlDetay.Width = 200;
            pnlDetay.Height = 60;
            pnlDetay.BorderStyle = BorderStyle.FixedSingle;
            pnlDetay.Margin = new Padding(5);
            pnlDetay.Tag = detay.DetayID;

            // Ürün adı ve miktar
            Label lblUrun = new Label();
            lblUrun.Text = $"{detay.UrunAdi} ({detay.Miktar} adet)";
            lblUrun.AutoSize = true;
            lblUrun.Location = new Point(5, 5);
            pnlDetay.Controls.Add(lblUrun);

            // Not varsa buton
            if (!string.IsNullOrEmpty(detay.DetayNotu))
            {
                Button btnUrunNot = new Button();
                btnUrunNot.Text = "Not";
                btnUrunNot.Size = new Size(40, 25);
                btnUrunNot.Location = new Point(5, 25);
                btnUrunNot.Click += (s, e) => MessageBox.Show(detay.DetayNotu, "Ürün Notu");
                pnlDetay.Controls.Add(btnUrunNot);
            }

            // Ürün iptal
            Button btnUrunIptal = new Button();
            btnUrunIptal.Text = "İptal";
            btnUrunIptal.Size = new Size(40, 25);
            btnUrunIptal.Location = new Point(50, 25);
            btnUrunIptal.Click += (s, e) => SiparisDetayiIptalEt(detay.DetayID);
            pnlDetay.Controls.Add(btnUrunIptal);

            // Durum butonu
            Button btnDurum = new Button();
            btnDurum.Name = $"btnDurum_{detay.DetayID}";
            btnDurum.Text = detay.Durumu;
            btnDurum.Size = new Size(60, 25);
            btnDurum.Location = new Point(100, 25);
            btnDurum.Tag = new { Durum = detay.Durumu, SiparisPanel = siparisPanel };
            btnDurum.BackColor = DurumRengineGoreRenk(detay.Durumu);
            btnDurum.Click += (s, e) => DurumuDegistirVeGuncelle((Button)s, detay.DetayID);
            btnDurum.Tag = new { Durum = detay.Durumu, SiparisPanel = siparisPanel };
            pnlDetay.Controls.Add(btnDurum);

            return pnlDetay;
        }

        public void SiparisDetayiIptalEt(int siparisDetayID)
        {
            DialogResult sonuc = MessageBox.Show("Bu sipariş detayını iptal etmek istiyor musunuz?", "İptal Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sonuc != DialogResult.Yes)
                return;
            vtSiparis.siparisDetaySil(siparisDetayID);

            var detay = gosterilenDetaylar.FirstOrDefault(d => d.DetayID == siparisDetayID);
            if (detay != null)
                gosterilenDetaylar.Remove(detay);

            LoadSiparisAndDetay();
        }

        private void DurumuDegistirVeGuncelle(Button btnDurum, int detayID)
        {
            // Detay objesini bul
            var detay = gosterilenDetaylar.FirstOrDefault(d => d.DetayID == detayID);

            if (detay == null) return;

            // UI'da yeni durum belirle (örnek: bekliyor → hazırlanıyor → hazır → bekliyor döngüsü)
            string yeniDurum;
            switch (detay.Durumu)
            {
                case "bekliyor":
                    yeniDurum = "hazırlanıyor";
                    break;
                case "hazırlanıyor":
                    yeniDurum = "hazır";
                    break;
                case "hazır":
                    yeniDurum = "hazır";
                    break;
                default:
                    yeniDurum = "bekliyor";
                    break;
            }



            // UI'da buton rengini ve metnini güncelle
            btnDurum.Text = yeniDurum;

            switch (yeniDurum)
            {
                case "bekliyor":
                    btnDurum.BackColor = Color.LightGray;
                    break;
                case "hazırlanıyor":
                    btnDurum.BackColor = Color.Orange;
                    break;
                case "hazır":
                    btnDurum.BackColor = Color.LightGreen;
                    break;
                default:
                    btnDurum.BackColor = Color.White;
                    break;
            }

            // Listede durumu güncelle
            detay.Durumu = yeniDurum;

            // Bu detaya ait SiparisID'yi al
            int siparisID = detay.SiparisID;

            var tagInfo = btnDurum.Tag; // Önce Tag bilgisini alıyoruz.

            if (tagInfo != null)
            {
                var panelProperty = tagInfo.GetType().GetProperty("SiparisPanel");
                if (panelProperty != null)
                {
                    FlowLayoutPanel siparisPanel1 = panelProperty.GetValue(tagInfo) as FlowLayoutPanel;
                    if (siparisPanel1 != null)
                    {
                        // Paneldeki "Durum" label'ını da güncelle
                        foreach (Control ctrl in siparisPanel1.Controls)
                        {
                            if (ctrl is Label lbl && lbl.Text.StartsWith("Durum:"))
                            {
                                lbl.Text = "Durum: Hazırlanıyor";
                                break;
                            }
                        }
                    }
                }
            }


            // Bu siparişe ait tüm detayları bul
            var ilgiliDetaylar = gosterilenDetaylar.Where(d => d.SiparisID == siparisID).ToList();
            // Eğer hepsi "hazır" ise siparişi tamamla butonunu bul ve görünür yap
            bool hepsiHazir = ilgiliDetaylar.All(d => d.Durumu == "hazır");

            if (hepsiHazir)
            {
                // Ekranda ilgili sipariş panelini bul ve tamamla butonunun görünürlüğünü ayarla
                var siparisPanel = flowLayoutPanel1.Controls.OfType<FlowLayoutPanel>()
                                            .FirstOrDefault(panel => (int)panel.Tag == siparisID);

                if (siparisPanel != null)
                {
                    // Sipariş panelindeki her bir detay paneli için işlemi yap
                    foreach (Control kontrol in siparisPanel.Controls.OfType<FlowLayoutPanel>())
                    {
                        foreach (Control detayPanel in kontrol.Controls)
                        {
                            foreach (var ilgiliDetay in ilgiliDetaylar)
                            {
                                if (detayPanel is Panel p && p.Tag != null && (int)p.Tag == ilgiliDetay.DetayID)
                                {
                                    // Durum değiştirme butonunun görünürlüğünü ayarla
                                    Button durumbtn = p.Controls.OfType<Button>()
                                        .FirstOrDefault(b => b.Name == $"btnDurum_{ilgiliDetay.DetayID}");

                                    if (durumbtn != null)
                                    {
                                        durumbtn.Visible = false; // Durumu 'hazır' olduğunda butonu gizle
                                    }
                                    break; // Detay paneli için işlemi sonlandır
                                }
                            }
                        }
                    }
                    // Tamamla butonunun görünürlüğünü ayarla
                    siparisPanel.Controls.OfType<Button>()
                        .Where(btn => btn.Name == $"btnTamamla_{siparisID}")
                        .ToList()
                        .ForEach(btn => btn.Visible = true);
                }


            }
        }

        private Color DurumRengineGoreRenk(string durum)
        {
            switch (durum)
            {
                case "bekliyor": return Color.LightGray;
                case "hazırlanıyor": return Color.Khaki;
                case "hazır": return Color.LightGreen;
                default: return Color.White;
            }
        }

        public void LoadSiparisAndDetay()
        {
            flowLayoutPanel1.Controls.Clear();
            gosterilenSiparisler.Clear();
            gosterilenDetaylar.Clear();
            DataTable siparisListesi = vtMutfak.siparisleriListe();
            for (int i = 0; i < siparisListesi.Rows.Count; i++)
            {
                var satirSiparis = siparisListesi.Rows[i];
                Siparisler siparis = new Siparisler(
                    Convert.ToInt32(satirSiparis["siparisID"]),
                    Convert.ToInt32(satirSiparis["masaID"]),
                    satirSiparis["durumu"].ToString() ?? "",
                    DateTime.Parse(satirSiparis["olusturmaTarihi"].ToString()),
                    satirSiparis["siparisNot"].ToString() ?? ""

                );
                gosterilenSiparisler.Add(siparis);
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
                        satirDetay["urunAdi"].ToString() ?? "",
                        satirDetay["detayNot"].ToString() ?? "",
                        Convert.ToInt32(satirDetay["siparisID"])
                    );
                    gosterilenDetaylar.Add(detay);
                    Panel detaySiparis = CreateDetayPanel(detay, siparisPanel);
                    detayPanel.Controls.Add(detaySiparis);
                }
                flowLayoutPanel1.Controls.Add(siparisPanel);
            }
        }

    }

    public class Siparisler
    {
        public int SiparisID { get; }
        public int MasaID { get; }
        public string Durumu { get; set; }
        public string MasaAdi { get; }
        public DateTime OlusturmaTarihi { get; }
        public string SiparisNotu { get; }

        public Siparisler(int siparisID, int masaID, string durumu, DateTime olusturmaTarihi, string siparisNotu)
        {
            VTMutfak vt = new VTMutfak();
            this.SiparisID = siparisID;
            this.MasaID = masaID;
            this.Durumu = durumu;
            this.OlusturmaTarihi = olusturmaTarihi;
            this.MasaAdi = vt.masaBilgisiAl(masaID).Rows[0]["adi"].ToString() ?? "";
            this.SiparisNotu = siparisNotu;
        }
    }

    public class Detaylar
    {
        public int DetayID { get; }
        public int Miktar { get; }
        public string Durumu { get; set; }
        public string UrunAdi { get; }
        public string DetayNotu { get; }
        public int SiparisID { get; }

        public Detaylar(int detayID, int miktar, string durumu, string urunAdi, string detayNotu, int siparisID)
        {
            this.DetayID = detayID;
            this.Miktar = miktar;
            this.Durumu = durumu;
            this.UrunAdi = urunAdi;
            this.DetayNotu = detayNotu;
            this.SiparisID = siparisID;
        }
    }
}
