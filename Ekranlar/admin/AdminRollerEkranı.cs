using System.Windows.Forms;
namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminRollerEkranı : Form
    {
        VTRoller vt = new VTRoller();

        // Rol bilgileri değişkenleri
        int rolID = 0;
        string adi, aciklama, durumu = null;

        public AdminRollerEkranı()
        {
            InitializeComponent();
            dataGridView1.DataSource = vt.Listele();
        }

        // Yeni Rol Ekle
        private void button5_Click(object sender, System.EventArgs e)
        {
            string mesaj = boslukKontrolu();
            if (mesaj == null)
            {
                degerAtama();
                // Kategori ekleme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.rolEkle(adi, aciklama, durumu))
                {
                    dataGridView1.DataSource = vt.Listele();
                    temizle();
                }

            }
            else
                MessageBox.Show(mesaj, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Rolü Güncelle butonu
        private void button4_Click(object sender, System.EventArgs e)
        {
            if (rolID > 0)
            {
                degerAtama();
                // Rol güncelleme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.rolGuncelle(rolID, adi, aciklama, durumu))
                {
                    dataGridView1.DataSource = vt.Listele();
                    temizle();
                }
            }
            else
                MessageBox.Show("Roller listesi içinden bir rol şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Rolü Sil butonu
        private void button3_Click(object sender, System.EventArgs e)
        {
            if (rolID > 0)
            {
                degerAtama();
                // Rol silme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.rolSil(rolID))
                {
                    dataGridView1.DataSource = vt.Listele();
                    temizle();
                }
            }
            else
                MessageBox.Show("Roller listesi içinden bir rol şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Hepsini Listele butonu
        private void button2_Click(object sender, System.EventArgs e)
        {
            dataGridView1.DataSource = vt.Listele();
            temizle();
        }

        // dataGridView listesinde bulunan bir satıra tıklanınca, o satır içinde bulunan bilgiler ile textBox ve comboBox'ları doldurur.
        // Ayriyeten rol üstünde işlem yapmak için rolID değerini alır.
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < (dataGridView1.Rows.Count - 1))
            {
                rolID = int.Parse(dataGridView1.Rows[satirNo].Cells[0].Value.ToString());
                textBox1.Text = dataGridView1.Rows[satirNo].Cells[1].Value.ToString();
                textBox2.Text = dataGridView1.Rows[satirNo].Cells[2].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[satirNo].Cells[3].Value.ToString();
            }
        }

        // Veri tablosunda NULL değer alamayan sütunlar için gerekli boşluk kontrolleri yapılır.
        public string boslukKontrolu()
        {
            string mesaj = null;
            if (string.IsNullOrEmpty(textBox1.Text))
                mesaj = "Ad bilgisi boş bırakılamaz";
            return mesaj;
        }

        // Hem ekranda ki alanları temizler hem de kullanılan değişkenlerin değerlerini standart konuma getirir.
        public void temizle()
        {
            // Ekranın temizlenmesi.
            textBox1.Text = null;
            comboBox1.Text = null;
            textBox2.Text = null;

            // Kullanıcı değişkenlerinin temizlenmesi.
            rolID = 0;
            adi = null;
            aciklama = null;
            durumu = null;
        }

        // Rol değişkenlerine değer atama işlemleri
        public void degerAtama()
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                adi = textBox1.Text;
            else adi = null;
            if (!string.IsNullOrEmpty(textBox2.Text))
                aciklama = textBox2.Text;
            else aciklama = null;
            if (!string.IsNullOrEmpty(comboBox1.Text))
                durumu = comboBox1.Text;
            else durumu = null;
        }
    }
}
