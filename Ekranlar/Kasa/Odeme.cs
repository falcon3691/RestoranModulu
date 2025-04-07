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
        int masaID, sandalyeSayisi, kullaniciID = 0;
        int secilenTutar, kalanTutar, toplamTutar, odenenTutar, paraUstu = 0;
        bool hepsiOdendiMi = false;
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
                int siparisDetayID = int.Parse(row.Cells["siparisDetayID"].Value.ToString());
                vtSiparis.siparisDetayGuncelle(siparisDetayID, "ödendi");
                secilenDetayID.Clear();
            }
            DataTable dt2 = new DataTable();
            foreach (DataRow siparis in vtSiparis.Listele2(masaID).Rows)
            {
                dt2.Merge(vtSiparis.detayListele(int.Parse(siparis[0].ToString()), true));
            }
            dataGridView1.DataSource = dt2;
            degerAtama(masaID);
            urunleriListele(masaID);
            ekranDoldurma();
        }

        // Seçilenleri Öde butonu
        private void button2_Click(object sender, EventArgs e)
        {
            int i = 1;
            foreach (siparisDetaylari siparis in secilenDetayID)
            {
                Console.WriteLine("Satır " + i.ToString() + " | " + siparis.siparisDetayID.ToString() + " | " + siparis.miktar.ToString() + " | " + siparis.toplamTutar.ToString());
                i++;
            }
            // kalanTutar-secilenTutar
            kalanTutar -= secilenTutar;
            secilenTutar = 0;
            ekranDoldurma();
            DataTable dt2 = new DataTable();
            List<siparisDetaylari> silinecekler = new List<siparisDetaylari>();
            // Sipariş detayının durumu ödendi olarak değiştirilecek
            foreach (siparisDetaylari siparisDetay in secilenDetayID)
            {
                int siparisDetayID = siparisDetay.siparisDetayID;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    int satirID = int.Parse(row.Cells["siparisDetayID"].Value.ToString());
                    if (siparisDetayID == satirID)
                    {
                        int miktar = int.Parse(row.Cells["miktar"].Value.ToString());
                        int toplamFiyat = siparisDetay.toplamTutar;
                        int birimFiyat = int.Parse(row.Cells["birimFiyat"].Value.ToString());
                        if (miktar == 0)
                        {
                            vtSiparis.siparisDetayGuncelle(siparisDetayID, "ödendi");
                            silinecekler.Add(siparisDetay);
                        }
                    }
                }
            }

            if (silinecekler.Count > 0)
            {
                foreach (siparisDetaylari siparisDetay in silinecekler)
                    secilenDetayID.Remove(siparisDetay);
            }

            foreach (DataRow siparis in vtSiparis.Listele2(masaID).Rows)
            {
                dt2.Merge(vtSiparis.detayListele(int.Parse(siparis[0].ToString()), true));
            }
            dataGridView1.DataSource = dt2;

            // Sipariş listesi güncellenir. Sadece ödenmemiş olanlar listelenecek.
            foreach (siparisDetaylari siparisDetay in secilenDetayID)
            {
                int siparisDetayID = siparisDetay.siparisDetayID;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    int satirID = int.Parse(row.Cells["siparisDetayID"].Value.ToString());
                    if (siparisDetayID == satirID)
                    {
                        int miktar = int.Parse(row.Cells["miktar"].Value.ToString()) - siparisDetay.miktar;
                        int toplamFiyat = siparisDetay.toplamTutar;
                        int birimFiyat = int.Parse(row.Cells["birimFiyat"].Value.ToString());


                        dataGridView1.Rows[row.Index].Cells["miktar"].Value = miktar;
                        dataGridView1.Rows[row.Index].Cells["toplamFiyat"].Value = birimFiyat * miktar;

                    }
                }

            }




            // Masaya ait siparişlerin, detaylar listesindeki tüm ürünleri ödenmiş ise, durumu güncellenir.

            // Toplam Tutar ödendiyse masayı kapatma işlemlerine başla.
            if (kalanTutar == 0)
                hepsiOdendiMi = true;
        }

        // Seçilenleri Kaldır butonu
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (siparisDetaylari siparis in secilenDetayID)
            {
                int siparisDetayID = siparis.siparisDetayID;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    int satirID = int.Parse(row.Cells["siparisDetayID"].Value.ToString());
                    if (siparisDetayID == satirID)
                    {
                        row.Cells["miktar"].Value = siparis.miktar;
                        row.Cells["toplamFiyat"].Value = siparis.toplamTutar;
                        break;
                    }
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
                    paraUstu = secilenTutar - int.Parse(textBox4.Text.ToString());
                    if (paraUstu < 0)
                        textBox5.Text = ((-1) * paraUstu).ToString();
                    else if (paraUstu >= 0)
                        textBox5.Text = 0.ToString();
                }
                catch (FormatException hata)
                {
                    Console.WriteLine("Değer olarak sayı girilmeli. HATA: " + hata.ToString());
                }
            }
            else
                textBox5.Text = 0.ToString();
        }

        // Sipariş listesindeki ürünleri tek tek ödemek için seçme işlemi yapılır.
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0)
            {
                int birimFiyat = int.Parse(dataGridView1.Rows[satirNo].Cells["birimFiyat"].Value.ToString());
                int toplamFiyat = int.Parse(dataGridView1.Rows[satirNo].Cells["toplamFiyat"].Value.ToString());
                int siparisDetayID = int.Parse(dataGridView1.Rows[satirNo].Cells["siparisDetayID"].Value.ToString());
                int miktar = int.Parse(dataGridView1.Rows[satirNo].Cells["miktar"].Value.ToString());
                if (miktar > 0)
                {
                    siparisDetaylari siparisDetayi = new siparisDetaylari();
                    siparisDetayi.siparisDetayID = siparisDetayID;
                    if (!secilenDetayID.Any(x => x.siparisDetayID == siparisDetayi.siparisDetayID))
                    {

                        siparisDetayi.miktar = 1;
                        siparisDetayi.toplamTutar = birimFiyat;
                        secilenDetayID.Add(siparisDetayi);
                    }
                    else
                    {
                        foreach (siparisDetaylari siparis in secilenDetayID)
                        {
                            if (siparis.siparisDetayID == siparisDetayID)
                            {
                                siparis.miktar += 1;
                                siparis.toplamTutar = siparis.miktar * birimFiyat;
                            }
                        }
                    }
                    secilenTutar += birimFiyat;
                    dataGridView1.Rows[satirNo].Cells["toplamFiyat"].Value = toplamFiyat - birimFiyat;
                    dataGridView1.Rows[satirNo].Cells["miktar"].Value = miktar - 1;
                }
            }
            ekranDoldurma();
        }

        public void urunleriListele(int masaID)
        {
            int toplamTutar = 0;
            DataTable dt2 = new DataTable();
            foreach (DataRow siparis in vtSiparis.Listele2(masaID).Rows)
            {
                dt2.Merge(vtSiparis.detayListele(int.Parse(siparis[0].ToString()), true));
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
                    toplamTutar += int.Parse(row.Cells["toplamFiyat"].Value.ToString());
            }

            this.toplamTutar = toplamTutar;
            this.kalanTutar = toplamTutar;
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
            if (masaDurumu == "boş")
                pictureBox1.BackColor = Color.Green;
            else if (masaDurumu == "dolu")
                pictureBox1.BackColor = Color.DarkGray;

        }
    }

    public class siparisDetaylari
    {
        public int siparisDetayID;
        public int miktar;
        public int toplamTutar;

    }
}
