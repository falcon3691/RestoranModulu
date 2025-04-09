using System;
using System.Data;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminSiparislerEkranı : Form
    {
        VTSiparisler vtSiparis = new VTSiparisler();

        int masaID, kullaniciID = 0;
        DateTime ilkTarih, sonTarih;

        public AdminSiparislerEkranı()
        {
            InitializeComponent();
            dataGridView1.DataSource = vtSiparis.tumSiparisleriListele();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker2.Enabled = checkBox1.Checked;
        }
        // Siparişleri Filtrele butonu
        private void button1_Click(object sender, EventArgs e)
        {
            degerAtama();
            if (vtSiparis.siparisFiltrele(masaID, kullaniciID, ilkTarih, sonTarih, checkBox1.Checked) != null)
                dataGridView1.DataSource = vtSiparis.siparisFiltrele(masaID, kullaniciID, ilkTarih, sonTarih, checkBox1.Checked);
            else
                dataGridView1.DataSource = vtSiparis.tumSiparisleriListele();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < (dataGridView1.Rows.Count - 1))
            {
                int siparisID = int.Parse(dataGridView1.Rows[satirNo].Cells[0].Value.ToString());
                AdminSiparisDetayEkranı form = new AdminSiparisDetayEkranı(siparisID);
                this.SuspendLayout();
                form.ShowDialog();
            }
        }

        // Hepsini Listele butonu
        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = vtSiparis.tumSiparisleriListele();
            if (dt.Rows.Count > 0)
                dataGridView1.DataSource = dt;
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
