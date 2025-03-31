using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public class VTKullanicilar
{
    // Veri tabanı bağlantısı.
    public string baglantiKodu = "Data Source=DESKTOP-HSH38D0;Initial Catalog=RestoranModulu;Integrated Security=True";

    // Sadece verilen tablo üzerindeki veriler çeker ve Data Table olarak geri döndürür.
    public DataTable Listele(string tabloAdi)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"SELECT * FROM {tabloAdi}";
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

    // "Kullanicilar" tablosuna veri ekler.
    public bool KullaniciEkle(string adiSoyadi, string kullaniciAdi, string parola, int rolID, byte durumu = 1,
                              string aciklama = null, string telefon = null, string eMail = null)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"INSERT INTO Kullanicilar(adiSoyadi, telefon, eMail, kullaniciAdi, parola, rolID, durumu, aciklama)" +
                                          $" VALUES('{adiSoyadi}', '{telefon}', '{eMail}', '{kullaniciAdi}', '{parola}', '{rolID}', '{durumu}', '{aciklama}')";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Kullanıcı başarılı bir şekilde eklendi");
            else
            {
                MessageBox.Show("Kullanıcı eklenemedi.");
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

    // "Kullanicilar" tablosu içerisinde, ID değeri verilen kullanıcının bilgilerini günceller.
    public bool KullaniciGuncelle(int kullaniciID, string adiSoyadi, string kullaniciAdi, string parola, int rolID, byte durumu = 1,
                              string aciklama = null, string telefon = null, string eMail = null)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"UPDATE Kullanicilar " +
                           $"SET adiSoyadi='{adiSoyadi}', telefon='{telefon}', eMail='{eMail}', kullaniciAdi='{kullaniciAdi}', parola='{parola}', rolID='{rolID}', durumu='{durumu}', aciklama='{aciklama}' " +
                           $"WHERE kullaniciID='{kullaniciID}'";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Kullanıcı bilgileri başarılı bir şekilde güncellendi");
            else
            {
                MessageBox.Show("Kullanıcı bilgileri güncellenemedi.");
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

    // "Kullanicilar" tablosu içerisinde, ID değeri verilen kullanıcının bilgilerini siler.
    public bool KullaniciSil(int kullaniciID)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"DELETE FROM Kullanicilar WHERE kullaniciID='{kullaniciID}'";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Kullanıcı bilgileri başarılı bir şekilde silindi");
            else
            {
                MessageBox.Show("Kullanıcı bilgileri silinemedi.");
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

    //  "Kullanicilar" tablosu içinde verilen değerlere göre filtreleme yapar.
    public DataTable kullaniciFiltrele(string adiSoyadi = null, string kullaniciAdi = null, string parola = null, string rolID = null,
                                   string durumu = null, string aciklama = null, string telefon = null, string eMail = null)
    {
        using (SqlConnection baglanti = new SqlConnection(baglantiKodu))
        {
            List<string> conditions = new List<string>(); // Filtreleri tutacak liste
            SqlCommand komut = new SqlCommand();

            if (!string.IsNullOrEmpty(adiSoyadi))
            {
                conditions.Add("adiSoyadi LIKE @adiSoyadi");
                komut.Parameters.AddWithValue("@adiSoyadi", "%" + adiSoyadi + "%");
            }
            if (!string.IsNullOrEmpty(kullaniciAdi))
            {
                conditions.Add("kullaniciAdi LIKE @kullaniciAdi");
                komut.Parameters.AddWithValue("@kullaniciAdi", "%" + kullaniciAdi + "%");
            }
            if (!string.IsNullOrEmpty(parola))
            {
                conditions.Add("parola LIKE @parola");
                komut.Parameters.AddWithValue("@parola", "%" + parola + "%");
            }
            if (!string.IsNullOrEmpty(rolID))
            {
                conditions.Add("rolID = @rolID");
                komut.Parameters.AddWithValue("@rolID", int.Parse(rolID));
            }
            if (!string.IsNullOrEmpty(durumu))
            {
                conditions.Add("durumu = @durumu");
                komut.Parameters.AddWithValue("@durumu", byte.Parse(durumu));
            }
            if (!string.IsNullOrEmpty(aciklama))
            {
                conditions.Add("aciklama LIKE @aciklama");
                komut.Parameters.AddWithValue("@aciklama", "%" + aciklama + "%");
            }
            if (!string.IsNullOrEmpty(telefon))
            {
                conditions.Add("telefon LIKE @telefon");
                komut.Parameters.AddWithValue("@telefon", "%" + telefon + "%");
            }
            if (!string.IsNullOrEmpty(eMail))
            {
                conditions.Add("eMail LIKE @eMail");
                komut.Parameters.AddWithValue("@eMail", "%" + eMail + "%");
            }

            // SQL Sorgusunu oluşturma
            string sqlKomutu = "SELECT * FROM Kullanicilar";
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
                    MessageBox.Show("Girilen bilgilere göre bir kullanıcı bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
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