using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.Kasa
{
    public partial class Odeme : Form
    {
        VTMasa vtMasa = new VTMasa();
        VTSiparisler vtSiparis = new VTSiparisler();

        string adi, masaDurumu, aciklama = null;
        int masaID, kullaniciID = 0;

        int secilenTutar, kalanTutar, toplamTutar, odenenTutar, paraUstu = 0;
        List<siparisDetaylari> secilenDetayID = new List<siparisDetaylari>();

        public Odeme(int masaID)
        {
            InitializeComponent();
            degerAtama(masaID);
            urunleriListele(masaID);
            ekranDoldurma();
        }

        // Hepsini Öde butonu
        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int siparisDetayID = Convert.ToInt32(row.Cells["siparisDetayID"].Value);
                vtSiparis.siparisDetayGuncelle(siparisDetayID, "ödendi");
            }
            secilenDetayID.Clear();

            foreach (DataRow siparis in vtSiparis.Listele2(masaID).Rows)
            {
                vtSiparis.siparisGuncelle(Convert.ToInt32(siparis["siparisID"]), toplamTutar, "ödendi");
            }
            this.Close();
        }

        // Seçilenleri Öde butonu
        private void button2_Click(object sender, EventArgs e)
        {
            kalanTutar -= secilenTutar;
            secilenTutar = 0;
            ekranDoldurma();

            List<siparisDetaylari> silinecekler = new List<siparisDetaylari>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int satirID = Convert.ToInt32(row.Cells["siparisDetayID"].Value);
                int miktar = Convert.ToInt32(row.Cells["miktar"].Value);

                if (miktar != 0)
                    continue;

                var siparisDetay = secilenDetayID.FirstOrDefault(x => x.siparisDetayID == satirID);

                if (siparisDetay != null)
                {
                    vtSiparis.siparisDetayGuncelle(satirID, "ödendi");
                    silinecekler.Add(siparisDetay);
                }
            }

            if (silinecekler.Count > 0)
            {
                foreach (siparisDetaylari siparisDetay in silinecekler)
                    secilenDetayID.Remove(siparisDetay);
            }
            /*
            DataTable dt = new DataTable();
            foreach (DataRow siparis in vtSiparis.Listele2(masaID).Rows)
            {
                dt.Merge(vtSiparis.detayListele(Convert.ToInt32(siparis[0]), true));
            }
            dataGridView1.DataSource = dt;*/
            urunleriListele(masaID);

            foreach (siparisDetaylari siparisDetay in secilenDetayID)
            {
                int siparisDetayID = siparisDetay.siparisDetayID;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    int satirID = Convert.ToInt32(row.Cells["siparisDetayID"].Value);
                    if (siparisDetayID == satirID)
                    {
                        int miktar = Convert.ToInt32(row.Cells["miktar"].Value) - siparisDetay.miktar;
                        int toplamFiyat = siparisDetay.toplamTutar;
                        int birimFiyat = Convert.ToInt32(row.Cells["birimFiyat"].Value);

                        row.Cells["miktar"].Value = miktar;
                        row.Cells["toplamFiyat"].Value = birimFiyat * miktar;
                    }
                }
            }

            // Masaya ait siparişlerin, detaylar listesindeki tüm ürünleri ödenmiş ise, durumu güncellenir.
            foreach (DataRow siparis in vtSiparis.Listele2(masaID).Rows)
            {
                var detaylar = vtSiparis.detayListele(Convert.ToInt32(siparis["siparisID"]), true);
                if (detaylar.Rows.Count == 0)
                {
                    vtSiparis.siparisGuncelle(Convert.ToInt32(siparis["siparisID"]), Convert.ToInt32(siparis["toplamFiyat"]), "ödendi");
                }
            }

            // Toplam Tutar ödendiyse masayı kapatma işlemlerine başla.
            if (kalanTutar == 0)
            {
                MessageBox.Show("Tüm hesap ödendi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }

        // Seçilenleri Kaldır butonu
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var siparis in secilenDetayID)
            {
                var satir = dataGridView1.Rows
                    .Cast<DataGridViewRow>()
                    .FirstOrDefault(r => Convert.ToInt32(r.Cells["siparisDetayID"].Value) == siparis.siparisDetayID);

                if (satir != null)
                {
                    satir.Cells["miktar"].Value = siparis.miktar;
                    satir.Cells["toplamFiyat"].Value = siparis.toplamTutar;
                }
            }
            secilenDetayID.Clear();
            secilenTutar = 0;
            ekranDoldurma();
        }

        // Ödenen Miktar değeri değiştiğinde otomatik olarak para üstünü hesaplar
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox4.Text))
            {
                try
                {
                    paraUstu = Convert.ToInt32(textBox4.Text) - secilenTutar;
                    if (paraUstu < 0)
                        textBox5.Text = 0.ToString();
                    else
                        textBox5.Text = paraUstu.ToString();
                }
                catch (FormatException hata)
                {
                    Console.WriteLine("Değer olarak sayı girilmeli. HATA: " + hata.ToString());
                }
            }
            else
                textBox5.Text = 0.ToString();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo < 0) return;

            var satir = dataGridView1.Rows[satirNo];

            int birimFiyat = Convert.ToInt32(satir.Cells["birimFiyat"].Value);
            int toplamFiyat = Convert.ToInt32(satir.Cells["toplamFiyat"].Value);
            int siparisDetayID = Convert.ToInt32(satir.Cells["siparisDetayID"].Value);
            int miktar = Convert.ToInt32(satir.Cells["miktar"].Value);

            if (miktar <= 0) return;

            var siparis = secilenDetayID.FirstOrDefault(x => x.siparisDetayID == siparisDetayID);
            if (siparis == null)
            {
                siparis = new siparisDetaylari
                {
                    siparisDetayID = siparisDetayID,
                    miktar = 1,
                    toplamTutar = birimFiyat
                };
                secilenDetayID.Add(siparis);
            }
            else
            {
                siparis.miktar += 1;
                siparis.toplamTutar = siparis.miktar * birimFiyat;
            }

            secilenTutar += birimFiyat;

            satir.Cells["toplamFiyat"].Value = toplamFiyat - birimFiyat;
            satir.Cells["miktar"].Value = miktar - 1;
            ekranDoldurma();
        }

        public void urunleriListele(int masaID)
        {
            int toplamTutar = 0;
            DataTable dt2 = new DataTable();
            int grupIDDegeri = vtMasa.grupDegeriniBul(masaID);
            if (grupIDDegeri > 0)
            {
                foreach (DataRow masa in vtMasa.grubuListele(grupIDDegeri).Rows)
                {
                    int masaIDDegeri = Convert.ToInt32(masa["masaID"]);
                    foreach (DataRow siparis in vtSiparis.Listele2(masaIDDegeri).Rows)
                    {
                        dt2.Merge(vtSiparis.detayListele(Convert.ToInt32(siparis[0]), true));
                    }
                }
            }
            else
            {
                foreach (DataRow siparis in vtSiparis.Listele2(masaID).Rows)
                {
                    dt2.Merge(vtSiparis.detayListele(Convert.ToInt32(siparis[0]), true));
                }
            }

            dataGridView1.DataSource = dt2;
            if (dataGridView1.Columns.Contains("urunID"))
            {
                dataGridView1.Columns["urunID"].Visible = false;
            }

            if (dataGridView1.Columns.Contains("siparisDetayID"))
            {
                dataGridView1.Columns["siparisDetayID"].Visible = false;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["toplamFiyat"].Value != null)
                    toplamTutar += Convert.ToInt32(row.Cells["toplamFiyat"].Value);
            }

            this.toplamTutar = toplamTutar;
            this.kalanTutar = toplamTutar;
        }

        public void degerAtama(int masaID)
        {
            DataTable dt = vtMasa.masaFiltrele(masaID, null, 0, null, 0);
            if (dt.Rows.Count > 0)
            {
                var satir = dt.Rows[0];
                this.masaID = Convert.ToInt32(satir[0]);
                this.adi = satir[1].ToString() ?? "";
                this.masaDurumu = satir[3].ToString() ?? "";
                this.aciklama = satir[4].ToString() ?? "";
            }
        }

        public void ekranDoldurma()
        {
            label1.Text = adi;
            textBox1.Text = secilenTutar.ToString();
            textBox2.Text = kalanTutar.ToString();
            textBox3.Text = toplamTutar.ToString();
            textBox4.Text = odenenTutar.ToString();
            textBox5.Text = paraUstu.ToString();

            bool odenDiMi = true;
            DataTable dt = new DataTable();
            foreach (DataRow siparis in vtSiparis.Listele2(masaID).Rows)
            {
                if (siparis["durumu"] != null && siparis["durumu"].ToString() != "ödendi")
                {
                    dt.Merge(vtSiparis.detayListele(Convert.ToInt32(siparis[0]), true));
                    pictureBox1.BackColor = Color.Red;
                    odenDiMi = false;
                }
            }
            if (odenDiMi)
                pictureBox1.BackColor = Color.LightGreen;

        }
    }

    public class siparisDetaylari
    {
        public int siparisDetayID;
        public int miktar;
        public int toplamTutar;
    }
}
