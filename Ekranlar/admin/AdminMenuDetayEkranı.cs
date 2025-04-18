using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminMenuDetayEkranı : Form
    {
        VTMenu vt = new VTMenu();
        int menuID = 0;
        string adi, aciklama = null;

        VTUrunler vtUrun = new VTUrunler();
        int urunID, kategoriID, fiyati, miktar = 0;
        string urunAdi = null;

        public AdminMenuDetayEkranı(int menuID = 0, string adi = null)
        {
            InitializeComponent();
            if (menuID != 0)
            {
                this.menuID = menuID;
                DataTable dt = vt.Listele(menuID);
                textBox5.Text = dt.Rows[0][1].ToString();
                textBox4.Text = dt.Rows[0][2].ToString();
            }
            else
            {
                DataTable dt = vt.sonMenuyuAl();
                this.menuID = Convert.ToInt32(dt.Rows[0][0]);
            }

            if (adi != null)
                this.adi = adi;
            dataGridView1.DataSource = vt.urunleriListele(menuID);
            dataGridView2.DataSource = vtUrun.Listele();
        }

        // Ürünleri Filtrele butonu
        private void button1_Click(object sender, System.EventArgs e)
        {
            urunDegerAtama();
            DataTable filtrelenmisListe = vt.urunFiltrele(urunAdi, kategoriID, fiyati, miktar);
            if (filtrelenmisListe.Rows.Count > 0)
                dataGridView2.DataSource = filtrelenmisListe ?? vtUrun.Listele();
            else
            {
                MessageBox.Show("Girilen bilgilere göre bir ürün bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridView2.DataSource = vtUrun.Listele();
            }
        }

        // Hepsini Listele butonu
        private void button2_Click(object sender, System.EventArgs e)
        {
            dataGridView2.DataSource = vtUrun.Listele();
            temizle();
        }

        // Menüyü Temizle butonu.
        private void button3_Click(object sender, System.EventArgs e)
        {
            vt.menuyuTemizle(menuID);
            dataGridView1.DataSource = vt.urunleriListele(menuID);
            dataGridView2.DataSource = vtUrun.Listele();

        }

        // Menüyü Kaydet butonu
        private void button4_Click(object sender, System.EventArgs e)
        {
            string mesaj = boslukKontrolu();
            if (mesaj == null)
            {
                menuDegerAtama();
                vt.menuGuncelle(menuID, adi, aciklama);
                this.Close();
            }
            else
                MessageBox.Show(mesaj, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Menüyü Sil butonu
        private void button5_Click(object sender, System.EventArgs e)
        {
            vt.menuSil(menuID);
            this.Close();
        }

        // QR Kod Oluştur butonu
        private void button6_Click(object sender, System.EventArgs e)
        {
            if (menuID > 0)
            {
                Bitmap qrKod = vt.qrOlustur(adi);
                if (qrKod == null)
                {
                    MessageBox.Show("QR kod oluşturulamadı.");
                    return;
                }
                AdminMenuQR form = new AdminMenuQR(qrKod);
                this.SuspendLayout();
                form.ShowDialog();
            }
            else
                MessageBox.Show("Menüler listesinden bir menü seçilmeli.");
        }

        // Menüdeki ürünler listesinden seçilen ürünün, menuID değeri 0 yapılır ve liste güncellenir.
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < (dataGridView1.Rows.Count - 1))
            {
                var satir = dataGridView1.Rows[satirNo];
                try
                {
                    if (menuID != 0)
                    {
                        urunID = Convert.ToInt32(satir.Cells[0].Value);
                        vt.menudenUrunSil(menuID, urunID);
                        dataGridView1.DataSource = vt.urunleriListele(menuID);
                        dataGridView2.DataSource = vtUrun.Listele();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Satır verileri alınırken bir sorun oluştu: " + ex.Message);
                }
            }
        }

        // Tüm ürünler listesinden seçilen ürünün, menuID değeri değiştirilir ve liste güncellenir.
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < (dataGridView2.Rows.Count - 1))
            {
                var satir = dataGridView2.Rows[satirNo];
                try
                {
                    if (menuID != 0)
                    {
                        urunID = Convert.ToInt32(satir.Cells[0].Value);
                        vt.menuyeUrunEkle(menuID, urunID);
                        dataGridView1.DataSource = vt.urunleriListele(menuID);
                        dataGridView2.DataSource = vtUrun.Listele();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Satır verileri alınırken bir sorun oluştu: " + ex.Message);
                }
            }
        }

        public string boslukKontrolu()
        {
            string mesaj = null;
            if (string.IsNullOrEmpty(textBox5.Text))
                mesaj = "Ad bilgisi boş bırakılamaz";
            return mesaj;
        }

        public void temizle()
        {
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            comboBox1.Text = null;

            urunID = 0;
            kategoriID = 0;
            fiyati = 0;
            miktar = 0;
            urunAdi = null;
        }

        public void menuDegerAtama()
        {
            if (!string.IsNullOrEmpty(textBox5.Text))
                adi = textBox5.Text;
            else adi = null;
            if (!string.IsNullOrEmpty(textBox4.Text))
                aciklama = textBox4.Text;
            else aciklama = null;
        }

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
