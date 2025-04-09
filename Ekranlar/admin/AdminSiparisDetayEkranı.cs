using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminSiparisDetayEkranı : Form
    {
        VTSiparisler vtSiparis = new VTSiparisler();
        int siparisID;
        public AdminSiparisDetayEkranı(int siparisID)
        {
            InitializeComponent();
            this.siparisID = siparisID;
            dataGridView1.DataSource = vtSiparis.detayListele2(siparisID);
        }

        // Hepsini Listele butonu
        private void button2_Click(object sender, System.EventArgs e)
        {
            dataGridView1.DataSource = vtSiparis.detayListele2(siparisID);
        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                if (vtSiparis.detayFiltrele(textBox1.Text, siparisID) != null)
                    dataGridView1.DataSource = vtSiparis.detayFiltrele(textBox1.Text, siparisID);
            }
            else
                dataGridView1.DataSource = vtSiparis.detayListele2(siparisID);

        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }
    }
}
