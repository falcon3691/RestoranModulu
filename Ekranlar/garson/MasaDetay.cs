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
        string adi, masaDurumu, aciklama = null;
        int masaID, sandalyeSayisi, katID, kullaniciID = 0;

        public MasaDetay(int masaID, int kullaniciID)
        {
            InitializeComponent();
            degerAtama(masaID);
            ekranDoldurma();
            this.kullaniciID = kullaniciID;
            urunleriListele(masaID);
        }

        // Sipariş Ver butonu
        private void button1_Click(object sender, System.EventArgs e)
        {
            if (vtSiparis.siparisEkle(masaID, kullaniciID))
            {
                UrunSecme form = new UrunSecme(masaID, kullaniciID);
                this.SuspendLayout();
                form.ShowDialog();
                urunleriListele(masaID);
            }
        }

        // Geri Dön butonu
        private void button2_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        public void degerAtama(int masaID)
        {
            DataTable dt = vtMasa.masaFiltrele(masaID);
            if (dt != null)
            {
                this.masaID = int.Parse(dt.Rows[0][0].ToString());
                this.adi = dt.Rows[0][1].ToString();
                this.sandalyeSayisi = int.Parse(dt.Rows[0][2].ToString());
                this.masaDurumu = dt.Rows[0][3].ToString();
                this.aciklama = dt.Rows[0][4].ToString();
                this.katID = int.Parse(dt.Rows[0][5].ToString());
            }
        }


        public void urunleriListele(int masaID)
        {
            int toplamTutar = 0;
            DataTable dt2 = new DataTable();
            foreach (DataRow siparis in vtSiparis.Listele2(masaID).Rows)
            {
                dt2.Merge(vtSiparis.detayListele(int.Parse(siparis[0].ToString())));
            }

            dataGridView1.DataSource = dt2;
            if (dataGridView1.Columns.Contains("urunID"))
            {
                dataGridView1.Columns["urunID"].Visible = false;
            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["toplamFiyat"].Value != null)
                    toplamTutar += int.Parse(row.Cells["toplamFiyat"].Value.ToString());
            }
            label5.Text = toplamTutar.ToString();
        }
        public void ekranDoldurma()
        {
            label1.Text = adi;
            label3.Text = sandalyeSayisi.ToString();
            if (masaDurumu == "boş")
                pictureBox1.BackColor = Color.Green;
            else if (masaDurumu == "dolu")
                pictureBox1.BackColor = Color.DarkGray;

        }
    }
}
