using MySql.Data.MySqlClient;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

public class VTMenu
{
    public string baglantiKodu = "Server=localhost; Database=restoranmodulu; Uid=root; Pwd=Malukat3691.;";

    public DataTable Listele(int menuID = 0)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT * FROM Menuler";

            MySqlCommand komut = new MySqlCommand
            {
                Connection = baglanti
            };

            if (menuID != 0)
            {
                sqlKomutu += " WHERE menuID=@menuID";
                komut.Parameters.AddWithValue("@menuID", menuID);
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

    public void menuEkle(string adi, string aciklama, DateTime olusturmaTarihi)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "INSERT INTO Menuler(adi, aciklama, olusturmaTarihi) VALUES(@adi, @aciklama, @olusturmaTarihi)";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@adi", adi);
                komut.Parameters.AddWithValue("@aciklama", aciklama);
                komut.Parameters.AddWithValue("@olusturmaTarihi", olusturmaTarihi);

                bool sonuc = komut.ExecuteNonQuery() == 1;
                if (!sonuc)
                    MessageBox.Show("Menü eklenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void menuGuncelle(int menuID, string adi, string aciklama)
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
            if (setKisimlari.Count == 0)
                MessageBox.Show("Güncellenecek herhangi bir alan belirtilmedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            string sqlKomutu = $"UPDATE menuler SET {string.Join(", ", setKisimlari)} WHERE menuID = @menuID";
            komut.CommandText = sqlKomutu;
            komut.Parameters.AddWithValue("@menuID", menuID);
            komut.Connection = baglanti;
            try
            {
                baglanti.Open();
                bool sonuc = komut.ExecuteNonQuery() == 1;
                if (!sonuc)
                    MessageBox.Show("Menü bilgileri güncellenemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void menuSil(int menuID)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "DELETE FROM Menuler WHERE menuID=@menuID";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@menuID", menuID);
                bool sonuc = komut.ExecuteNonQuery() == 1;
                if (!sonuc)
                    MessageBox.Show("Menü bilgileri silinemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void menuyeUrunEkle(int menuID, int urunID)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {

            string sqlKomutu = "INSERT INTO MenuDetay(menuID, urunID)" +
                               "SELECT @menuID, @urunID FROM DUAL " +
                               "WHERE NOT EXISTS( SELECT 1 FROM MenuDetay WHERE menuID = @menuID AND urunID = @urunID );";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@menuID", menuID);
                komut.Parameters.AddWithValue("@urunID", urunID);

                bool sonuc = komut.ExecuteNonQuery() > 0;
                if (!sonuc)
                    MessageBox.Show("Bu ürün zaten menüye eklenmiş olabilir.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void menudenUrunSil(int menuID, int urunID)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "DELETE FROM MenuDetay WHERE menuID=@menuID AND urunID=@urunID";

            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@menuID", menuID);
                komut.Parameters.AddWithValue("@urunID", urunID);
                bool sonuc = komut.ExecuteNonQuery() == 1;
                if (!sonuc)
                    MessageBox.Show("Ürün, menüden silinemedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public DataTable urunleriListele(int menuID)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT U.* FROM MenuDetay MD " +
                               "INNER JOIN Urunler U ON MD.urunID = U.urunID " +
                               "WHERE MD.menuID = @menuID;";
            MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
            komut.Parameters.AddWithValue("@menuID", menuID);

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

    public void menuyuTemizle(int menuID)
    {
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "DELETE FROM menudetay WHERE menuID=@menuID";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@menuID", menuID);

                bool sonuc = komut.ExecuteNonQuery() > 0;
                if (!sonuc)
                    MessageBox.Show("Menü içeriği silinemedi.");
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public DataTable sonMenuyuAl()
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT menuID FROM menuler ORDER BY olusturmaTarihi DESC LIMIT 1";
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

    public DataTable urunFiltrele(string adi, int kategoriID = 0, int fiyati = 0, int miktar = 0)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            List<string> conditions = new List<string>();
            MySqlCommand komut = new MySqlCommand();

            if (!string.IsNullOrEmpty(adi))
            {
                conditions.Add("adi LIKE @adi");
                komut.Parameters.AddWithValue("@adi", "%" + adi + "%");
            }
            if (kategoriID != 0)
            {
                conditions.Add("kategoriID = @kategoriID");
                komut.Parameters.AddWithValue("@kategoriID", kategoriID);
            }
            if (fiyati != 0)
            {
                conditions.Add("fiyati = @fiyati");
                komut.Parameters.AddWithValue("@fiyati", fiyati);
            }
            if (miktar != 0)
            {
                conditions.Add("miktar = @miktar");
                komut.Parameters.AddWithValue("@miktar", miktar);
            }

            string sqlKomutu = "SELECT * FROM urunler";
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

    public Bitmap qrOlustur(string menuAdi)
    {
        Bitmap qrKod = null;
        if (!string.IsNullOrEmpty(menuAdi))
        {
            string link = "https://www.bilsoft.com";

            string content = $"{link}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            qrKod = qrCode.GetGraphic(20);
        }
        return qrKod;
    }
}