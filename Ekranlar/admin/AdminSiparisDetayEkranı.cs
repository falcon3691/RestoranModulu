using System;
using System.Data;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminSiparisDetayEkranı : Form
    {
        VTSiparisler vtSiparis = new VTSiparisler();
        int siparisID = 0;
        public AdminSiparisDetayEkranı(int siparisID)
        {
            InitializeComponent();
            this.siparisID = siparisID;
            dataGridView1.DataSource = vtSiparis.detayListele2(siparisID);
        }

        // Hepsini Listele butonu
        private void button1_Click(object sender, System.EventArgs e)
        {
            dataGridView1.DataSource = vtSiparis.detayListele2(siparisID);
        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                DataTable filtrelenmisListe = vtSiparis.detayFiltrele(textBox1.Text, siparisID);
                if (filtrelenmisListe.Rows.Count > 0)
                    dataGridView1.DataSource = filtrelenmisListe ?? vtSiparis.detayListele2(siparisID);
            }
            else
            {
                MessageBox.Show("Girilen bilgilere göre bir detay bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridView1.DataSource = vtSiparis.detayListele2(siparisID);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                DataTable filtrelenmisListe = vtSiparis.detayFiltrele2(textBox2.Text, siparisID);
                if (filtrelenmisListe.Rows.Count > 0)
                    dataGridView1.DataSource = filtrelenmisListe ?? vtSiparis.detayListele2(siparisID);
            }
            else
            {
                MessageBox.Show("Girilen bilgilere göre bir detay bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridView1.DataSource = vtSiparis.detayListele2(siparisID);
            }
        }
    }
}
