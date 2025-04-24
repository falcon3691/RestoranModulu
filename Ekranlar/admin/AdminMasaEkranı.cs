using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminMasaEkranı : Form
    {
        VTMasa vt = new VTMasa();

        // Masa değişken bilgileri.
        string adi, masaDurumu, aciklama = null;
        int masaID, sandalyeSayisi, katID = 0;

        public AdminMasaEkranı(int katID = 0)
        {
            InitializeComponent();

            if (katID != 0)
            {
                this.katID = katID;
                Panel[] aktifPaneller = new[] { panel4, panel6 };
                Panel[] pasifPaneller = new[] { panel1, panel3, panel5 };

                foreach (var panel in aktifPaneller.Concat(pasifPaneller))
                    panel.Parent = this;

                foreach (var panel in aktifPaneller)
                {
                    panel.Visible = true;
                    panel.Enabled = true;
                }

                foreach (var panel in pasifPaneller)
                {
                    panel.Visible = false;
                    panel.Enabled = false;
                }

                dataGridView2.DataSource = vt.Listele(true);
                dataGridView3.DataSource = vt.Listele(false, katID);
            }
            else
                dataGridView1.DataSource = vt.Listele();
        }

        // Yeni Masa Ekle butonu
        private void button1_Click(object sender, System.EventArgs e)
        {
            string mesaj = boslukKontrolu();
            if (mesaj == null)
            {
                degerAtama();
                vt.masaEkle(adi, sandalyeSayisi, masaDurumu, katID, aciklama);

                if (panel4.Visible == true)
                {
                    dataGridView2.DataSource = vt.Listele();
                    dataGridView3.DataSource = vt.Listele(false, katID);
                }
                else if (panel1.Visible == true)
                    dataGridView1.DataSource = vt.Listele();

                temizle();
            }
            else
                MessageBox.Show(mesaj, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Masayı Güncelle butonu
        private void button2_Click(object sender, System.EventArgs e)
        {
            if (masaID > 0)
            {
                degerAtama();
                vt.masaGuncelle(masaID, adi, masaDurumu, aciklama, sandalyeSayisi, katID);
                dataGridView1.DataSource = vt.Listele();
                temizle();
            }
            else
                MessageBox.Show("Masalar listesi içinden bir masa şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Masayı Sil butonu
        private void button3_Click(object sender, EventArgs e)
        {
            if (masaID > 0)
            {
                degerAtama();
                vt.masaSil(masaID);
                dataGridView1.DataSource = vt.Listele();
                temizle();
            }
            else
                MessageBox.Show("Masalar listesi içinden bir masa şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Hepsini Listele butonu
        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = vt.Listele();
            temizle();
        }

        // Boş Masaları Listele butonu
        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = vt.Listele(true);
            temizle();
        }

        // Masaları Filtrele butonu
        private void button6_Click(object sender, EventArgs e)
        {
            degerAtama();
            DataTable filtrelenmisListe = vt.masaFiltrele(0, adi, sandalyeSayisi, masaDurumu, katID);
            if (filtrelenmisListe.Rows.Count > 0)
                dataGridView1.DataSource = filtrelenmisListe ?? vt.Listele();
            else
            {
                MessageBox.Show("Girilen bilgilere göre bir masa bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridView1.DataSource = vt.Listele();
            }
        }

        // Masaları Temizle butonu
        private void button8_Click(object sender, EventArgs e)
        {
            if (katID > 0)
            {
                vt.masalariTemizle(katID);
                dataGridView2.DataSource = vt.Listele(true);
                dataGridView3.DataSource = vt.Listele(true, katID);
            }
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < (dataGridView1.Rows.Count - 1))
            {
                var satir = dataGridView1.Rows[satirNo];

                try
                {
                    masaID = Convert.ToInt32(satir.Cells[0].Value);
                    textBox1.Text = satir.Cells[1].Value?.ToString() ?? "";
                    textBox2.Text = satir.Cells[2].Value?.ToString() ?? "";
                    textBox6.Text = satir.Cells[3].Value?.ToString() ?? "";
                    if (panel3.Visible == true)
                    {
                        textBox3.Text = satir.Cells[4].Value?.ToString() ?? "";
                        textBox4.Text = satir.Cells[5].Value?.ToString() ?? "";
                    }
                    else if (panel4.Visible == true)
                    {
                        textBox5.Text = satir.Cells[4].Value?.ToString() ?? "";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Satır verileri alınırken bir hata oluştu: " + ex.Message);
                }
            }
        }

        // Boşta olan masalar listesi içinden seçilen masa, kata eklenir.
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < dataGridView2.Rows.Count)
            {
                var satir = dataGridView2.Rows[satirNo];

                try
                {
                    masaID = Convert.ToInt32(satir.Cells[0].Value);
                    adi = satir.Cells[1].Value?.ToString() ?? "";
                    sandalyeSayisi = Convert.ToInt32(satir.Cells[2].Value);
                    masaDurumu = satir.Cells[3].Value?.ToString() ?? "";
                    aciklama = satir.Cells[4].Value?.ToString() ?? "";

                    vt.masaGuncelle(masaID, adi, masaDurumu, aciklama, sandalyeSayisi, katID);
                    dataGridView2.DataSource = vt.Listele(true);
                    dataGridView3.DataSource = vt.Listele(true, katID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Satır verileri alınırken bir hata oluştu: " + ex.Message);
                }
            }
        }

        //Kata ait masalar listesinden seçilen masa, listeden çıkartılır.
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < dataGridView3.Rows.Count)
            {
                var satir = dataGridView3.Rows[satirNo];

                try
                {
                    masaID = Convert.ToInt32(satir.Cells[0].Value);
                    adi = satir.Cells[1].Value?.ToString() ?? "";
                    sandalyeSayisi = Convert.ToInt32(satir.Cells[2].Value);
                    masaDurumu = satir.Cells[3].Value?.ToString() ?? "";
                    aciklama = satir.Cells[4].Value?.ToString() ?? "";

                    vt.masaGuncelle(masaID, adi, masaDurumu, aciklama, sandalyeSayisi, 0);
                    dataGridView2.DataSource = vt.Listele(true);
                    dataGridView3.DataSource = vt.Listele(true, katID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Satır verileri alınırken bir hata oluştu: " + ex.Message);
                }
            }
        }

        public string boslukKontrolu()
        {
            string mesaj = null;
            List<string> hataMesajlari = new List<string>();
            if (string.IsNullOrEmpty(textBox1.Text))
                hataMesajlari.Add("Ad bilgisi boş bırakılamaz");
            if (string.IsNullOrEmpty(textBox2.Text))
                hataMesajlari.Add("Sandalye sayısı belirtilmeli");
            if (hataMesajlari.Count > 0)
                mesaj = string.Join(Environment.NewLine, hataMesajlari);
            return mesaj;
        }

        public void temizle()
        {
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            textBox6.Text = null;

            masaID = 0;
            katID = 0;
            sandalyeSayisi = 0;
            adi = null;
            masaDurumu = null;
            aciklama = null;
        }

        public void degerAtama()
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                adi = textBox1.Text;
            else adi = null;
            if (!string.IsNullOrEmpty(textBox6.Text))
                masaDurumu = textBox6.Text;
            else masaDurumu = null;
            if (!string.IsNullOrEmpty(textBox2.Text))
                sandalyeSayisi = int.Parse(textBox2.Text);
            else sandalyeSayisi = 0;
            if (panel3.Visible == true)
            {
                if (!string.IsNullOrEmpty(textBox4.Text))
                    katID = int.Parse(textBox4.Text);
                else katID = 0;
                if (!string.IsNullOrEmpty(textBox3.Text))
                    aciklama = textBox3.Text;
                else aciklama = null;
            }
            else if (panel4.Visible == true)
            {
                if (!string.IsNullOrEmpty(textBox5.Text))
                    aciklama = textBox5.Text;
                else aciklama = null;
            }
        }
    }
}
