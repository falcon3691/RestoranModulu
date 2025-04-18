using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

public class VTRoller
{
    public string baglantiKodu = "Server=localhost; Database=restoranmodulu; Uid=root; Pwd=Malukat3691.;";

    public DataTable Listele()
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = $"SELECT * FROM Roller";
            MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
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

    public void rolEkle(string adi, string aciklama, string durumu)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "INSERT INTO Roller(adi, aciklama, durumu) VALUES(@adi, @aciklama, @durumu)";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@adi", adi);
                komut.Parameters.AddWithValue("@aciklama", aciklama);
                komut.Parameters.AddWithValue("@durumu", durumu);

                bool sonuc = komut.ExecuteNonQuery() == 1;
                if (!sonuc)
                    MessageBox.Show("Rol eklenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void rolGuncelle(int rolID, string adi, string aciklama, string durumu)
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

            if (!string.IsNullOrEmpty(aciklama))
            {
                setKisimlari.Add("aciklama = @aciklama");
                komut.Parameters.AddWithValue("@aciklama", aciklama);
            }

            if (!string.IsNullOrEmpty(durumu))
            {
                setKisimlari.Add("durumu = @durumu");
                komut.Parameters.AddWithValue("@durumu", durumu);
            }
            if (setKisimlari.Count == 0)
                MessageBox.Show("Güncellenecek herhangi bir alan belirtilmedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            string sqlKomutu = $"UPDATE roller SET {string.Join(", ", setKisimlari)} WHERE rolID = @rolID";
            komut.CommandText = sqlKomutu;
            komut.Parameters.AddWithValue("@rolID", rolID);
            komut.Connection = baglanti;

            try
            {
                baglanti.Open();
                bool sonuc = komut.ExecuteNonQuery() == 1;
                if (!sonuc)
                    MessageBox.Show("Rol bilgileri güncellenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void rolSil(int rolID)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "DELETE FROM Roller WHERE rolID=@rolID";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@rolID", rolID);

                bool sonuc = komut.ExecuteNonQuery() == 1;
                if (!sonuc)
                    MessageBox.Show("Rol bilgileri silinemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}