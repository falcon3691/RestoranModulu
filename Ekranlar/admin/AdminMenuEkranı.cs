using System;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminMenuEkranı : Form
    {
        public AdminMenuEkranı()
        {
            InitializeComponent();
        }

        // Menüyü Düzenle butonu
        private void button2_Click(object sender, EventArgs e)
        {
            AdminMenuUrunEkranı form = new AdminMenuUrunEkranı();
            this.SuspendLayout();
            form.ShowDialog();
        }
    }
}
