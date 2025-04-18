using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

public class VTKategoriler
{
    public string baglantiKodu = "Server=localhost; Database=restoranmodulu; Uid=root; Pwd=Malukat3691.;";

    public DataTable Listele()
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT * FROM kategoriler";
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
    public void kategoriEkle(string adi, string aciklama)
    {
        bool sonuc = false;
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "INSERT INTO Kategoriler(adi, aciklama) VALUES(@adi, @aciklama)";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@adi", adi);
                komut.Parameters.AddWithValue("@aciklama", aciklama);

                sonuc = (komut.ExecuteNonQuery() == 1) ? true : false;
                if (!sonuc)
                    MessageBox.Show("Kullanıcı eklenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void kategoriGuncelle(int kategoriID, string adi, string aciklama)
    {
        bool sonuc = false;
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            List<string> setKisimlari = new List<string>();
            MySqlCommand komut = new MySqlCommand();

            if (!string.IsNullOrEmpty(adi))
            {
                setKisimlari.Add("adi = @adi");
                komut.Parameters.AddWithValue("@adi", adi);
            }

            if (!string.IsNullOrEmpty(aciklama))
            {
                setKisimlari.Add("aciklama = @aciklama");
                komut.Parameters.AddWithValue("@aciklama", aciklama);
            }

            if (setKisimlari.Count == 0)
                MessageBox.Show("Güncellenecek herhangi bir alan belirtilmedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            string sqlKomutu = $"UPDATE kategoriler SET {string.Join(", ", setKisimlari)} WHERE kategoriID = @kategoriID";
            komut.CommandText = sqlKomutu;
            komut.Parameters.AddWithValue("@kategoriID", kategoriID);
            komut.Connection = baglanti;

            try
            {
                baglanti.Open();
                sonuc = (komut.ExecuteNonQuery() == 1) ? true : false;
                if (!sonuc)
                    MessageBox.Show("Kategori bilgileri güncellenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void kategoriSil(int kategoriID)
    {
        bool sonuc = false;
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "DELETE FROM kategoriler WHERE kategoriID=@kategoriID";
            try
            {

                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@kategoriID", kategoriID);

                sonuc = (komut.ExecuteNonQuery() == 1) ? true : false;
                if (!sonuc)
                    MessageBox.Show("Kategori bilgileri silinemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}