using System;
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
                vt.rolEkle(adi, aciklama, durumu);
                dataGridView1.DataSource = vt.Listele();
                temizle();
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
                vt.rolGuncelle(rolID, adi, aciklama, durumu);
                dataGridView1.DataSource = vt.Listele();
                temizle();
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
                vt.rolSil(rolID);
                dataGridView1.DataSource = vt.Listele();
                temizle();
            }
            else
                MessageBox.Show("Roller listesi içinden bir rol şeçilmeli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < (dataGridView1.Rows.Count - 1))
            {
                var satir = dataGridView1.Rows[satirNo];

                try
                {
                    rolID = Convert.ToInt32(satir.Cells[0].Value);
                    textBox1.Text = satir.Cells[1].Value?.ToString() ?? "";
                    textBox2.Text = satir.Cells[2].Value?.ToString() ?? "";
                    textBox3.Text = satir.Cells[3].Value?.ToString() ?? "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Satır verileri alınırken bir hata oluştu: " + ex.Message);
                }
            }
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
            textBox3.Text = null;
            textBox2.Text = null;

            rolID = 0;
            adi = null;
            aciklama = null;
            durumu = null;
        }

        public void degerAtama()
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                adi = textBox1.Text;
            else adi = null;
            if (!string.IsNullOrEmpty(textBox2.Text))
                aciklama = textBox2.Text;
            else aciklama = null;
            if (!string.IsNullOrEmpty(textBox3.Text))
                durumu = textBox3.Text;
            else durumu = null;
        }
    }
}
