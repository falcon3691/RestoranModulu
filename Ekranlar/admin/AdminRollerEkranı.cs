using System.Windows.Forms;
namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminRollerEkranı : Form
    {
        VTRoller vt = new VTRoller();
        public AdminRollerEkranı()
        {
            InitializeComponent();
            Listele();
        }

        public void Listele()
        {
            dataGridView1.DataSource = vt.Listele();
        }
        // Hepsini Listele butonu
        private void button2_Click(object sender, System.EventArgs e)
        {
            Listele();
        }
    }
}
