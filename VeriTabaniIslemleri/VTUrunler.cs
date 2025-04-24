using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

public class VTUrunler
{
    public string baglantiKodu = "Server=localhost; Database=restoranmodulu; Uid=root; Pwd=Malukat3691.;";

    public DataTable Listele()
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT urunler.urunID, urunler.adi, urunler.fiyati, urunler.miktar, urunler.durumu, urunler.resimYolu, urunler.aciklama, kategoriler.adi AS kategoriAdi FROM urunler " +
                               "JOIN kategoriler ON urunler.kategoriID = kategoriler.kategoriID";

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

    public void urunEkle(string adi, int kategoriID, int fiyati, int miktar, string durumu = null,
                              string resimYolu = null, string aciklama = null)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "INSERT INTO urunler(adi, kategoriID, fiyati, miktar, resimYolu, durumu, aciklama)" +
                                              " VALUES(@adi, @kategoriID, @fiyati, @miktar, @resimYolu, @durumu, @aciklama)";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@adi", adi);
                komut.Parameters.AddWithValue("@kategoriID", kategoriID);
                komut.Parameters.AddWithValue("@fiyati", fiyati);
                komut.Parameters.AddWithValue("@miktar", miktar);
                komut.Parameters.AddWithValue("@resimYolu", resimYolu);
                komut.Parameters.AddWithValue("@durumu", durumu);
                komut.Parameters.AddWithValue("@aciklama", aciklama);

                bool sonuc = komut.ExecuteNonQuery() == 1;
                if (!sonuc)
                    MessageBox.Show("Ürün eklenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void urunGuncelle(int urunID, string adi, string durumu, string resimYolu, string aciklama, int kategoriID = 0, int fiyati = 0, int miktar = 0)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            List<string> setKisimlari = new List<string>();
            MySqlCommand komut = new MySqlCommand();
            if (!string.IsNullOrEmpty(adi))
            {
                setKisimlari.Add("adi = @adi");
                komut.Parameters.AddWithValue("@adi", adi);
            }
            if (!string.IsNullOrEmpty(durumu))
            {
                setKisimlari.Add("durumu = @durumu");
                komut.Parameters.AddWithValue("@durumu", durumu);
            }
            if (!string.IsNullOrEmpty(resimYolu))
            {
                setKisimlari.Add("resimYolu = @resimYolu");
                komut.Parameters.AddWithValue("@resimYolu", resimYolu);
            }
            if (!string.IsNullOrEmpty(aciklama))
            {
                setKisimlari.Add("aciklama = @aciklama");
                komut.Parameters.AddWithValue("@aciklama", aciklama);
            }
            if (kategoriID != 0)
            {
                setKisimlari.Add("kategoriID = @kategoriID");
                komut.Parameters.AddWithValue("@kategoriID", kategoriID);
            }
            else if (kategoriID == 0)
                setKisimlari.Add("kategoriID = 0");

            if (fiyati != 0)
            {
                setKisimlari.Add("fiyati = @fiyati");
                komut.Parameters.AddWithValue("@fiyati", fiyati);
            }
            else if (fiyati == 0)
                setKisimlari.Add("fiyati = 0");

            if (miktar != 0)
            {
                setKisimlari.Add("miktar = @miktar");
                komut.Parameters.AddWithValue("@miktar", miktar);
            }
            else if (miktar == 0)
                setKisimlari.Add("miktar = 0");

            if (setKisimlari.Count == 0)
                MessageBox.Show("Güncellenecek herhangi bir alan belirtilmedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            string sqlKomutu = $"UPDATE urunler SET {string.Join(", ", setKisimlari)} WHERE urunID = @urunID";
            komut.CommandText = sqlKomutu;
            komut.Parameters.AddWithValue("@urunID", urunID);
            komut.Connection = baglanti;
            try
            {
                baglanti.Open();
                bool sonuc = komut.ExecuteNonQuery() == 1;
                if (!sonuc)
                    MessageBox.Show("Ürün bilgileri güncellenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void urunSil(int urunID)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "DELETE FROM Urunler WHERE urunID=@urunID";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@urunID", urunID);
                bool sonuc = komut.ExecuteNonQuery() == 1;
                if (!sonuc)
                    Console.Out.WriteLine("Ürün bilgileri başarılı bir şekilde silindi");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public DataTable urunFiltrele(string adi, string durumu, string resimYolu, string aciklama, int kategoriID = 0, int fiyati = 0, int miktar = 0)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            List<string> conditions = new List<string>();
            MySqlCommand komut = new MySqlCommand();

            if (!string.IsNullOrEmpty(adi))
            {
                conditions.Add("urunler.adi LIKE @adi");
                komut.Parameters.AddWithValue("@adi", "%" + adi + "%");
            }
            if (!string.IsNullOrEmpty(durumu))
            {
                conditions.Add("urunler.durumu = @durumu");
                komut.Parameters.AddWithValue("@durumu", durumu);
            }
            if (!string.IsNullOrEmpty(resimYolu))
            {
                conditions.Add("urunler.resimYolu LIKE @resimYolu");
                komut.Parameters.AddWithValue("@resimYolu", "%" + resimYolu + "%");
            }
            if (!string.IsNullOrEmpty(aciklama))
            {
                conditions.Add("urunler.aciklama LIKE @aciklama");
                komut.Parameters.AddWithValue("@aciklama", "%" + aciklama + "%");
            }
            if (kategoriID != 0)
            {
                conditions.Add("urunler.kategoriID = @kategoriID");
                komut.Parameters.AddWithValue("@kategoriID", kategoriID);
            }
            if (fiyati != 0)
            {
                conditions.Add("urunler.fiyati = @fiyati");
                komut.Parameters.AddWithValue("@fiyati", fiyati);
            }
            if (miktar != 0)
            {
                conditions.Add("urunler.miktar = @miktar");
                komut.Parameters.AddWithValue("@miktar", miktar);
            }

            string sqlKomutu = "SELECT urunler.urunID, urunler.adi, urunler.fiyati, urunler.miktar, urunler.durumu, urunler.resimYolu, urunler.aciklama, kategoriler.adi AS kategoriAdi " +
                               "FROM urunler " +
                               "JOIN kategoriler ON urunler.kategoriID = kategoriler.kategoriID";
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

    public DataTable kategoriListele()
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT kategoriID, adi FROM kategoriler";
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
                    MessageBox.Show(hata.Message, "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        return dt;
    }
}