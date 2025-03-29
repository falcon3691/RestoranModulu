using System.Windows.Forms;
namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminKullaniciEkranı : Form
    {
        public AdminKullaniciEkranı()
        {
            InitializeComponent();
            Listele();
        }

        VeriTabani vt = new VeriTabani();

        public void Listele()
        {
            dataGridView1.DataSource = vt.Listele("Kullanicilar");
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            string adiSoyadi = textBox2.Text;
            string telefon = textBox1.Text;
            string eMail = textBox3.Text;
            string kullaniciAdi = textBox4.Text;
            string parola = textBox5.Text;
            int rolID = int.Parse(comboBox1.Text);
            byte durumu = byte.Parse(comboBox2.Text);
            string aciklama = textBox6.Text;

            if (vt.KullaniciEkle(adiSoyadi, kullaniciAdi, parola, rolID, durumu, aciklama, telefon, eMail))
                Listele();
        }
    }
}
