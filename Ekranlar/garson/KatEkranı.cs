using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.garson
{
    public partial class KatEkranı : Form
    {

        VTKatlar vtKat = new VTKatlar();
        VTMasa vtMasa = new VTMasa();

        int rolID, kullaniciID = 0;
        public KatEkranı(int kullaniciID, int rolID)
        {
            InitializeComponent();
            LoadKatsAndMasas();
            this.kullaniciID = kullaniciID;
            this.rolID = rolID;
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
                BackColor = durumu == "dolu" ? Color.Red : durumu == "boş" ? Color.LightGreen : Color.Gray,
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

        private void MasaButton_Click(object sender, EventArgs e)
        {
            Button tiklananButon = sender as Button;
            if (tiklananButon != null)
            {
                int masaID = (int)tiklananButon.Tag;

                // Yeni ekranı aç, masaID'yi parametre olarak gönder
                MasaDetay detayForm = new MasaDetay(masaID, kullaniciID, rolID);
                this.SuspendLayout();
                detayForm.ShowDialog(); // veya Show(), ihtiyaca göre
            }
        }

        public void LoadKatsAndMasas()
        {
            // Veri tabanında kayıtlı olan katların listesi alınır ve her satırı ile ayrı ayrı işlem yapılmak üzere döngüye sokulur.
            DataTable katlar = vtKat.Listele();
            for (int i = 0; i < katlar.Rows.Count; i++)
            {
                // Yeni bir Kat objesi oluşturulur ve özellikleri belirlenmek üzere döngüye sokulur.
                Kat kat = new Kat();
                for (int j = 0; j < katlar.Columns.Count; j++)
                {
                    if (j == 0)
                        kat.setKatID(int.Parse(katlar.Rows[i][j].ToString()));  // Veri tabanından, "katID" değeri alınır ve Katın katID özelliği olarak atanır.
                    else if (j == 1)
                        kat.setKatAdi(katlar.Rows[i][j].ToString());            // Veri tabanından, "adi" değeri alınır ve Katın katAdi özelliği olarak atanır.
                    else if (j == 2)
                        kat.setDurumu(katlar.Rows[i][j].ToString());            // Veri tabanından, "durumu" değeri alınır ve Katın durumu özelliği olarak atanır.
                    else if (j == 3)
                        kat.setAciklama(katlar.Rows[i][j].ToString());          // Veri tabanından, "aciklama" değeri alınır ve Katın aciklama özelliği olarak atanır.
                }

                // Oluşturulan katın özellikleri ile yeni bir Kat Paneli oluşturulur.
                FlowLayoutPanel katPanel = CreateKatPanel(kat.getKatID(), kat.getKatAdi(), kat.getAciklama(), kat.getDurumu());

                // Kat içinde bulunan masaları yerleştirmek için yeni bir Masa Paneli oluşturulur.
                FlowLayoutPanel masaPanel = katPanel.Controls.OfType<FlowLayoutPanel>().FirstOrDefault();

                // Oluşturulan kat objesinin "katID" değerine sahip masaların listesi alınır ve
                // her satırı ile ayrı ayrı işlem yapılmak üzere döngüye sokulur.
                DataTable masalar = vtMasa.Listele(true, kat.getKatID());
                for (int x = 0; x < masalar.Rows.Count; x++)
                {
                    // Yeni bir Masa objesi oluşturulur.
                    Masa masa = new Masa();
                    for (int y = 0; y < masalar.Columns.Count; y++)
                    {
                        if (y == 0)
                            masa.setMasaID(int.Parse(masalar.Rows[x][y].ToString()));           // Veri tabanından, "masaID" değeri alınır ve Masanın masaID özelliği olarak atanır.
                        else if (y == 1)
                            masa.setMasaAdi(masalar.Rows[x][y].ToString());                     // Veri tabanından, "adi" değeri alınır ve Masanın masaAdi özelliği olarak atanır.
                        else if (y == 2)
                            masa.setSandalyeSayisi(int.Parse(masalar.Rows[x][y].ToString()));   // Veri tabanından, "sandalyeSayisi" değeri alınır ve Masanın sandalyeSayisi özelliği olarak atanır.
                        else if (y == 3)
                            masa.setDurumu(masalar.Rows[x][y].ToString());                      // Veri tabanından, "masaDurumu" değeri alınır ve Masanın durumu özelliği olarak atanır.
                        else if (y == 4)
                            masa.setAciklama(masalar.Rows[x][y].ToString());                    // Veri tabanından, "aciklama" değeri alınır ve Masanın aciklama özelliği olarak atanır.
                        else if (y == 5)
                            masa.setKatID(int.Parse(masalar.Rows[x][y].ToString()));            // Veri tabanından, "katID" değeri alınır ve Masanın katID özelliği olarak atanır.
                    }

                    // Oluşturulan masanın özellikleri ile yeni bir masaButton oluşturulur.
                    Button masaButton = CreateMasaButton(masa.getMasaID(), masa.getMasaAdi(), masa.getSandalyeSayisi(), masa.getKatID(), masa.getDurumu());

                    masaButton.Click += MasaButton_Click;
                    // ID değeri Tag içine atanıyor
                    masaButton.Tag = masa.getMasaID();
                    // Oluşturulan masaButton, daha önce oluşturulan Masa Panelinin içine yerleştirilir.
                    masaPanel.Controls.Add(masaButton);
                }

                // Tasarımı tamamlanan kat ekranda gösterilir. 
                flowLayoutPanel1.Controls.Add(katPanel);
            }

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

        private string aciklama;
        public string getAciklama() { return this.aciklama; }
        public void setAciklama(string aciklama) { this.aciklama = durumu; }
    }

}
