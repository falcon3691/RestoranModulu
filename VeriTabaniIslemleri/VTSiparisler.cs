using System;
using System.Collections.Generic;
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

    public DataTable detayListele2(int siparisID, bool filtre = false)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"SELECT * FROM SiparisDetay WHERE siparisID = '{siparisID}'";

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

    public DataTable tumSiparisleriListele()
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
                                          $" VALUES('{masaID}', '{kullaniciID}', '{0}', 'oluşturuldu', '{DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"))}')";
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
                           $"SET toplamFiyat='{toplamFiyat}', durumu='{durumu}', olusturmaTarihi='{DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"))}' " +
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

    public DataTable siparisFiltrele(int masaID, int kullaniciID, DateTime ilkTarih, DateTime sonTarih, bool check)
    {
        using (SqlConnection baglanti = new SqlConnection(baglantiKodu))
        {
            List<string> conditions = new List<string>(); // Filtreleri tutacak liste
            SqlCommand komut = new SqlCommand();

            if (!string.IsNullOrEmpty(masaID.ToString()) && masaID != 0)
            {
                conditions.Add("masaID=@masaID");
                komut.Parameters.AddWithValue("@masaID", masaID);
            }
            if (!string.IsNullOrEmpty(kullaniciID.ToString()) && kullaniciID != 0)
            {
                conditions.Add("kullaniciID=@kullaniciID");
                komut.Parameters.AddWithValue("@kullaniciID", kullaniciID);
            }
            if (!string.IsNullOrEmpty(ilkTarih.ToString()) && !check)
            {
                conditions.Add("olusturmaTarihi BETWEEN @ilkTarih1 AND @ilkTarih2");
                komut.Parameters.Add("@ilkTarih1", SqlDbType.DateTime).Value = ilkTarih.Date.ToString("MM-dd-yyyy 00:00:00");
                komut.Parameters.Add("@ilkTarih2", SqlDbType.DateTime).Value = ilkTarih.ToString("MM-dd-yyyy  23:59:59");
            }
            if (check)
            {
                conditions.Add("olusturmaTarihi BETWEEN @ilkTarih1 AND @ilkTarih2");
                komut.Parameters.Add("@ilkTarih1", SqlDbType.DateTime).Value = ilkTarih.Date.ToString("MM-dd-yyyy 00:00:00");
                komut.Parameters.Add("@ilkTarih2", SqlDbType.DateTime).Value = sonTarih.ToString("MM-dd-yyyy  23:59:59");
            }

            // SQL Sorgusunu oluşturma
            string sqlKomutu = "SELECT * FROM Siparisler";
            if (conditions.Count > 0)
            {
                sqlKomutu += " WHERE " + string.Join(" OR ", conditions);
            }
            else
                return null;

            komut.CommandText = sqlKomutu;
            komut.Connection = baglanti;

            try
            {
                baglanti.Open();
                SqlDataAdapter da = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                da.Fill(dt);
                baglanti.Close();

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Girilen bilgilere göre sipariş bilgisi bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                else
                {
                    return dt;
                }
            }
            catch (Exception hata)
            {
                baglanti.Close();
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }


    public DataTable detayFiltrele(string urunAdi = null, int siparisID = 0)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        if (urunAdi != null && siparisID != 0)
        {
            string sqlKomutu = $"SELECT * FROM SiparisDetay WHERE urunAdi LIKE '%{urunAdi}%' AND siparisID='{siparisID}'";

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
        }
        return null;
    }

}