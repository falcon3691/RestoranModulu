using System;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminKategoriEkranı : Form
    {
        VTKategoriler vt = new VTKategoriler();
        public int kategoriID = 0;
        public string adi, aciklama = null;

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
                vt.kategoriEkle(adi, aciklama);
                dataGridView1.DataSource = vt.Listele();
                temizle();

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
                vt.kategoriGuncelle(kategoriID, adi, aciklama);
                dataGridView1.DataSource = vt.Listele();
                temizle();
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
                vt.kategoriSil(kategoriID);
                dataGridView1.DataSource = vt.Listele();
                temizle();
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < dataGridView1.Rows.Count)
            {
                var satir = dataGridView1.Rows[satirNo];

                try
                {
                    kategoriID = Convert.ToInt32(satir.Cells[0].Value);
                    textBox1.Text = satir.Cells[1].Value.ToString() ?? "";
                    textBox2.Text = satir.Cells[2].Value.ToString() ?? "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Satır verileri alınırken bir hata oluştu: " + ex.Message);
                }
            }
        }

        public void temizle()
        {
            textBox1.Clear();
            textBox2.Clear();
            kategoriID = 0;
            adi = null;
            aciklama = null;
        }

        public string boslukKontrolu()
        {
            string mesaj = null;
            if (string.IsNullOrEmpty(textBox1.Text))
                mesaj = "Ad bilgisi boş bırakılamaz";
            return mesaj;
        }

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
