using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public class VTMenu
{
    // Veri tabanı bağlantısı.
    public string baglantiKodu = "Data Source=DESKTOP-HSH38D0;Initial Catalog=RestoranModulu;Integrated Security=True";

    // Tablo üzerindeki verileri çeker ve Data Table olarak geri döndürür.
    public DataTable Listele(int menuID = 0)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"SELECT * FROM Menuler";

        if (menuID != 0)
            sqlKomutu += $" WHERE menuID='{menuID}'";

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
    public bool menuEkle(string adi, string aciklama = null)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"INSERT INTO Menuler(adi, aciklama)" +
                                          $" VALUES('{adi}', '{aciklama}')";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Menü başarılı bir şekilde eklendi");
            else
            {
                MessageBox.Show("Menü eklenemedi.");
                return false;
            }
        }
        catch (Exception hata)
        {
            baglanti.Close();
            MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        return true;
    }

    // "Menuler" tablosu içerisinde, ID değeri verilen menünün bilgilerini günceller.
    public bool menuGuncelle(int menuID, string adi, string aciklama = null)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"UPDATE Menuler " +
                           $"SET adi='{adi}', aciklama='{aciklama}' " +
                           $"WHERE menuID='{menuID}'";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Menü bilgileri başarılı bir şekilde güncellendi");
            else
            {
                MessageBox.Show("Menü bilgileri güncellenemedi.");
                return false;
            }
        }
        catch (Exception hata)
        {
            baglanti.Close();
            MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        return true;
    }

    // "Menuler" tablosu içerisinde, ID değeri verilen menünün bilgilerini siler.
    public bool menuSil(int menuID)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"DELETE FROM Menuler WHERE menuID='{menuID}'";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Menü bilgileri başarılı bir şekilde silindi");
            else
            {
                MessageBox.Show("Menü bilgileri silinemedi.");
                return false;
            }
        }
        catch (Exception hata)
        {
            baglanti.Close();
            MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        return true;
    }

    public bool menuyeUrunEkle(int menuID, int urunID)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);

        string sqlKomutu = "INSERT INTO MenuDetay(menuID, urunID) " +
                           "SELECT @menuID, @urunID " +
                           "WHERE NOT EXISTS (SELECT 1 FROM MenuDetay WHERE menuID = @menuID AND urunID = @urunID)";

        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            komut.Parameters.AddWithValue("@menuID", menuID);
            komut.Parameters.AddWithValue("@urunID", urunID);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Ürün, menüye başarılı bir şekilde eklendi");
            else
            {
                MessageBox.Show("Ürün, menüye eklenemedi.");
                return false;
            }
        }
        catch (SqlException)
        {
            baglanti.Close();
            MessageBox.Show("Bu ürün zaten eklendi.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        catch (Exception hata)
        {
            baglanti.Close();
            MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        return true;
    }

    public bool menudenUrunSil(int menuID, int urunID)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"DELETE FROM MenuDetay " +
                           $"WHERE menuID='{menuID}' AND urunID='{urunID}'";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc >= 1)
                Console.Out.WriteLine("Ürün, menüden başarılı bir şekilde silindi");
            else
            {
                MessageBox.Show("Ürün, menüden silinemedi.");
                return false;
            }
        }
        catch (Exception hata)
        {
            baglanti.Close();
            MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        return true;
    }

    public DataTable urunleriListele(int menuID)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"SELECT * FROM MenuDetay";

        if (menuID != 0)
        {
            sqlKomutu = $"SELECT U.* " +
                        $"FROM MenuDetay MD " +
                        $"INNER JOIN Urunler U ON MD.urunID = U.urunID " +
                        $"WHERE MD.menuID = '{menuID}';";
        }

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

    public bool menuyuTemizle(int menuID)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"DELETE FROM MenuDetay WHERE menuID='{menuID}'";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc >= 1)
                Console.Out.WriteLine("Menü içeriği başarılı bir şekilde silindi");
            else
            {
                MessageBox.Show("Menü içeriği silinemedi.");
                return false;
            }
        }
        catch (Exception hata)
        {
            baglanti.Close();
            MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        return true;
    }
}