using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

public class VTMasa
{
    public string baglantiKodu = "Server=localhost; Database=restoranmodulu; Uid=root; Pwd=Malukat3691.;";

    public DataTable Listele(bool boslar = false, int katID = 0)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT * FROM Masalar";
            MySqlCommand komut = new MySqlCommand
            {
                Connection = baglanti
            };

            if (katID != 0 || boslar == true)
            {
                sqlKomutu += " WHERE katID = @katID";
                komut.Parameters.AddWithValue("@katID", katID);
            }

            komut.CommandText = sqlKomutu;
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

    public DataTable Listele2(int masaID)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT * FROM Masalar WHERE masaID=@masaID";
            MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
            komut.Parameters.AddWithValue("@masaID", masaID);
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

    public void masaEkle(string adi, int sandalyeSayisi, string masaDurumu, int katID, string aciklama)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "INSERT INTO Masalar(adi, sandalyeSayisi, masaDurumu, aciklama, katID)" +
                                          " VALUES(@adi, @sandalyeSayisi, @masaDurumu, @aciklama, @katID)";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@adi", adi);
                komut.Parameters.AddWithValue("@sandalyeSayisi", sandalyeSayisi);
                komut.Parameters.AddWithValue("@masaDurumu", masaDurumu);
                komut.Parameters.AddWithValue("@aciklama", aciklama);
                komut.Parameters.AddWithValue("@katID", katID);

                bool sonuc = komut.ExecuteNonQuery() == 1;
                if (!sonuc)
                    MessageBox.Show("Masa eklenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void masaGuncelle(int masaID, string adi, string masaDurumu, string aciklama, int sandalyeSayisi = 0, int katID = 0, string grupID = null)
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

            if (!string.IsNullOrEmpty(masaDurumu))
            {
                setKisimlari.Add("masaDurumu = @masaDurumu");
                komut.Parameters.AddWithValue("@masaDurumu", masaDurumu);
            }

            if (!string.IsNullOrEmpty(aciklama))
            {
                setKisimlari.Add("aciklama = @aciklama");
                komut.Parameters.AddWithValue("@aciklama", aciklama);
            }

            if (sandalyeSayisi != 0)
            {
                setKisimlari.Add("sandalyeSayisi = @sandalyeSayisi");
                komut.Parameters.AddWithValue("@sandalyeSayisi", sandalyeSayisi);
            }

            if (katID != 0)
            {
                setKisimlari.Add("katID = @katID");
                komut.Parameters.AddWithValue("@katID", katID);
            }

            if (!string.IsNullOrEmpty(grupID))
            {
                setKisimlari.Add("grupID = @grupID");
                komut.Parameters.AddWithValue("@grupID", Convert.ToInt32(grupID));
            }
            else
                setKisimlari.Add("grupID = NULL");

            if (setKisimlari.Count == 0)
                MessageBox.Show("Güncellenecek herhangi bir alan belirtilmedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            string sqlKomutu = $"UPDATE masalar SET {string.Join(", ", setKisimlari)} WHERE masaID = @masaID";
            komut.CommandText = sqlKomutu;
            komut.Parameters.AddWithValue("@masaID", masaID);
            komut.Connection = baglanti;

            try
            {
                baglanti.Open();
                bool sonuc = komut.ExecuteNonQuery() == 1;
                if (!sonuc)
                    MessageBox.Show("Masa bilgileri güncellenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void masaSil(int masaID)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "DELETE FROM Masalar WHERE masaID=@masaID";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@masaID", masaID);

                bool sonuc = komut.ExecuteNonQuery() == 1;
                if (!sonuc)
                    MessageBox.Show("Masa bilgileri silinemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public DataTable masaFiltrele(int masaID, string adi, int sandalyeSayisi, string masaDurumu, int katID = 0)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            List<string> conditions = new List<string>();
            MySqlCommand komut = new MySqlCommand();

            if (masaID != 0)
            {
                conditions.Add("masaID = @masaID");
                komut.Parameters.AddWithValue("@masaID", masaID);
            }

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
            if (katID != 0)
            {
                conditions.Add("katID = @katID");
                komut.Parameters.AddWithValue("@katID", katID);
            }

            string sqlKomutu = "SELECT * FROM masalar";
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

    public void masalariTemizle(int katID)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "UPDATE Masalar SET katID=0 WHERE katID=@katID";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@katID", katID);

                if (komut.ExecuteNonQuery() <= 0)
                    MessageBox.Show("Masalar temizlenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public DataTable masaGrupListele(int masaID)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT masaID, adi FROM masalar WHERE (grupID IS NULL) AND masaID!=@masaID";

            try
            {
                baglanti.Open();
                using (MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti))
                {
                    komut.Parameters.AddWithValue("@masaID", masaID);

                    MySqlDataAdapter da = new MySqlDataAdapter(komut);
                    da.Fill(dt);
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        return dt;
    }

    public DataTable masaGrupListele2(int masaID)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT masaID, adi " +
                               "FROM masalar " +
                               "WHERE grupID = (SELECT grupID FROM masalar WHERE masaID = @masaID) AND masaID != @masaID";

            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.Clear();
                komut.Parameters.AddWithValue("@masaID", masaID);

                MySqlDataAdapter da = new MySqlDataAdapter(komut);
                da.Fill(dt);
                komut.Dispose();
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        return dt;
    }

    public void masalarıGrupla(DataGridView masalarID)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT IFNULL(MAX(grupID), 0) + 1 FROM masagrup";
            try
            {
                baglanti.Open();
                using (MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti))
                {
                    komut.Parameters.Clear();
                    object sonucObj = komut.ExecuteScalar();

                    if (sonucObj != null && int.TryParse(sonucObj.ToString(), out int grupIDDegeri))
                    {
                        int masaID = 0;
                        if (grupIDDegeri > 1)
                        {
                            sqlKomutu = "DELETE FROM masagrup WHERE masaID=@masaID";
                            komut.CommandText = sqlKomutu;

                            foreach (DataGridViewRow row in masalarID.Rows)
                            {
                                masaID = Convert.ToInt32(row.Cells["masaID"].Value);
                                komut.Parameters.Clear();
                                komut.Parameters.AddWithValue("@masaID", masaID);

                                komut.ExecuteNonQuery();
                            }
                        }

                        sqlKomutu = "INSERT INTO masagrup(grupID, masaID) VALUES(@grupIDDegeri, @masaID)";
                        komut.CommandText = sqlKomutu;
                        List<int> basariliMasalar = new List<int>();
                        List<int> basarisizMasalar = new List<int>();

                        foreach (DataGridViewRow row in masalarID.Rows)
                        {
                            masaID = Convert.ToInt32(row.Cells["masaID"].Value);
                            komut.Parameters.Clear();
                            komut.Parameters.AddWithValue("@grupIDDegeri", grupIDDegeri);
                            komut.Parameters.AddWithValue("@masaID", masaID);
                            bool sonuc = komut.ExecuteNonQuery() == 1;
                            if (sonuc)
                            {
                                masaGuncelle(masaID, null, null, null, 0, 0, grupIDDegeri.ToString());
                                basariliMasalar.Add(masaID);
                            }
                            else
                            {
                                MessageBox.Show($"HATA: {row.Cells["masaID"].Value} ID değerine sahip masa, {grupIDDegeri} ID değerine sahip gruba eklenemedi.");
                                basarisizMasalar.Add(masaID);
                            }
                        }
                        if (basariliMasalar.Any())
                        {
                            MessageBox.Show($"Masalar gruplandı: {string.Join(", ", basariliMasalar)}", "BAŞARILI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        if (basarisizMasalar.Any())
                        {
                            MessageBox.Show($"Bazı masalar gruplanamadı: {string.Join(", ", basarisizMasalar)}", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                        MessageBox.Show("Yeni grup oluşturulamadı.");
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void gruptanAyril(int masaID)
    {
        if (MessageBox.Show("Seçili masa gruptan çıkarılacak. Emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            return;

        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "DELETE FROM masagrup WHERE masaID=@masaID";
            try
            {
                baglanti.Open();
                using (MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti))
                {
                    komut.Parameters.Clear();
                    komut.Parameters.AddWithValue("@masaID", masaID);
                    int grupIDDegeri = grupDegeriniBul(masaID);
                    bool sonuc = komut.ExecuteNonQuery() == 1;
                    if (sonuc)
                        masaGuncelle(masaID, null, null, null, 0, 0, null);
                    else
                        MessageBox.Show("Masa gruptan çıkartılamadı.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (grupIDDegeri != 0)
                    {
                        sqlKomutu = "SELECT COUNT(PKsutunu) FROM masagrup WHERE grupID=@grupID";
                        komut.CommandText = sqlKomutu;
                        komut.Parameters.Clear();
                        komut.Parameters.AddWithValue("@grupID", grupIDDegeri);
                        int deger = Convert.ToInt32(komut.ExecuteScalar());
                        if (deger == 1)
                        {
                            grubuSil(grupIDDegeri);
                        }
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public int grupDegeriniBul(int masaID)
    {
        int grupIDDegeri = 0;
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT grupID FROM masalar WHERE masaID=@masaID";
            using (MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti))
            {
                try
                {
                    baglanti.Open();
                    komut.Parameters.Clear();
                    komut.Parameters.AddWithValue("@masaID", masaID);
                    bool sonuc = int.TryParse(komut.ExecuteScalar().ToString(), out grupIDDegeri);
                    if (!sonuc)
                        grupIDDegeri = 0;
                }
                catch (Exception hata)
                {
                    MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        return grupIDDegeri;
    }

    public void grubuSil(int grupID)
    {

        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "DELETE FROM masagrup WHERE grupID=@grupID";
            using (MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti))
            {
                try
                {
                    baglanti.Open();
                    komut.Parameters.Clear();
                    komut.Parameters.AddWithValue("@grupID", grupID);
                    bool sonuc = komut.ExecuteNonQuery() == 1;
                    if (sonuc)
                    {
                        sqlKomutu = "SELECT masaID FROM masalar WHERE grupID=@grupID";
                        komut.CommandText = sqlKomutu;
                        komut.Parameters.Clear();
                        komut.Parameters.AddWithValue("@grupID", grupID);

                        sonuc = int.TryParse(komut.ExecuteScalar().ToString(), out int masaID);
                        if (sonuc)
                            masaGuncelle(masaID, null, null, null, 0, 0, null);
                    }
                    else
                        MessageBox.Show("Tek masası kalan grup silinemedi.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception hata)
                {
                    MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    public void grubuDagit(DataGridView masalarID)
    {
        if (MessageBox.Show("Seçili tüm masalar gruptan çıkarılacak. Emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            return;

        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            try
            {
                baglanti.Open();
                string sqlKomutu = "DELETE FROM masagrup WHERE masaID=@masaID";
                using (MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti))
                {
                    int masaID = 0;
                    foreach (DataGridViewRow row in masalarID.Rows)
                    {
                        masaID = Convert.ToInt32(row.Cells["masaID"].Value);

                        komut.Parameters.Clear();
                        komut.Parameters.AddWithValue("@masaID", masaID);

                        bool sonuc = komut.ExecuteNonQuery() == 1;
                        if (sonuc)
                            masaGuncelle(masaID, null, null, null, 0, 0, null);
                        else
                            MessageBox.Show("Masa gruptan çıkartılamadı.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}