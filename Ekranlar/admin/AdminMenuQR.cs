using System;
using System.Drawing;
using System.Windows.Forms;

namespace RestoranModulu.Ekranlar.admin
{
    public partial class AdminMenuQR : Form
    {
        Bitmap qrKod;
        public AdminMenuQR(Bitmap qrKod)
        {
            InitializeComponent();
            this.qrKod = qrKod;
            pictureBox1.Image = qrKod;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.Location = new Point(20, 20);
            button2.Location = new Point(20, pictureBox1.Bottom + 15);
            button1.Location = new Point(button2.Right + 10, button2.Top);

            int padding = 40;
            int formWidth = pictureBox1.Width + padding;
            int formHeight = button2.Bottom + 60;

            formWidth = Math.Max(formWidth, 300);
            formHeight = Math.Max(formHeight, 250);

            this.ClientSize = new Size(formWidth, formHeight);
        }

        // Geri Dön butonu
        private void button1_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        // Kaydet butonu
        private void button2_Click(object sender, System.EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PNG Dosyası|*.png";
                saveDialog.FileName = "QRKod.png";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    qrKod.Save(saveDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    MessageBox.Show("QR kod başarıyla kaydedildi.");
                }
            }
        }

    }
}
