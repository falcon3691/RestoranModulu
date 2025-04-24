using System;
using System.Data;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminSiparislerEkranı : Form
    {
        VTSiparisler vt = new VTSiparisler();
        VTMasa vtMasa = new VTMasa();
        VTKullanicilar vtKullanicilar = new VTKullanicilar();

        int masaID, kullaniciID = 0;
        DateTime ilkTarih, sonTarih;

        public AdminSiparislerEkranı()
        {
            InitializeComponent();

            // Masaları listeleme
            DataTable dtMasa = vtMasa.Listele2();
            DataRow yeniSatir = dtMasa.NewRow();
            yeniSatir["masaID"] = 0;
            yeniSatir["adi"] = "Bir masa seçin";
            dtMasa.Rows.InsertAt(yeniSatir, 0);

            comboBox1.DataSource = dtMasa;
            comboBox1.DisplayMember = "adi";
            comboBox1.ValueMember = "masaID";

            // Kullanıcıları listeleme
            DataTable dtKullanici = vtKullanicilar.Listele2();
            yeniSatir = dtKullanici.NewRow();
            yeniSatir["kullaniciID"] = 0;
            yeniSatir["adiSoyadi"] = "Bir kullanıcı seçin";
            dtKullanici.Rows.InsertAt(yeniSatir, 0);

            comboBox2.DataSource = dtKullanici;
            comboBox2.DisplayMember = "adiSoyadi";
            comboBox2.ValueMember = "kullaniciID";


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
            if (Convert.ToInt32(comboBox1.SelectedValue) != 0)
                masaID = Convert.ToInt32(comboBox1.SelectedValue);
            else masaID = 0;
            if (Convert.ToInt32(comboBox2.SelectedValue) != 0)
                kullaniciID = Convert.ToInt32(comboBox2.SelectedValue);
            else kullaniciID = 0;
            if (checkBox1.Checked)
                sonTarih = dateTimePicker2.Value;
            ilkTarih = dateTimePicker1.Value;
        }
    }
}
