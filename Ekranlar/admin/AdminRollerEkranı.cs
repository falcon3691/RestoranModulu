using System.Windows.Forms;
namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminRollerEkranı : Form
    {
        VTGiris vt = new VTGiris();
        public AdminRollerEkranı()
        {
            InitializeComponent();
            Listele();
        }

        public void Listele()
        {
            dataGridView1.DataSource = vt.Listele("Roller");
        }
        // Hepsini Listele butonu
        private void button2_Click(object sender, System.EventArgs e)
        {
            Listele();
        }
    }
}
