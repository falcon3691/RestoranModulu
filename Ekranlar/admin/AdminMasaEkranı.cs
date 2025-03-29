using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminMasaEkranı : Form
    {
        public AdminMasaEkranı(string formAdi = null)
        {
            InitializeComponent();
            if (formAdi == "AdminKatEkranı")
            {
                panel2.Parent = this;
                panel4.Parent = this;
                panel2.Visible = true;
                panel4.Visible = true;
                panel1.Visible = false;
                panel3.Visible = false;
            }
        }
        // Masayı Güncelle butonu
        private void button2_Click(object sender, System.EventArgs e)
        {

        }
        // Masayı Kata Ekle butonu
        private void button7_Click(object sender, System.EventArgs e)
        {

        }

    }
}
