using System;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminKatEkranı : Form
    {
        VTKatlar vt = new VTKatlar();

        int katID = 0;
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
                vt.katEkle(adi, durumu, aciklama);
                dataGridView1.DataSource = vt.Listele();
                temizle();
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
                vt.katGuncelle(katID, adi, durumu, aciklama);
                dataGridView1.DataSource = vt.Listele();
                temizle();
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
                vt.katSil(katID);
                dataGridView1.DataSource = vt.Listele();
                temizle();
            }
            else
                MessageBox.Show("Katlar listesi içinden bir kat şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Masa Ekle butonu
        private void button4_Click(object sender, EventArgs e)
        {
            if (katID > 0)
            {
                AdminMasaEkranı form = new AdminMasaEkranı(katID);
                this.SuspendLayout();
                form.ShowDialog();
                dataGridView1.DataSource = vt.Listele();
                temizle();
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < dataGridView1.Rows.Count)
            {
                var satir = dataGridView1.Rows[satirNo];

                try
                {
                    katID = Convert.ToInt32(satir.Cells[0].Value);
                    textBox1.Text = satir.Cells[1].Value.ToString() ?? "";
                    textBox4.Text = satir.Cells[2].Value.ToString() ?? "";
                    textBox3.Text = satir.Cells[3].Value.ToString() ?? "";
                    textBox2.Text = (vt.masaSayisi(katID) != 0) ? vt.masaSayisi(katID).ToString() : 0.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Satır verileri alınırken bir hata oluştu: " + ex.Message);
                }
            }
        }

        public void degerAtama()
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                adi = textBox1.Text;
            else adi = null;
            if (!string.IsNullOrEmpty(textBox4.Text))
                durumu = textBox4.Text;
            else durumu = null;
            if (!string.IsNullOrEmpty(textBox3.Text))
                aciklama = textBox3.Text;
            else aciklama = null;
        }

        public string boslukKontrolu()
        {
            string mesaj = null;
            if (string.IsNullOrEmpty(textBox1.Text))
                mesaj = "Ad bilgisi boş bırakılamaz";
            return mesaj;
        }

        public void temizle()
        {
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;

            katID = 0;
            adi = null;
            durumu = null;
            aciklama = null;
        }
    }
}

