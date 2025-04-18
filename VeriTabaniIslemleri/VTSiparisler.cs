using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

public class VTSiparisler
{
    public string baglantiKodu = "Server=localhost; Database=restoranmodulu; Uid=root; Pwd=Malukat3691.;";

    public DataTable Listele(int masaID)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT * FROM Siparisler WHERE masaID = @masaID " +
                               "ORDER BY olusturmaTarihi DESC LIMIT 1";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@masaID", masaID);

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

    public DataTable Listele2(int masaID)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT * FROM siparisler WHERE masaID = @masaID";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@masaID", masaID);

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

    public DataTable detayListele(int siparisID, bool filtre = false)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT urunID, urunAdi, miktar, birimFiyat, toplamFiyat, durumu, siparisDetayID FROM siparisdetay WHERE siparisID = @siparisID";

            if (filtre)
                sqlKomutu += " AND durumu!='ödendi'";

            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@siparisID", siparisID);

                MySqlDataAdapter da = new MySqlDataAdapter(komut);
                da.Fill(dt);
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }
    }

    public DataTable detayListele2(int siparisID, bool filtre = false)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT * FROM SiparisDetay WHERE siparisID = @siparisID";

            if (filtre)
                sqlKomutu += " AND durumu='ödenmedi'";

            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@siparisID", siparisID);
                MySqlDataAdapter da = new MySqlDataAdapter(komut);
                da.Fill(dt);
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }
    }

    public DataTable tumSiparisleriListele()
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT * FROM siparisler";
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

    public void siparisDetayEkle(int siparisID, int urunID, int miktar, int birimFiyat, int toplamFiyat, string durumu, string urunAdi)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "INSERT INTO siparisdetay(siparisID, urunID, miktar, birimFiyat, toplamFiyat, durumu, urunAdi) " +
                                               "VALUES(@siparisID, @urunID, @miktar, @birimFiyat, @toplamFiyat, @durumu, @urunAdi)";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@siparisID", siparisID);
                komut.Parameters.AddWithValue("@urunID", urunID);
                komut.Parameters.AddWithValue("@miktar", miktar);
                komut.Parameters.AddWithValue("@birimFiyat", birimFiyat);
                komut.Parameters.AddWithValue("@toplamFiyat", toplamFiyat);
                komut.Parameters.AddWithValue("@durumu", durumu);
                komut.Parameters.AddWithValue("@urunAdi", urunAdi);

                bool sonuc = komut.ExecuteNonQuery() == 1;
                if (!sonuc)
                    MessageBox.Show("Detay bilgileri eklenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void siparisDetayGuncelle(int siparisDetayID, string durumu)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "UPDATE SiparisDetay " +
                               "SET durumu=@durumu " +
                               "WHERE siparisDetayID=@siparisDetayID";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@durumu", durumu);
                komut.Parameters.AddWithValue("@siparisDetayID", siparisDetayID);
                bool sonuc = komut.ExecuteNonQuery() == 1;
                if (!sonuc)
                    MessageBox.Show("Sipariş detayı güncellenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void siparisSil(int siparisID)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "DELETE FROM Siparisler WHERE siparisID=@siparisID";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@siparisID", siparisID);

                bool sonuc = komut.ExecuteNonQuery() == 1;
                if (!sonuc)
                    MessageBox.Show("Sipariş bilgileri silinemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void siparisEkle(int masaID, int kullaniciID = 0)
    {
        string tarih = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "INSERT INTO siparisler(masaID, kullaniciID, toplamFiyat, durumu, olusturmaTarihi)" +
                                               "VALUES(@masaID, @kullaniciID, 0, 'oluşturuldu', @tarih)";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@masaID", masaID);
                komut.Parameters.AddWithValue("@kullaniciID", kullaniciID);
                komut.Parameters.AddWithValue("@tarih", tarih);

                bool sonuc = komut.ExecuteNonQuery() == 1;
                if (!sonuc)
                    MessageBox.Show("Sipariş bilgisi eklenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void siparisGuncelle(int siparisID, int toplamFiyat, string durumu)
    {
        string tarih = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "UPDATE siparisler " +
                               "SET toplamFiyat=@toplamFiyat, durumu=@durumu, olusturmaTarihi=@tarih " +
                               "WHERE siparisID=@siparisID";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@toplamFiyat", toplamFiyat);
                komut.Parameters.AddWithValue("@durumu", durumu);
                komut.Parameters.AddWithValue("@tarih", tarih);
                komut.Parameters.AddWithValue("@siparisID", siparisID);

                bool sonuc = komut.ExecuteNonQuery() == 1;
                if (!sonuc)
                    MessageBox.Show("Sipariş bilgisi güncellenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public DataTable siparisFiltrele(DateTime ilkTarih, DateTime sonTarih, bool check = false, int masaID = 0, int kullaniciID = 0)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            List<string> conditions = new List<string>();
            MySqlCommand komut = new MySqlCommand();

            if (masaID != 0)
            {
                conditions.Add("masaID=@masaID");
                komut.Parameters.AddWithValue("@masaID", masaID);
            }
            if (kullaniciID != 0)
            {
                conditions.Add("kullaniciID=@kullaniciID");
                komut.Parameters.AddWithValue("@kullaniciID", kullaniciID);
            }
            if (!check)
            {
                conditions.Add("(olusturmaTarihi BETWEEN @ilkTarih1 AND @ilkTarih2)");
                komut.Parameters.Add("@ilkTarih1", MySqlDbType.DateTime).Value = Convert.ToDateTime(ilkTarih.Date.ToString("yyyy-MM-dd 00:00:00"));
                komut.Parameters.Add("@ilkTarih2", MySqlDbType.DateTime).Value = Convert.ToDateTime(ilkTarih.ToString("yyyy-MM-dd 23:59:59"));
            }
            if (check)
            {
                conditions.Add("(olusturmaTarihi BETWEEN @ilkTarih1 AND @ilkTarih2)");
                komut.Parameters.Add("@ilkTarih1", MySqlDbType.DateTime).Value = Convert.ToDateTime(ilkTarih.Date.ToString("yyyy-MM-dd 00:00:00"));
                komut.Parameters.Add("@ilkTarih2", MySqlDbType.DateTime).Value = Convert.ToDateTime(sonTarih.ToString("yyyy-MM-dd 23:59:59"));
            }

            string sqlKomutu = "SELECT * FROM siparisler";
            if (conditions.Count > 0)
                sqlKomutu += " WHERE " + string.Join(" OR ", conditions);

            komut.CommandText = sqlKomutu;
            komut.Connection = baglanti;

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
        return dt;
    }

    public DataTable detayFiltrele(string urunAdi, int siparisID = 0)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            List<string> conditions = new List<string>();
            MySqlCommand komut = new MySqlCommand();

            if (!string.IsNullOrEmpty(urunAdi))
            {
                conditions.Add("urunAdi LIKE @urunAdi");
                komut.Parameters.AddWithValue("@urunAdi", "%" + urunAdi + "%");
            }
            if (siparisID != 0)
            {
                conditions.Add("siparisID = @siparisID");
                komut.Parameters.AddWithValue("@siparisID", siparisID);
            }

            string sqlKomutu = "SELECT * FROM siparisdetay";
            if (conditions.Count > 0)
                sqlKomutu += " WHERE " + string.Join(" AND ", conditions);

            komut.CommandText = sqlKomutu;
            komut.Connection = baglanti;

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
            return dt;
        }
    }

    public DataTable detayFiltrele2(int kategoriID = 0, int siparisID = 0)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            List<string> conditions = new List<string>();
            MySqlCommand komut = new MySqlCommand();

            if (kategoriID != 0)
            {
                conditions.Add("kategoriID = @kategoriID");
                komut.Parameters.AddWithValue("@kategoriID", "%" + kategoriID + "%");
            }
            if (siparisID != 0)
            {
                conditions.Add("siparisID = @siparisID");
                komut.Parameters.AddWithValue("@siparisID", siparisID);
            }

            string sqlKomutu = "SELECT * FROM siparisdetay";
            if (conditions.Count > 0)
                sqlKomutu += " WHERE " + string.Join(" AND ", conditions);

            komut.CommandText = sqlKomutu;
            komut.Connection = baglanti;

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
            return dt;
        }
    }

    public DataTable enSonSiparisiBul(int masaID)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT durumu FROM siparisler WHERE masaID = @masaID ORDER BY siparisID DESC LIMIT 1;";

            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@masaID", masaID);

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