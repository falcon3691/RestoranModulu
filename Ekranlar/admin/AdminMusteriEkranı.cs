using System;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminMusteriEkranı : Form
    {
        VTMusteriler vt = new VTMusteriler();

        string adiSoyadi = null;
        int musteriID, iskontoDegeri = 0;

        public AdminMusteriEkranı()
        {
            InitializeComponent();
            listele();
        }

        // Yeni Müşteri Ekle butonu
        private void button1_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Müşteri adı soyadı bilgisi boş bırakılamaz.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            degerAtama();
            vt.MusteriEkle(adiSoyadi, iskontoDegeri);
            listele();
            temizle();
        }

        // Müşteriyi Güncelle butonu
        private void button3_Click(object sender, System.EventArgs e)
        {
            if (musteriID > 0)
            {
                degerAtama();
                vt.MusteriGuncelle(musteriID, adiSoyadi, iskontoDegeri);
                listele();
                temizle();
            }
            else
                MessageBox.Show("Müşteriler listesi içinden bir müşteri şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Müşteriyi Sil butonu
        private void button4_Click(object sender, System.EventArgs e)
        {
            if (musteriID > 0)
            {
                degerAtama();
                vt.MusteriSil(musteriID);
                listele();
                temizle();
            }
            else
                MessageBox.Show("Müşteriler listesi içinden bir müşteri şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < dataGridView1.Rows.Count)
            {
                var satir = dataGridView1.Rows[satirNo];

                try
                {
                    musteriID = Convert.ToInt32(satir.Cells[0].Value);
                    textBox2.Text = satir.Cells[1].Value?.ToString() ?? "";
                    textBox1.Text = satir.Cells[2].Value?.ToString() ?? "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Satır verileri alınırken bir hata oluştu: " + ex.Message);
                }
            }
        }

        public void temizle()
        {
            musteriID = 0;
            iskontoDegeri = 0;
            adiSoyadi = null;

            textBox1.Clear();
            textBox2.Clear();
        }

        public void listele()
        {
            dataGridView1.DataSource = vt.Listele();
            if (dataGridView1.Columns.Contains("musteriID"))
                dataGridView1.Columns["musteriID"].Visible = false;
        }

        public void degerAtama()
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                adiSoyadi = textBox1.Text;
            else
                adiSoyadi = null;
            if (!string.IsNullOrEmpty(textBox2.Text))
                int.TryParse(textBox2.Text, out iskontoDegeri);
            else
                iskontoDegeri = 0;
        }
    }
}

