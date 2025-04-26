using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

public class VTMutfak
{
    public string baglantiKodu = "Server=localhost; Database=restoranmodulu; Uid=root; Pwd=Malukat3691.;";
    public DataTable masaBilgisiAl(int masaID)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT adi FROM Masalar WHERE masaID=@masaID";
            using (MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti))
            {
                komut.Parameters.AddWithValue("@masaID", masaID);
                try
                {
                    baglanti.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(komut);
                    da.Fill(dt);
                }
                catch (Exception hata)
                {
                    MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        return dt;
    }

    public DataTable siparisleriListe()
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT * FROM Siparisler WHERE durumu != 'ödendi' AND durumu != 'tamamlandı' ORDER BY olusturmaTarihi DESC";

            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                MySqlDataAdapter da = new MySqlDataAdapter(komut);
                da.Fill(dt);
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        return dt;
    }

}