using System;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminKatEkranı : Form
    {
        public AdminKatEkranı()
        {
            InitializeComponent();
        }
        // Filtrele butonu
        private void button6_Click(object sender, EventArgs e)
        {

        }
        // Masa Ekle butonu
        private void button4_Click(object sender, EventArgs e)
        {
            string formAdi = "AdminKatEkranı";
            AdminMasaEkranı form = new AdminMasaEkranı(formAdi);
            this.SuspendLayout();
            form.ShowDialog();
        }
    }
}
