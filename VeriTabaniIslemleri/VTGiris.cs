using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
public class VTGiris
{
    // MySQL Veri tabanı bağlantısı.
    public string baglantiKodu = "Server=localhost; Database=restoranmodulu; Uid=root; Pwd=Malukat3691.;";

    public DataTable KullaniciListele(string kullaniciAdi, string kullaniciParola, int rolID)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection baglanti = new MySqlConnection(baglantiKodu))
        {
            string sqlKomutu = "SELECT kullaniciID FROM kullanicilar WHERE kullaniciAdi = @kullaniciAdi AND parola = @parola AND rolID = @rolID";
            try
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand(sqlKomutu, baglanti);
                komut.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                komut.Parameters.AddWithValue("@parola", ParolaHashle(kullaniciParola));
                komut.Parameters.AddWithValue("@rolID", rolID);

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

    public static string ParolaHashle(string parola)
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

}