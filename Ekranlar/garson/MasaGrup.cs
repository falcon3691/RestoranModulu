using System;
using System.Data;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.garson
{
    public partial class MasaGrup : Form
    {
        VTMasa vtMasa = new VTMasa();

        int masaID = 0;
        string masaAdi = null;
        public MasaGrup(int masaID, string masaAdi)
        {
            InitializeComponent();
            this.masaID = masaID;
            this.masaAdi = masaAdi;
            ekranDoldurma(masaID);
        }

        // Geri Dön butonu
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Yeni Grubu Kur butonu
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count > 0)
            {
                vtMasa.masalarıGrupla(dataGridView2);
                this.Close();
            }

        }

        // Gruptan Ayrıl butonu
        private void button3_Click(object sender, EventArgs e)
        {
            if (masaID > 0)
            {
                vtMasa.gruptanAyril(masaID);
                ekranDoldurma(masaID);
            }

        }

        // Grubu Dağıt butonu
        private void button4_Click(object sender, EventArgs e)
        {
            if (masaID > 0)
            {
                vtMasa.grubuDagit(dataGridView2);
                ekranDoldurma(masaID);
            }
        }

        // Listeden seçilen masa gruba eklenmek üzere listeye eklenir.
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < dataGridView1.Rows.Count)
            {
                var satir = dataGridView1.Rows[satirNo];
                try
                {
                    int masaID = Convert.ToInt32(satir.Cells["masaID"].Value);
                    string adi = satir.Cells["adi"].Value.ToString() ?? "";
                    dataGridView2.Rows.Add(masaID, adi);
                    dataGridView1.Rows.Remove(satir);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Satır verileri alınırken bir sorun oluştu: " + ex.Message);
                }
            }
        }

        // Listeye eklenen masa, listeden kaldırılır.
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = e.RowIndex;
            if (satirNo >= 0 && satirNo < dataGridView2.Rows.Count)
            {
                var satir = dataGridView2.Rows[satirNo];
                try
                {
                    int masaID = Convert.ToInt32(satir.Cells["masaID"].Value);
                    string adi = satir.Cells["adi"].Value.ToString() ?? "";
                    dataGridView1.Rows.Add(masaID, adi);
                    dataGridView2.Rows.Remove(satir);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Satır verileri alınırken bir sorun oluştu: " + ex.Message);
                }
            }
        }

        public void ekranDoldurma(int masaID)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("masaID", "Masa ID");
            dataGridView1.Columns.Add("adi", "Masa Adı");

            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            dataGridView2.Columns.Add("masaID", "Masa ID");
            dataGridView2.Columns.Add("adi", "Masa Adı");

            foreach (DataRow row in vtMasa.masaGrupListele(masaID).Rows)
            {
                dataGridView1.Rows.Add(row["masaID"], row["adi"]);
            }
            dataGridView2.Rows.Add(masaID, masaAdi);
            foreach (DataRow row in vtMasa.masaGrupListele2(masaID).Rows)
            {
                dataGridView2.Rows.Add(row["masaID"], row["adi"]);
            }
        }

    }
}
