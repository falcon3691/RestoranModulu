using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public class VTMutfak
{
    // Veri tabanı bağlantısı.
    public string baglantiKodu = "Data Source=DESKTOP-HSH38D0;Initial Catalog=RestoranModulu;Integrated Security=True";
    public DataTable masaBilgisiAl(int masaID)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"SELECT * FROM Masalar WHERE masaID='{masaID}'";

        SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
        try
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            baglanti.Close();
            return dt;
        }
        catch (Exception hata)
        {
            baglanti.Close();
            MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        return null;
    }

    public DataTable siparisleriListe()
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"SELECT * FROM Siparisler";

        SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
        try
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            baglanti.Close();
            return dt;
        }
        catch (Exception hata)
        {
            baglanti.Close();
            MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        return null;
    }
}