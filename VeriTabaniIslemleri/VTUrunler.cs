using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public class VTUrunler
{
    // Veri tabanı bağlantısı.
    public string baglantiKodu = "Data Source=DESKTOP-HSH38D0;Initial Catalog=RestoranModulu;Integrated Security=True";

    // Tablo üzerindeki verileri çeker ve Data Table olarak geri döndürür.
    public DataTable Listele()
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"SELECT * FROM Urunler";
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
    public bool urunEkle(string adi, int kategoriID, int fiyati, int miktar, string durumu = null,
                              string resimYolu = null, string aciklama = null)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"INSERT INTO Urunler(adi, kategoriID, fiyati, miktar, resimYolu, durumu, aciklama)" +
                                          $" VALUES('{adi}', '{kategoriID}', '{fiyati}', '{miktar}', '{resimYolu}', '{durumu}', '{aciklama}')";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Ürün başarılı bir şekilde eklendi");
            else
            {
                MessageBox.Show("Ürün eklenemedi.");
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

    // "Urunler" tablosu içerisinde, ID değeri verilen ürünün bilgilerini günceller.
    public bool urunGuncelle(int urunID, string adi, int kategoriID, int fiyati, int miktar, string durumu = null,
                              string resimYolu = null, string aciklama = null)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"UPDATE Urunler " +
                           $"SET adi='{adi}', kategoriID='{kategoriID}', fiyati='{fiyati}', miktar='{miktar}', durumu='{durumu}', resimYolu='{resimYolu}', aciklama='{aciklama} '" +
                           $"WHERE urunID='{urunID}'";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Ürün bilgileri başarılı bir şekilde güncellendi");
            else
            {
                MessageBox.Show("Ürün bilgileri güncellenemedi.");
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

    // "Urunler" tablosu içerisinde, ID değeri verilen ürünün bilgilerini siler.
    public bool urunSil(int urunID)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"DELETE FROM Urunler WHERE urunID='{urunID}'";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Ürün bilgileri başarılı bir şekilde silindi");
            else
            {
                MessageBox.Show("Ürün bilgileri silinemedi.");
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

    //  "Urunler" tablosu içinde verilen değerlere göre filtreleme yapar.
    public DataTable urunFiltrele(string adi = null, int kategoriID = 0, int fiyati = 0, int miktar = 0,
                                   string durumu = null, string resimYolu = null, string aciklama = null)
    {
        using (SqlConnection baglanti = new SqlConnection(baglantiKodu))
        {
            List<string> conditions = new List<string>(); // Filtreleri tutacak liste
            SqlCommand komut = new SqlCommand();

            if (!string.IsNullOrEmpty(adi))
            {
                conditions.Add("adi LIKE @adi");
                komut.Parameters.AddWithValue("@adi", "%" + adi + "%");
            }
            if (!string.IsNullOrEmpty(kategoriID.ToString()))
            {
                conditions.Add("kategoriID LIKE @kategoriID");
                komut.Parameters.AddWithValue("@kategoriID", kategoriID);
            }
            if (!string.IsNullOrEmpty(fiyati.ToString()))
            {
                conditions.Add("fiyati LIKE @fiyati");
                komut.Parameters.AddWithValue("@fiyati", fiyati);
            }
            if (!string.IsNullOrEmpty(miktar.ToString()))
            {
                conditions.Add("miktar = @miktar");
                komut.Parameters.AddWithValue("@miktar", miktar);
            }
            if (!string.IsNullOrEmpty(durumu))
            {
                conditions.Add("durumu = @durumu");
                komut.Parameters.AddWithValue("@durumu", durumu);
            }
            if (!string.IsNullOrEmpty(resimYolu))
            {
                conditions.Add("resimYolu LIKE @resimYolu");
                komut.Parameters.AddWithValue("@resimYolu", "%" + resimYolu + "%");
            }
            if (!string.IsNullOrEmpty(aciklama))
            {
                conditions.Add("aciklama LIKE @aciklama");
                komut.Parameters.AddWithValue("@aciklama", "%" + aciklama + "%");
            }

            // SQL Sorgusunu oluşturma
            string sqlKomutu = "SELECT * FROM Urunler";
            if (conditions.Count > 0)
            {
                sqlKomutu += " WHERE " + string.Join(" OR ", conditions); // OR yerine AND kullanıldı, istenirse değiştirilebilir
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
                    MessageBox.Show("Girilen bilgilere göre bir ürün bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

}