using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminKategoriEkranı : Form
    {
        VTKategoriler vt = new VTKategoriler();

        // Kategori bilgileri değişkenleri.
        int kategoriID = 0;
        string adi, aciklama = null;

        public AdminKategoriEkranı()
        {
            InitializeComponent();
            dataGridView1.DataSource = vt.Listele();
        }
        // Yeni Kategori Ekle butonu
        private void button1_Click(object sender, System.EventArgs e)
        {
            string mesaj = boslukKontrolu();
            if (mesaj == null)
            {
                degerAtama();
                // Kategori ekleme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.kategoriEkle(adi, aciklama))
                {
                    dataGridView1.DataSource = vt.Listele();
                    temizle();
                }

            }
            else
                MessageBox.Show(mesaj, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Kategoriyi Güncelle butonu
        private void button2_Click(object sender, System.EventArgs e)
        {
            if (kategoriID > 0)
            {
                degerAtama();
                // Kategori güncelleme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.kategoriGuncelle(kategoriID, adi, aciklama))
                {
                    dataGridView1.DataSource = vt.Listele();
                    temizle();
                }
            }
            else
                MessageBox.Show("Kategoriler listesi içinden bir kategori şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Kategoriyi Sil butonu
        private void button3_Click(object sender, System.EventArgs e)
        {
            if (kategoriID > 0)
            {
                degerAtama();
                // Kategori silme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.kategoriSil(kategoriID))
                {
                    dataGridView1.DataSource = vt.Listele();
                    temizle();
                }
            }
            else
                MessageBox.Show("Kategoriler listesi içinden bir kategori şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Hepsini Listele butonu
        private void button4_Click(object sender, System.EventArgs e)
        {
            dataGridView1.DataSource = vt.Listele();
            temizle();
        }

        // dataGridView listesinde bulunan bir satıra tıklanınca, o satır içinde bulunan bilgiler ile textBox ve comboBox'ları doldurur.
        // Ayriyeten kategori üstünde işlem yapmak için kategoriID değerini alır.
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < (dataGridView1.Rows.Count - 1))
            {
                kategoriID = int.Parse(dataGridView1.Rows[satirNo].Cells[0].Value.ToString());
                textBox1.Text = dataGridView1.Rows[satirNo].Cells[1].Value.ToString();
                textBox2.Text = dataGridView1.Rows[satirNo].Cells[2].Value.ToString();
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
            textBox2.Text = null;

            // Kullanıcı değişkenlerinin temizlenmesi.
            kategoriID = 0;
            adi = null;
            aciklama = null;
        }

        // Ürün değişkenlerine değer atama işlemleri
        public void degerAtama()
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                adi = textBox1.Text;
            else adi = null;
            if (!string.IsNullOrEmpty(textBox2.Text))
                aciklama = textBox2.Text;
            else aciklama = null;
        }
    }
}
