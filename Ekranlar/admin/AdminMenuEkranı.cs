using System;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminMenuEkranı : Form
    {
        VTMenu vt = new VTMenu();

        // Menü bilgileri değişkenleri
        int menuID = 0;

        public AdminMenuEkranı()
        {
            InitializeComponent();
            dataGridView1.DataSource = vt.Listele();
        }

        // Yeni Menü Ekle butonu
        private void button1_Click(object sender, EventArgs e)
        {
            AdminMenuDetayEkranı form = new AdminMenuDetayEkranı();
            this.SuspendLayout();
            form.ShowDialog();
            dataGridView1.DataSource = vt.Listele();
        }

        // Seçilen menünün, menuID değeri ile menü detay ekranına geçiş yapar.
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0)
            {
                menuID = int.Parse(dataGridView1.Rows[satirNo].Cells[0].Value.ToString());
                AdminMenuDetayEkranı form = new AdminMenuDetayEkranı(menuID);
                this.SuspendLayout();
                form.ShowDialog();
                dataGridView1.DataSource = vt.Listele();
            }
        }
    }
}
