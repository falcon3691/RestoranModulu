using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public class VTSiparisler
{
    // Veri tabanı bağlantısı.
    public string baglantiKodu = "Data Source=DESKTOP-HSH38D0;Initial Catalog=RestoranModulu;Integrated Security=True";

    // Tablo üzerindeki verileri çeker ve Data Table olarak geri döndürür.
    public DataTable Listele(int masaID)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"SELECT * FROM Siparisler WHERE masaID = '{masaID}' AND " +
                                                            $"olusturmaTarihi = (SELECT MAX(olusturmaTarihi) FROM Siparisler WHERE masaID = '{masaID}')";

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

    public DataTable Listele2(int masaID)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"SELECT * FROM Siparisler WHERE masaID = '{masaID}'";

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

    public DataTable detayListele(int siparisID, bool filtre = false)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"SELECT urunID, urunAdi, miktar, birimFiyat, toplamFiyat, siparisDetayID FROM SiparisDetay WHERE siparisID = '{siparisID}'";

        if (filtre)
            sqlKomutu += $" AND durumu='ödenmedi'";

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

    public bool siparisDetayEkle(int siparisID, int urunID, int miktar, int birimFiyat, int toplamFiyat, string durumu, string urunAdi)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"INSERT INTO SiparisDetay(siparisID, urunID, miktar, birimFiyat, toplamFiyat, durumu, urunAdi)" +
                                          $" VALUES('{siparisID}', '{urunID}', '{miktar}', '{birimFiyat}', '{toplamFiyat}', '{durumu}', '{urunAdi}')";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Detay bilgileri başarılı bir şekilde eklendi");
            else
            {
                MessageBox.Show("Detay bilgileri eklenemedi.");
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

    public bool siparisDetayGuncelle(int siparisDetayID, string durumu)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"UPDATE SiparisDetay " +
                           $"SET durumu='{durumu}' " +
                           $"WHERE siparisDetayID='{siparisDetayID}'";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Sipariş detayı başarılı bir şekilde güncellendi");
            else
            {
                MessageBox.Show("Sipariş detayı güncellenemedi.");
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

    public bool siparisSil(int siparisID)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"DELETE FROM Siparisler WHERE siparisID='{siparisID}'";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Sipariş bilgileri başarılı bir şekilde silindi");
            else
            {
                MessageBox.Show("Sipariş bilgileri silinemedi.");
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

    public bool siparisEkle(int masaID, int kullaniciID = 0)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"INSERT INTO Siparisler(masaID, kullaniciID, toplamFiyat, durumu, olusturmaTarihi)" +
                                          $" VALUES('{masaID}', '{kullaniciID}', '{0}', 'oluşturuldu', '{DateTime.Now}')";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Sipariş bilgisi başarılı bir şekilde eklendi");
            else
            {
                MessageBox.Show("Sipariş bilgisi eklenemedi.");
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

    public bool siparisGuncelle(int siparisID, int toplamFiyat, string durumu)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"UPDATE Siparisler " +
                           $"SET toplamFiyat='{toplamFiyat}', durumu='{durumu}', olusturmaTarihi='{DateTime.Now}' " +
                           $"WHERE siparisID='{siparisID}'";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Sipariş bilgisi başarılı bir şekilde güncellendi");
            else
            {
                MessageBox.Show("Sipariş bilgisi güncellenemedi.");
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