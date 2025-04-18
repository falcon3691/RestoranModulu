using System;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminMenuEkranı : Form
    {
        VTMenu vt = new VTMenu();

        public AdminMenuEkranı()
        {
            InitializeComponent();
            dataGridView1.DataSource = vt.Listele();
        }

        // Yeni Menü Ekle butonu
        private void button1_Click(object sender, EventArgs e)
        {
            string tarih = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            vt.menuEkle(" ", " ", Convert.ToDateTime(tarih));
            AdminMenuDetayEkranı form = new AdminMenuDetayEkranı();
            this.SuspendLayout();
            form.ShowDialog();
            dataGridView1.DataSource = vt.Listele();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < dataGridView1.Rows.Count)
            {
                var satir = dataGridView1.Rows[satirNo];
                try
                {
                    int menuID = Convert.ToInt32(satir.Cells["menuID"].Value);
                    string adi = satir.Cells["adi"].Value.ToString() ?? "";
                    AdminMenuDetayEkranı form = new AdminMenuDetayEkranı(menuID, adi);
                    this.SuspendLayout();
                    form.ShowDialog();
                    dataGridView1.DataSource = vt.Listele();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Satır verileri alınırken bir hata oluştu: " + ex.Message);
                }
            }
        }
    }
}
