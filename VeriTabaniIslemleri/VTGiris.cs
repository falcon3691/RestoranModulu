using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public class VTGiris
{
    // Veri tabanı bağlantısı.
    public string baglantiKodu = "Data Source=DESKTOP-HSH38D0;Initial Catalog=RestoranModulu;Integrated Security=True";

    public VTGiris()
    {

    }

    // Giriş ekranında kullanıcı kontrolü yaparken kullanılır ve Data Table olarak geri döndürür.
    public DataTable KullaniciListele(string kullaniciAdi, string kullaniciParola, int rolID)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"SELECT * FROM Kullanici WHERE kullaniciAdi='{kullaniciAdi}' AND parola='{kullaniciParola}' AND rolID='{rolID}'";
        SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
        try
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            baglanti.Close();
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Girilen bilgilere göre bir kullanıcı bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            else
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
