using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

public class VTKullanicilar
{
    public string baglantiKodu = "Server=localhost; Database=restoranmodulu; Uid=root; Pwd=Malukat3691.;";

    public DataTable Listele()
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT kullanicilar.kullaniciID, kullanicilar.adiSoyadi, kullanicilar.telefon, kullanicilar.eMail, kullanicilar.kullaniciAdi, kullanicilar.parola, " +
                               "kullanicilar.durumu, kullanicilar.aciklama, roller.adi AS RolAdi FROM kullanicilar " +
                               "JOIN roller ON kullanicilar.rolID=roller.rolID";
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

    public DataTable Listele2()
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT kullaniciID, adiSoyadi FROM kullanicilar";
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

    public void KullaniciEkle(string adiSoyadi, string kullaniciAdi, string parola, int rolID, string durumu, string aciklama, string telefon, string eMail)
    {
        bool sonuc = false;
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string parolaHashli = ParolaHashle(parola);
            string sqlKomutu = "INSERT INTO kullanicilar(adiSoyadi, telefon, eMail, kullaniciAdi, parola, rolID, durumu, aciklama)" +
                                                   " VALUES(@adiSoyadi, @telefon, @eMail, @kullaniciAdi, @parola, @rolID, @durumu, @aciklama)";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@adiSoyadi", adiSoyadi);
                komut.Parameters.AddWithValue("@telefon", telefon);
                komut.Parameters.AddWithValue("@eMail", eMail);
                komut.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                komut.Parameters.AddWithValue("@parola", parolaHashli);
                komut.Parameters.AddWithValue("@rolID", rolID);
                komut.Parameters.AddWithValue("@durumu", durumu);
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

    public void KullaniciGuncelle(int kullaniciID, string adiSoyadi, string kullaniciAdi, string parola, string durumu,
                              string aciklama, string telefon, string eMail, int rolID = 0)
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

            if (!string.IsNullOrEmpty(telefon))
            {
                setKisimlari.Add("telefon = @telefon");
                komut.Parameters.AddWithValue("@telefon", telefon);
            }

            if (!string.IsNullOrEmpty(eMail))
            {
                setKisimlari.Add("eMail = @eMail");
                komut.Parameters.AddWithValue("@eMail", eMail);
            }

            if (!string.IsNullOrEmpty(kullaniciAdi))
            {
                setKisimlari.Add("kullaniciAdi = @kullaniciAdi");
                komut.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
            }

            if (!string.IsNullOrEmpty(parola))
            {
                setKisimlari.Add("parola = @parola");
                komut.Parameters.AddWithValue("@parola", ParolaHashle(parola));
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

            if (rolID != 0)
            {
                setKisimlari.Add("rolID = @rolID");
                komut.Parameters.AddWithValue("@rolID", rolID);
            }

            if (setKisimlari.Count == 0)
                MessageBox.Show("Güncellenecek herhangi bir alan belirtilmedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            string sqlKomutu = $"UPDATE kullanicilar SET {string.Join(", ", setKisimlari)} WHERE kullaniciID = @kullaniciID";
            komut.CommandText = sqlKomutu;
            komut.Parameters.AddWithValue("@kullaniciID", kullaniciID);
            komut.Connection = baglanti;

            try
            {
                baglanti.Open();
                bool sonuc = (komut.ExecuteNonQuery() == 1) ? true : false;
                if (!sonuc)
                    MessageBox.Show("Kullanıcı bilgileri güncellenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void KullaniciSil(int kullaniciID)
    {
        bool sonuc = false;
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "DELETE FROM kullanicilar WHERE kullaniciID=@kullaniciID";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@kullaniciID", kullaniciID);

                sonuc = (komut.ExecuteNonQuery() == 1) ? true : false;
                if (!sonuc)
                    MessageBox.Show("Kullanıcı bilgileri silinemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public DataTable kullaniciFiltrele(string adiSoyadi, int rolID, string durumu, string telefon, string eMail)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            List<string> conditions = new List<string>();
            MySqlCommand komut = new MySqlCommand();

            if (!string.IsNullOrEmpty(adiSoyadi))
            {
                conditions.Add("adiSoyadi LIKE @adiSoyadi");
                komut.Parameters.AddWithValue("@adiSoyadi", "%" + adiSoyadi + "%");
            }
            if (!string.IsNullOrEmpty(rolID.ToString()))
            {
                conditions.Add("rolID = @rolID");
                komut.Parameters.AddWithValue("@rolID", rolID);
            }
            if (!string.IsNullOrEmpty(durumu))
            {
                conditions.Add("durumu LIKE @durumu");
                komut.Parameters.AddWithValue("@durumu", "%" + durumu + "%");
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

            string sqlKomutu = "SELECT * FROM kullanicilar ";
            if (conditions.Count > 0)
                sqlKomutu += "WHERE " + string.Join(" OR ", conditions);

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

    public string ParolaHashle(string parola)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(parola);
            byte[] hash = sha256.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }

    public DataTable rolListele()
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT rolID, adi FROM roller";
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