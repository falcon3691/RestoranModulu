using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

public class VTKatlar
{
    public string baglantiKodu = "Server=localhost; Database=restoranmodulu; Uid=root; Pwd=Malukat3691.;";

    public DataTable Listele()
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT * FROM katlar";
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

    public void katEkle(string adi, string durumu, string aciklama)
    {
        bool sonuc = false;
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "INSERT INTO Katlar(adi, durumu, aciklama) VALUES(@adi, @durumu, @aciklama)";
            try
            {

                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@adi", adi);
                komut.Parameters.AddWithValue("@durumu", durumu);
                komut.Parameters.AddWithValue("@aciklama", aciklama);

                sonuc = (komut.ExecuteNonQuery() == 1) ? true : false;
                if (!sonuc)
                    MessageBox.Show("Kat eklenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void katGuncelle(int katID, string adi, string durumu, string aciklama)
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

            if (!string.IsNullOrEmpty(durumu))
            {
                setKisimlari.Add("durumu = @durumu");
                komut.Parameters.AddWithValue("@durumu", durumu);
            }

            if (!string.IsNullOrEmpty(aciklama))
            {
                setKisimlari.Add("aciklama = @aciklama");
                komut.Parameters.AddWithValue("@aciklama", aciklama);
            }

            if (setKisimlari.Count == 0)
                MessageBox.Show("Güncellenecek herhangi bir alan belirtilmedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            string sqlKomutu = $"UPDATE katlar SET {string.Join(", ", setKisimlari)} WHERE katID = @katID";
            komut.CommandText = sqlKomutu;
            komut.Parameters.AddWithValue("@katID", katID);
            komut.Connection = baglanti;

            try
            {
                baglanti.Open();
                sonuc = (komut.ExecuteNonQuery() == 1) ? true : false;
                if (!sonuc)
                    MessageBox.Show("Kat bilgileri güncellenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void katSil(int katID)
    {

        bool sonuc = false;
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "DELETE FROM Katlar WHERE katID=@katID";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@katID", katID);

                sonuc = (komut.ExecuteNonQuery() == 1) ? true : false;
                if (!sonuc)
                    MessageBox.Show("Kat bilgileri silinemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public int masaSayisi(int katID)
    {
        int masaSayisi = 0;
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT COUNT(*) FROM Masalar WHERE katID=@katID";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@katID", katID);

                masaSayisi = Convert.ToInt32(komut.ExecuteScalar());
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        return masaSayisi;
    }
}