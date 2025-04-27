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
            string sqlKomutu = "SELECT * FROM siparisler WHERE masaID = @masaID AND durumu!='ödendi'";
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
            string sqlKomutu = "SELECT urunID, urunAdi, miktar, birimFiyat, toplamFiyat, durumu, siparisDetayID, detayNot, siparisID FROM siparisdetay WHERE siparisID = @siparisID";

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
            string sqlKomutu = "SELECT siparisDetayID, siparisID, urunAdi, miktar, birimFiyat, toplamFiyat, durumu, detayNot " +
                               "FROM SiparisDetay WHERE siparisID = @siparisID";

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
            string sqlKomutu = "SELECT siparisler.siparisID, siparisler.toplamFiyat, siparisler.durumu, siparisler.olusturmaTarihi, siparisler.siparisNot, " +
                               "masalar.adi AS MasaAdi, kullanicilar.adiSoyadi AS AdiSoyadi FROM siparisler " +
                               "JOIN masalar ON siparisler.masaID=masalar.masaID " +
                               "JOIN kullanicilar ON siparisler.kullaniciID=kullanicilar.kullaniciID";
            using (MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti))
            {
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

    public void siparisDetayEkle(int siparisID, int urunID, int miktar, int birimFiyat, int toplamFiyat, string durumu, string urunAdi, string detayNot)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "INSERT INTO siparisdetay(siparisID, urunID, miktar, birimFiyat, toplamFiyat, durumu, urunAdi, detayNot) " +
                                               "VALUES(@siparisID, @urunID, @miktar, @birimFiyat, @toplamFiyat, @durumu, @urunAdi, @detayNot)";
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
                komut.Parameters.AddWithValue("@detayNot", detayNot);

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
            string sqlKomutu = "UPDATE SiparisDetay SET durumu=@durumu WHERE siparisDetayID=@siparisDetayID";
            using (MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti))
            {

                komut.Parameters.AddWithValue("@durumu", durumu);
                komut.Parameters.AddWithValue("@siparisDetayID", siparisDetayID);
                try
                {
                    baglanti.Open();
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
    }

    public void siparisDetaySil(int siparisDetayID)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "DELETE FROM siparisDetay WHERE siparisDetayID=@siparisDetayID";
            using (MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti))
            {
                komut.Parameters.Clear();
                komut.Parameters.AddWithValue("@siparisDetayID", siparisDetayID);
                try
                {
                    baglanti.Open();
                    bool sonuc = komut.ExecuteNonQuery() == 1;
                    if (!sonuc)
                        MessageBox.Show("Sipariş detayı silinemedi.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception hata)
                {
                    MessageBox.Show(hata.Message, "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
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

    public void siparisGuncelle(int siparisID, int toplamFiyat, string durumu, string siparisNot, int iskontoDegeri)
    {
        string tarih = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            List<string> setKisimlari = new List<string>();
            MySqlCommand komut = new MySqlCommand();

            if (toplamFiyat != 0)
            {
                setKisimlari.Add("toplamFiyat = @toplamFiyat");
                komut.Parameters.AddWithValue("@toplamFiyat", toplamFiyat);
            }

            if (!string.IsNullOrEmpty(durumu))
            {
                setKisimlari.Add("durumu = @durumu");
                komut.Parameters.AddWithValue("@durumu", durumu);
            }

            if (!string.IsNullOrEmpty(siparisNot))
            {
                setKisimlari.Add("siparisNot = @siparisNot");
                komut.Parameters.AddWithValue("@siparisNot", siparisNot);
            }

            if (!string.IsNullOrEmpty(siparisNot))
            {
                setKisimlari.Add("siparisNot = @siparisNot");
                komut.Parameters.AddWithValue("@siparisNot", siparisNot);
            }
            if (iskontoDegeri != 0)
            {
                setKisimlari.Add("iskontoDegeri = @iskontoDegeri");
                komut.Parameters.AddWithValue("@iskontoDegeri", iskontoDegeri);
            }
            if (setKisimlari.Count == 0)
                MessageBox.Show("Güncellenecek herhangi bir alan belirtilmedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            string sqlKomutu = $"UPDATE siparisler SET {string.Join(", ", setKisimlari)}, olusturmaTarihi=@tarih WHERE siparisID = @siparisID";
            komut.CommandText = sqlKomutu;
            komut.Parameters.AddWithValue("@siparisID", siparisID);
            komut.Parameters.AddWithValue("@tarih", tarih);
            komut.Connection = baglanti;

            try
            {
                baglanti.Open();
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
            using (MySqlCommand komut = new MySqlCommand())
            {

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

                string sqlKomutu = "SELECT siparisDetayID, siparisID, urunAdi, miktar, birimFiyat, toplamFiyat, durumu, detayNot FROM siparisdetay";
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
            }
        }
        return dt;
    }

    public DataTable detayFiltrele2(string durumu = null, int siparisID = 0)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            List<string> conditions = new List<string>();
            using (MySqlCommand komut = new MySqlCommand())
            {

                if (!string.IsNullOrEmpty(durumu))
                {
                    conditions.Add("durumu LIKE @durumu");
                    komut.Parameters.AddWithValue("@durumu", "%" + durumu + "%");
                }
                if (siparisID != 0)
                {
                    conditions.Add("siparisID = @siparisID");
                    komut.Parameters.AddWithValue("@siparisID", siparisID);
                }

                string sqlKomutu = "SELECT siparisDetayID, siparisID, urunAdi, miktar, birimFiyat, toplamFiyat, durumu, detayNot FROM siparisdetay";
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
            }
        }
        return dt;
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