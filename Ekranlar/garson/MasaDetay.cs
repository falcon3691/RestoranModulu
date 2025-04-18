using RestoranModulu.Ekranlar.Kasa;
using System;
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
        string adi, masaDurumu, aciklama, siparisDurumu = null;
        int masaID, sandalyeSayisi, kullaniciID = 0;

        public MasaDetay(int masaID, int kullaniciID, int rolID)
        {
            InitializeComponent();
            degerAtama(masaID);
            ekranDoldurma();
            this.kullaniciID = kullaniciID;
            urunleriListele(masaID);
            if (rolID == 2)
                button3.Visible = true;
            if (dataGridView1.Columns.Contains("siparisDetayID"))
            {
                dataGridView1.Columns["siparisDetayID"].Visible = false;
            }
        }

        // Ödeme Yap butonu
        private void button3_Click(object sender, System.EventArgs e)
        {
            Odeme form = new Odeme(masaID);
            this.SuspendLayout();
            form.ShowDialog();
            urunleriListele(masaID);
        }

        // Masa Birleştir butonu
        private void button4_Click(object sender, EventArgs e)
        {
            // masaID değeri ile masa grup işlemlerinin yapılacağı ekrana geçiş yapılır.
            MasaGrup form = new MasaGrup(this.masaID, this.adi);
            this.SuspendLayout();
            form.ShowDialog();
        }

        // Sipariş Ver butonu
        private void button1_Click(object sender, System.EventArgs e)
        {
            vtSiparis.siparisEkle(masaID, kullaniciID);
            UrunSecme form = new UrunSecme(masaID, kullaniciID);
            this.SuspendLayout();
            form.ShowDialog();
            urunleriListele(masaID);

        }

        // Geri Dön butonu
        private void button2_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        public void degerAtama(int masaID)
        {
            DataTable dt = vtMasa.masaFiltrele(masaID, null, 0, null, 0);
            if (dt != null)
            {
                var satir = dt.Rows[0];
                this.masaID = Convert.ToInt32(satir[0]);
                this.adi = satir[1].ToString() ?? "";
                this.sandalyeSayisi = Convert.ToInt32(satir[2]);
                this.masaDurumu = satir[3].ToString() ?? "";
                this.aciklama = satir[4].ToString() ?? "";
            }
        }

        public void urunleriListele(int masaID)
        {
            int toplamTutar = 0;
            DataTable dt = new DataTable();
            foreach (DataRow siparis in vtSiparis.Listele2(masaID).Rows)
            {
                // Ödenmemiş siparişlerin listesi oluşturuluyor.
                dt.Merge(vtSiparis.detayListele(Convert.ToInt32(siparis[0]), true));
            }

            dataGridView1.DataSource = dt;
            if (dataGridView1.Columns.Contains("urunID"))
            {
                dataGridView1.Columns["urunID"].Visible = false;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["toplamFiyat"].Value != null)
                    toplamTutar += Convert.ToInt32(row.Cells["toplamFiyat"].Value);
            }
            label5.Text = toplamTutar.ToString();
        }

        public void ekranDoldurma()
        {
            label1.Text = adi ?? "";
            label3.Text = sandalyeSayisi.ToString() ?? "0";
            if (masaDurumu == "boş")
                pictureBox1.BackColor = Color.LightGreen;
            else if (masaDurumu == "dolu")
                pictureBox1.BackColor = Color.Red;
            else if (masaDurumu == "rezerve")
                pictureBox1.BackColor = Color.Yellow;

            DataTable dt = vtSiparis.enSonSiparisiBul(masaID);
            if (dt.Rows.Count > 0)
            {
                siparisDurumu = dt.Rows[0][0].ToString() ?? "";
                if (siparisDurumu == "bekliyor")
                    pictureBox2.BackColor = Color.Red;
                else if (siparisDurumu == "hazırlanıyor")
                    pictureBox2.BackColor = Color.Yellow;
                else if (siparisDurumu == "tamamlandı")
                    pictureBox2.BackColor = Color.LightGreen;
                else if (siparisDurumu == "ödendi")
                    pictureBox2.BackColor = Color.LightGray;
            }
        }
    }
}
