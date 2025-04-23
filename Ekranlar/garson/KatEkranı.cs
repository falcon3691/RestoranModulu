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

        public FlowLayoutPanel CreateKatPanel(Kat kat)
        {
            FlowLayoutPanel katPanel = new FlowLayoutPanel
            {
                Width = 660,
                Height = 350,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                Tag = kat,
            };

            // Kat Adı
            Label lblKatAdi = new Label
            {
                Text = kat.KatAdi,
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
                BackColor = kat.Durumu == "Aktif" ? Color.Green : kat.Durumu == "Temizlik" ? Color.Red : Color.Gray,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Açıklama Kutusu
            Label lblAciklama = new Label
            {
                Text = string.IsNullOrEmpty(kat.Aciklama) ? "Açıklama yok." : kat.Aciklama,
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

        public Button CreateMasaButton(Masa masa)
        {
            Button masaButton = new Button
            {
                Width = 120,
                Height = 120,
                Text = $"{masa.MasaAdi}\nSandalyeler: {masa.SandalyeSayisi}" + ((masa.grupID != 0) ? $"\nGrubu: {masa.grupID}" : ""),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = masa.Durumu == "dolu" ? Color.Red : masa.Durumu == "boş" ? Color.LightGreen : masa.Durumu == "rezerve" ? Color.Yellow : Color.Gray,
                Tag = masa,
            };

            /*
             PictureBox masaResmi = new PictureBox
            {
                Image = Image.FromFile("masa.png"),
                Width = 100,
                Height = 100,
                Location = new Point(10, 20),
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            masaButton.Controls.Add(masaResmi);
            */
            return masaButton;
        }

        private void MasaButton_Click(object sender, EventArgs e)
        {
            Button tiklananButon = sender as Button;
            if (tiklananButon != null)
            {
                Masa masa = tiklananButon.Tag as Masa;
                MasaDetay detayForm = new MasaDetay(masa.MasaID, kullaniciID, rolID);
                this.SuspendLayout();
                detayForm.ShowDialog();
                LoadKatsAndMasas();
            }
        }

        public void LoadKatsAndMasas()
        {
            flowLayoutPanel1.Controls.Clear();
            DataTable katlar = vtKat.Listele();
            for (int i = 0; i < katlar.Rows.Count; i++)
            {
                var katSatir = katlar.Rows[i];

                Kat kat = new Kat(
                    Convert.ToInt32(katSatir["katID"]),
                    katSatir["adi"].ToString() ?? "",
                    katSatir["durumu"].ToString() ?? "",
                    katSatir["aciklama"].ToString() ?? ""
                );

                FlowLayoutPanel katPanel = CreateKatPanel(kat);
                FlowLayoutPanel masaPanel = katPanel.Controls.OfType<FlowLayoutPanel>().FirstOrDefault();

                DataTable masalar = vtMasa.Listele(true, kat.KatID);
                for (int x = 0; x < masalar.Rows.Count; x++)
                {
                    var masaSatir = masalar.Rows[x];

                    Masa masa = new Masa(
                        Convert.ToInt32(masaSatir["masaID"]),
                        masaSatir["adi"].ToString() ?? "",
                        Convert.ToInt32(masaSatir["sandalyeSayisi"]),
                        Convert.ToInt32(masaSatir["katID"]),
                        masaSatir["masaDurumu"].ToString() ?? "",
                        masaSatir["aciklama"].ToString() ?? "",
                        (!string.IsNullOrEmpty(masaSatir["grupID"].ToString())) ? Convert.ToInt32(masaSatir["grupID"]) : 0
                    );

                    Button masaButton = CreateMasaButton(masa);
                    masaButton.Click += MasaButton_Click;
                    masaPanel.Controls.Add(masaButton);
                }
                flowLayoutPanel1.Controls.Add(katPanel);
            }
        }
    }
    public class Kat
    {
        public int KatID { get; }

        public string KatAdi { get; }

        public string Durumu { get; }

        public string Aciklama { get; }

        public Kat(int katID, string katAdi, string durumu, string aciklama)
        {
            this.KatID = katID;
            this.KatAdi = katAdi;
            this.Durumu = durumu;
            this.Aciklama = aciklama;
        }
    }

    public class Masa
    {
        public int MasaID { get; }

        public string MasaAdi { get; }

        public int SandalyeSayisi { get; }

        public int KatID { get; }

        public string Durumu { get; }

        public string Aciklama { get; }

        public int grupID { get; }

        public Masa(int masaID, string masaAdi, int sandalyeSayisi, int katID, string durumu, string aciklama, int grupID)
        {
            this.MasaID = masaID;
            this.MasaAdi = masaAdi;
            this.SandalyeSayisi = sandalyeSayisi;
            this.KatID = katID;
            this.Durumu = durumu;
            this.Aciklama = aciklama;
            if (grupID != 0)
                this.grupID = grupID;
        }
    }
}
