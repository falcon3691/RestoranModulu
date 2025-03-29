using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public class VeriTabani
{
    // Veri tabanı bağlantısı.
    public string baglantiKodu = "Data Source=DESKTOP-HSH38D0;Initial Catalog=RestoranModulu;Integrated Security=True";

    public VeriTabani()
    {

    }
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


    // GİRİŞ EKRANI //
    // Giriş ekranında kullanıcı kontrolü yaparken kullanılır ve Data Table olarak geri döndürür.
    public DataTable KullaniciListele(string tabloAdi, string kullaniciAdi, string kullaniciParola)
    {
        SqlConnection baglanti = new SqlConnection(baglantiKodu);
        string sqlKomutu = $"SELECT * FROM {tabloAdi} WHERE kullaniciAdi='{kullaniciAdi}' AND parola='{kullaniciParola}'";
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
}
