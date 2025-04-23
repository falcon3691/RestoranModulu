using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.garson
{
    public partial class DetayNotEkleme : Form
    {
        public string NotIcerigi { get; private set; }
        public DetayNotEkleme(string mevcutNot = "")
        {
            InitializeComponent();
            textBox1.Text = mevcutNot;
        }

        // Notu Kaydet butonu
        private void button1_Click(object sender, System.EventArgs e)
        {
            NotIcerigi = textBox1.Text;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        // Notu Sil butonu
        private void button2_Click(object sender, System.EventArgs e)
        {
            NotIcerigi = string.Empty;
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
