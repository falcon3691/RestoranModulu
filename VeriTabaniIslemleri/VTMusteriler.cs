using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

public class VTMusteriler
{
    public string baglantiKodu = "Server=localhost; Database=restoranmodulu; Uid=root; Pwd=Malukat3691.;";

    public DataTable Listele()
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT * FROM musteriler";
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

    public void MusteriEkle(string adiSoyadi, int iskontoDegeri)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {

            string sqlKomutu = "INSERT INTO musteriler(adiSoyadi, iskontoDegeri) VALUES(@adiSoyadi, @iskontoDegeri)";
            using (MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti))
            {
                komut.Parameters.AddWithValue("@adiSoyadi", adiSoyadi);
                komut.Parameters.AddWithValue("@iskontoDegeri", iskontoDegeri);
                try
                {
                    baglanti.Open();
                    if (!(komut.ExecuteNonQuery() == 1))
                        MessageBox.Show("Müşteri eklenemedi.");
                }
                catch (Exception hata)
                {
                    MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    public void MusteriGuncelle(int musteriID, string adiSoyadi = null, int iskontoDegeri = 0)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            List<string> setKisimlari = new List<string>();
            MySqlCommand komut = new MySqlCommand();

            if (!string.IsNullOrEmpty(adiSoyadi))
            {
                setKisimlari.Add("adiSoyadi = @adiSoyadi");
                komut.Parameters.AddWithValue("@adiSoyadi", adiSoyadi);
            }

            if (iskontoDegeri != 0)
            {
                setKisimlari.Add("iskontoDegeri = @iskontoDegeri");
                komut.Parameters.AddWithValue("@iskontoDegeri", iskontoDegeri);
            }

            if (setKisimlari.Count == 0)
                MessageBox.Show("Güncellenecek herhangi bir alan belirtilmedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            string sqlKomutu = $"UPDATE musteriler SET {string.Join(", ", setKisimlari)} WHERE musteriID = @musteriID";
            komut.CommandText = sqlKomutu;
            komut.Parameters.Clear();
            komut.Parameters.AddWithValue("@musteriID", musteriID);
            komut.Connection = baglanti;

            try
            {
                baglanti.Open();
                if (!(komut.ExecuteNonQuery() == 1))
                    MessageBox.Show("Müşteri bilgileri güncellenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void MusteriSil(int musteriID)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "DELETE FROM musteriler WHERE musteriID=@musteriID";
            using (MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti))
            {
                komut.Parameters.Clear();
                komut.Parameters.AddWithValue("@musteriID", musteriID);
                try
                {
                    baglanti.Open();
                    if (!(komut.ExecuteNonQuery() == 1))
                        MessageBox.Show("Müşteri bilgileri silinemedi.");
                }
                catch (Exception hata)
                {
                    MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

}