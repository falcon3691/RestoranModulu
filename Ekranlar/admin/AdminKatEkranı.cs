using System;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminKatEkranı : Form
    {
        VTKatlar vt = new VTKatlar();

        // Ürün bilgileri değişkenleri
        int katID, masaSayisi = 0;
        string adi, durumu, aciklama = null;

        public AdminKatEkranı()
        {
            InitializeComponent();
            dataGridView1.DataSource = vt.Listele();
        }

        // Yeni Kat Ekle butonu
        private void button1_Click(object sender, EventArgs e)
        {
            string mesaj = boslukKontrolu();
            if (mesaj == null)
            {
                degerAtama();
                // Kat ekleme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.katEkle(adi, durumu, aciklama))
                {
                    dataGridView1.DataSource = vt.Listele();
                    temizle();
                }
            }
            else
                MessageBox.Show(mesaj, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Katı Güncelle butonu
        private void button2_Click(object sender, EventArgs e)
        {
            if (katID > 0)
            {
                degerAtama();
                // Kat güncelleme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.katGuncelle(katID, adi, durumu, aciklama))
                {
                    dataGridView1.DataSource = vt.Listele();
                    temizle();
                }
            }
            else
                MessageBox.Show("Katlar listesi içinden bir kat şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Katı Sil butonu
        private void button3_Click(object sender, EventArgs e)
        {
            if (katID > 0)
            {
                degerAtama();
                // Kat silme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.katSil(katID))
                {
                    dataGridView1.DataSource = vt.Listele();
                    temizle();
                }
            }
            else
                MessageBox.Show("Katlar listesi içinden bir kat şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Hepsini Listele butonu
        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = vt.Listele();
            temizle();
        }

        // dataGridView listesinde bulunan bir satıra tıklanınca, o satır içinde bulunan bilgiler ile textBox ve comboBox'ları doldurur.
        // Ayriyeten kullanıcı üstünde işlem yapmak için katID değerini alır.
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < (dataGridView1.Rows.Count - 1))
            {
                katID = int.Parse(dataGridView1.Rows[satirNo].Cells[0].Value.ToString());
                textBox1.Text = dataGridView1.Rows[satirNo].Cells[1].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[satirNo].Cells[2].Value.ToString();
                textBox3.Text = dataGridView1.Rows[satirNo].Cells[3].Value.ToString();
                if (!(vt.masaSayisi(katID) == int.Parse("-1")))
                    textBox2.Text = vt.masaSayisi(katID).ToString();
            }
        }

        // Masa Ekle butonu
        private void button4_Click(object sender, EventArgs e)
        {
            if (katID > 0)
            {
                AdminMasaEkranı form = new AdminMasaEkranı("AdminKatEkranı", katID);
                this.SuspendLayout();
                form.ShowDialog();
                dataGridView1.DataSource = vt.Listele();
                temizle();
            }
            else
                MessageBox.Show("Katlar listesi içinden bir kat şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Hem ekranda ki alanları temizler hem de kullanılan değişkenlerin değerlerini standart konuma getirir.
        public void temizle()
        {
            // Ekranın temizlenmesi.
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            comboBox1.Text = null;

            // Kullanıcı değişkenlerinin temizlenmesi.
            katID = 0;
            masaSayisi = 0;
            adi = null;
            durumu = null;
            aciklama = null;
        }

        // Kullanıcı değişkenlerine değer atama işlemleri
        public void degerAtama()
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                adi = textBox1.Text;
            else adi = null;
            if (!string.IsNullOrEmpty(textBox2.Text))
                masaSayisi = int.Parse(textBox2.Text);
            else masaSayisi = 0;
            if (!string.IsNullOrEmpty(comboBox1.Text))
                durumu = comboBox1.Text;
            else durumu = null;
            if (!string.IsNullOrEmpty(textBox3.Text))
                aciklama = textBox3.Text;
            else aciklama = null;
        }

        // Veri tablosunda NULL değer alamayan sütunlar için gerekli boşluk kontrolleri yapılır.
        public string boslukKontrolu()
        {
            string mesaj = null;
            if (string.IsNullOrEmpty(textBox1.Text))
                mesaj = "Ad bilgisi boş bırakılamaz";
            return mesaj;
        }

    }
}
