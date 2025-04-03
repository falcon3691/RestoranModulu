using System.Data;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminMenuDetayEkranı : Form
    {
        VTMenu vt = new VTMenu();
        VTUrunler vtUrun = new VTUrunler();

        // Menü bilgileri değişkenleri
        int menuID = 0;
        string adi, aciklama = null;

        // Ürün bilgileri değişkenleri
        int urunID, kategoriID, fiyati, miktar = 0;
        string urunAdi, durumu, resimYolu, urunAciklama = null;

        public AdminMenuDetayEkranı(int menuID = 0)
        {
            InitializeComponent();
            if (menuID != 0)
            {
                this.menuID = menuID;
                DataTable dt = vt.Listele(menuID);
                textBox5.Text = dt.Rows[0][1].ToString();
                textBox4.Text = dt.Rows[0][2].ToString();

                dataGridView1.DataSource = vt.urunleriListele(menuID);
            }
            dataGridView2.DataSource = vtUrun.Listele();
        }

        // Ürünleri Filtrele butonu
        private void button1_Click(object sender, System.EventArgs e)
        {
            urunDegerAtama();
            if (vtUrun.urunFiltrele(urunAdi, kategoriID, fiyati, miktar) != null)
                dataGridView2.DataSource = vtUrun.urunFiltrele(urunAdi, kategoriID, fiyati, miktar);
            else
                dataGridView2.DataSource = vtUrun.Listele();
        }

        // Hepsini Listele butonu
        private void button2_Click(object sender, System.EventArgs e)
        {
            dataGridView2.DataSource = vtUrun.Listele();
            temizle();
        }

        // Menüyü Kaydet butonu
        private void button4_Click(object sender, System.EventArgs e)
        {
            string mesaj = boslukKontrolu();
            if (mesaj == null)
            {
                if (menuID == 0)
                {
                    degerAtama();
                    if (vt.menuEkle(adi, aciklama))
                    {
                        this.Close();
                    }
                }
                else
                {
                    degerAtama();
                    if (vt.menuGuncelle(menuID, adi, aciklama))
                    {
                        this.Close();
                    }
                }
            }
            else
                MessageBox.Show(mesaj, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Menüyü Sil butonu
        private void button5_Click(object sender, System.EventArgs e)
        {
            if (menuID == 0)
            {
                this.Close();
            }
            else
            {
                // Menü silme işlemi başarılı olursa true döndürür, başarısız olursa hata verir ve false döndürür.
                if (vt.menuSil(menuID))
                {
                    this.Close();
                }
            }

        }

        // Menüyü Temizle butonu.
        private void button3_Click(object sender, System.EventArgs e)
        {
            if (vt.menuyuTemizle(menuID))
            {
                dataGridView1.DataSource = vt.urunleriListele(menuID);
                dataGridView2.DataSource = vtUrun.Listele();
            }
        }

        // QR Kod Oluştur butonu
        private void button6_Click(object sender, System.EventArgs e)
        {

        }

        // Tüm Ürünler listesinde bulunan ürüne tıklanınca, menuID değeri değiştirilir ve Menüdeki Ürünler listesi güncellenir.
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < (dataGridView2.Rows.Count - 1))
            {
                if (menuID != 0)
                {
                    urunID = int.Parse(dataGridView2.Rows[satirNo].Cells[0].Value.ToString());


                    if (vt.menuyeUrunEkle(menuID, urunID))
                    {
                        dataGridView1.DataSource = vt.urunleriListele(menuID);
                        dataGridView2.DataSource = vtUrun.Listele();
                    }
                }
            }
        }

        // Menüdeki Ürünler listesinde seçilen ürünün, menuID değeri 0 yapılır ve Menüdeki Ürünler listesi güncellenir.
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0)
            {
                if (menuID != 0)
                {
                    urunID = int.Parse(dataGridView1.Rows[satirNo].Cells[0].Value.ToString());

                    if (vt.menudenUrunSil(menuID, urunID))
                    {
                        dataGridView1.DataSource = vt.urunleriListele(menuID);
                        dataGridView2.DataSource = vtUrun.Listele();
                    }
                }
            }
        }

        // Veri tablosunda NULL değer alamayan sütunlar için gerekli boşluk kontrolleri yapılır.
        public string boslukKontrolu()
        {
            string mesaj = null;
            if (string.IsNullOrEmpty(textBox5.Text))
                mesaj = "Ad bilgisi boş bırakılamaz";
            return mesaj;
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
            urunID = 0;
            kategoriID = 0;
            fiyati = 0;
            miktar = 0;
            urunAdi = null;
        }

        // Menü değişkenlerine değer atama işlemleri
        public void degerAtama()
        {
            if (!string.IsNullOrEmpty(textBox5.Text))
                adi = textBox5.Text;
            else adi = null;
            if (!string.IsNullOrEmpty(textBox4.Text))
                aciklama = textBox4.Text;
            else aciklama = null;
        }

        // Ürün değişkenlerine değer atama işlemleri
        public void urunDegerAtama()
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                urunAdi = textBox1.Text;
            else adi = null;
            if (!string.IsNullOrEmpty(comboBox1.Text))
                kategoriID = int.Parse(comboBox1.Text);
            else kategoriID = 0;
            if (!string.IsNullOrEmpty(textBox2.Text))
                fiyati = int.Parse(textBox2.Text);
            else fiyati = 0;
            if (!string.IsNullOrEmpty(textBox3.Text))
                miktar = int.Parse(textBox3.Text);
            else miktar = 0;
        }
    }
}
