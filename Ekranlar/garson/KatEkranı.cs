using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.garson
{
    public partial class KatEkranı : Form
    {
        public KatEkranı()
        {
            InitializeComponent();
        }

        public FlowLayoutPanel CreateKatPanel(int katId, string katAdi, string aciklama, string durumu)
        {
            FlowLayoutPanel katPanel = new FlowLayoutPanel
            {
                Width = 660,
                Height = 350,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                Tag = katId
            };

            // Kat Adı
            Label lblKatAdi = new Label
            {
                Text = katAdi,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(30, 5),
                AutoSize = true
            };

            // Kat Durum Göstergesi (Daire)
            PictureBox durumGosterge = new PictureBox
            {
                Width = 20,
                Height = 20,
                Location = new Point(5, 5),
                BackColor = durumu == "Aktif" ? Color.Green : durumu == "Temizlik" ? Color.Red : Color.Gray,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Açıklama Kutusu
            Label lblAciklama = new Label
            {
                Text = string.IsNullOrEmpty(aciklama) ? "Açıklama yok." : aciklama,
                Font = new Font("Arial", 10, FontStyle.Italic),
                Location = new Point(5, 30),
                Width = 290,
                Height = 50,
                AutoSize = false
            };

            // Masa Alanı
            FlowLayoutPanel masaPanel = new FlowLayoutPanel
            {
                Width = 700,
                Height = 260,
                Location = new Point(5, 90),
                AutoScroll = true
            };

            // Kat Paneline öğeleri ekleme
            katPanel.Controls.Add(durumGosterge);
            katPanel.Controls.Add(lblKatAdi);
            katPanel.Controls.Add(lblAciklama);
            katPanel.Controls.Add(masaPanel);

            return katPanel;
        }


        public Button CreateMasaButton(int masaId, string masaAdi, int sandalyeSayisi, int katID, string durumu)
        {
            Button masaButton = new Button
            {
                Width = 120,
                Height = 120,
                Text = $"{masaAdi}\nSandalyeler: {sandalyeSayisi}",
                TextAlign = ContentAlignment.TopCenter,
                BackColor = durumu == "Dolu" ? Color.Red : durumu == "Boş" ? Color.LightGreen : Color.Gray,
                Tag = masaId
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
            return masaButton;
        }


        public void LoadKatsAndMasas()
        {
            Kat kat = new Kat();
            kat.setKatID(1);
            kat.setKatAdi("KAT 1");
            kat.setDurumu("Temizlik");
            kat.setAciklama("Yere çorba döküldü.");
            Random rnd = new Random();
            List<Masa> masalar = new List<Masa>();
            for (int i = 1; i <= 10; i++)
            {
                Masa masa = new Masa();
                masa.setMasaID(i);
                int masaID = masa.getMasaID();
                masa.setMasaAdi("Masa " + masaID.ToString());
                masa.setSandalyeSayisi(rnd.Next(3, 6));
                masa.setKatID(kat.getKatID());
                string durum = (masa.getMasaID() % 4) == 0 ? "Dolu" : "Boş";
                masa.setDurumu(durum);
                kat.setMasalarID("M0" + masaID.ToString());
                masalar.Add(masa);
            }
            FlowLayoutPanel katPanel = CreateKatPanel(kat.getKatID(), kat.getKatAdi(), kat.getAciklama(), kat.getDurumu());

            // İlgili katın masalarını çek
            FlowLayoutPanel masaPanel = katPanel.Controls.OfType<FlowLayoutPanel>().FirstOrDefault();

            foreach (var masa in masalar)
            {
                Button masaButton = CreateMasaButton(masa.getMasaID(), masa.getMasaAdi(), masa.getSandalyeSayisi(), masa.getKatID(), masa.getDurumu());
                masaPanel.Controls.Add(masaButton);
            }

            flowLayoutPanel1.Controls.Add(katPanel);






            Kat kat1 = new Kat();
            kat1.setKatID(2);
            kat1.setKatAdi("KAT 2");
            kat1.setDurumu("Aktif");
            kat1.setAciklama("");
            List<Masa> masalar1 = new List<Masa>();
            for (int i = 1; i <= 10; i++)
            {
                Masa masa = new Masa();
                masa.setMasaID(i);
                int masaID = masa.getMasaID();
                masa.setMasaAdi("Masa " + masaID.ToString());
                masa.setSandalyeSayisi(rnd.Next(3, 6));
                masa.setKatID(kat1.getKatID());
                string durum = (masa.getMasaID() % 3) == 0 ? "Dolu" : "Boş";
                masa.setDurumu(durum);
                kat1.setMasalarID("M0" + masaID.ToString());
                masalar1.Add(masa);
            }
            FlowLayoutPanel katPanel1 = CreateKatPanel(kat1.getKatID(), kat1.getKatAdi(), kat1.getAciklama(), kat1.getDurumu());

            // İlgili katın masalarını çek
            FlowLayoutPanel masaPanel1 = katPanel1.Controls.OfType<FlowLayoutPanel>().FirstOrDefault();

            foreach (var masa in masalar1)
            {
                Button masaButton = CreateMasaButton(masa.getMasaID(), masa.getMasaAdi(), masa.getSandalyeSayisi(), masa.getKatID(), masa.getDurumu());
                masaPanel1.Controls.Add(masaButton);
            }

            flowLayoutPanel1.Controls.Add(katPanel1);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadKatsAndMasas();
        }

    }

    public class Kat
    {
        private int katID;
        public int getKatID() { return this.katID; }
        public void setKatID(int katID) { this.katID = katID; }

        private string katAdi;
        public string getKatAdi() { return this.katAdi; }
        public void setKatAdi(string katAdi) { this.katAdi = katAdi; }

        private string[] masalarID;
        public string[] getMasalarID() { return this.masalarID; }
        public void setMasalarID(string masaID) { try { this.masalarID.Append(masaID); } catch (Exception hata) { Console.WriteLine("HATA: " + hata.ToString()); } }

        private string durumu;
        public string getDurumu() { return this.durumu; }
        public void setDurumu(string durumu) { this.durumu = durumu; }

        private string aciklama;
        public string getAciklama() { return this.aciklama; }
        public void setAciklama(string aciklama) { this.aciklama = aciklama; }

    }

    public class Masa
    {
        private int masaID;
        public int getMasaID() { return this.masaID; }
        public void setMasaID(int masaID) { this.masaID = masaID; }

        private string masaAdi;
        public string getMasaAdi() { return this.masaAdi; }
        public void setMasaAdi(string masaAdi) { this.masaAdi = masaAdi; }

        private int sandalyeSayisi;
        public int getSandalyeSayisi() { return this.sandalyeSayisi; }
        public void setSandalyeSayisi(int sandalyeSayisi) { this.sandalyeSayisi = sandalyeSayisi; }

        private int katID;
        public int getKatID() { return this.katID; }
        public void setKatID(int katID) { this.katID = katID; }

        private string durumu;
        public string getDurumu() { return this.durumu; }
        public void setDurumu(string durumu) { this.durumu = durumu; }
    }
}
