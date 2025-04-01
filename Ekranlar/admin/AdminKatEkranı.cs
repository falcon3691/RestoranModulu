using System;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminKatEkranı : Form
    {
        VTKatlar vt = new VTKatlar();

        // Ürün bilgileri değişkenleri
        int katID = 0;
        string adi, masalarID, durumu, aciklama = null;

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
            if (katID > 0)
            {
                AdminMasaEkranı form = new AdminMasaEkranı("AdminKatEkranı", katID);
                this.SuspendLayout();
                form.ShowDialog();
            }
            else
                MessageBox.Show("Katlar listesi içinden bir kat şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
