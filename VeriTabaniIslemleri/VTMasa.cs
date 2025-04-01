using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public class VTMasa
{
    // Veri tabanı bağlantısı.
    public string baglantiKodu = "Data Source=DESKTOP-HSH38D0;Initial Catalog=RestoranModulu;Integrated Security=True";

    // Tablo üzerindeki verileri çeker ve Data Table olarak geri döndürür.
    public DataTable Listele()
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"SELECT * FROM Masalar";

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
    public bool masaEkle(string adi, int sandalyeSayisi, string masaDurumu = null, int katID = 0, string aciklama = null)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"INSERT INTO Masalar(adi, sandalyeSayisi, masaDurumu, aciklama, katID)" +
                                          $" VALUES('{adi}', '{sandalyeSayisi}', '{masaDurumu}', '{aciklama}', '{katID}')";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Masa başarılı bir şekilde eklendi");
            else
            {
                MessageBox.Show("Masa eklenemedi.");
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

    // "Masalar" tablosu içerisinde, ID değeri verilen masanın bilgilerini günceller.
    public bool masaGuncelle(int masaID, string adi, int sandalyeSayisi, string masaDurumu = null, string aciklama = null, int katID = 0)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"UPDATE Masalar " +
                           $"SET adi='{adi}', sandalyeSayisi='{sandalyeSayisi}', masaDurumu='{masaDurumu}', aciklama='{aciklama}', katID='{katID}' " +
                           $"WHERE masaID='{masaID}'";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Masa bilgileri başarılı bir şekilde güncellendi");
            else
            {
                MessageBox.Show("Masa bilgileri güncellenemedi.");
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

    // "Masalar" tablosu içerisinde, ID değeri verilen masanın bilgilerini siler.
    public bool masaSil(int masaID)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"DELETE FROM Masalar WHERE masaID='{masaID}'";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Masa bilgileri başarılı bir şekilde silindi");
            else
            {
                MessageBox.Show("Masa bilgileri silinemedi.");
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

    //  "Masalar" tablosu içinde verilen değerlere göre filtreleme yapar.
    public DataTable masaFiltrele(string adi = null, int sandalyeSayisi = 0, string masaDurumu = null, string aciklama = null, int katID = 0)
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
            if (!(sandalyeSayisi == 0))
            {
                conditions.Add("sandalyeSayisi LIKE @sandalyeSayisi");
                komut.Parameters.AddWithValue("@sandalyeSayisi", sandalyeSayisi);
            }
            if (!string.IsNullOrEmpty(masaDurumu))
            {
                conditions.Add("masaDurumu LIKE @masaDurumu");
                komut.Parameters.AddWithValue("@masaDurumu", masaDurumu);
            }
            if (!string.IsNullOrEmpty(aciklama))
            {
                conditions.Add("aciklama = @aciklama");
                komut.Parameters.AddWithValue("@aciklama", aciklama);
            }
            if (!(katID == 0))
            {
                conditions.Add("katID = @katID");
                komut.Parameters.AddWithValue("@katID", katID);
            }

            // SQL Sorgusunu oluşturma
            string sqlKomutu = "SELECT * FROM Masalar";
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
                    MessageBox.Show("Girilen bilgilere göre bir masa bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    public bool masayiKataEkle(int masaID, int katID)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"UPDATE Masalar " +
                           $"SET katID='{katID}' " +
                           $"WHERE masaID='{masaID}'";
        try
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand(sqlKomutu, baglanti);
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc == 1)
                Console.Out.WriteLine("Masa başarılı bir şekilde kata eklendi");
            else
            {
                MessageBox.Show("Masa kata eklenemedi.");
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