using System;
using System.Data;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminSiparislerEkranı : Form
    {
        VTSiparisler vt = new VTSiparisler();

        int masaID, kullaniciID = 0;
        DateTime ilkTarih, sonTarih;

        public AdminSiparislerEkranı()
        {
            InitializeComponent();
            dataGridView1.DataSource = vt.tumSiparisleriListele();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker2.Enabled = checkBox1.Checked;
        }

        // Siparişleri Filtrele butonu
        private void button1_Click(object sender, EventArgs e)
        {
            degerAtama();
            DataTable filtrelenmisListe = vt.siparisFiltrele(ilkTarih, sonTarih, checkBox1.Checked, masaID, kullaniciID);
            if (filtrelenmisListe.Rows.Count > 0)
                dataGridView1.DataSource = filtrelenmisListe ?? vt.tumSiparisleriListele();
            else
            {
                MessageBox.Show("Girilen bilgilere göre bir siparis bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridView1.DataSource = vt.tumSiparisleriListele();
            }
        }

        // Hepsini Listele butonu
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = vt.tumSiparisleriListele();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < (dataGridView1.Rows.Count - 1))
            {
                var satir = dataGridView1.Rows[satirNo];
                try
                {
                    int siparisID = Convert.ToInt32(satir.Cells[0].Value);
                    AdminSiparisDetayEkranı form = new AdminSiparisDetayEkranı(siparisID);
                    this.SuspendLayout();
                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Satır verileri alınırken bir hata oluştu: " + ex.Message);
                }
            }

        }

        public void degerAtama()
        {
            if (!string.IsNullOrEmpty(comboBox1.Text))
                masaID = int.Parse(comboBox1.Text);
            else masaID = 0;
            if (!string.IsNullOrEmpty(comboBox2.Text))
                kullaniciID = int.Parse(comboBox2.Text);
            else kullaniciID = 0;
            if (checkBox1.Checked)
                sonTarih = dateTimePicker2.Value;
            ilkTarih = dateTimePicker1.Value;
        }
    }
}
