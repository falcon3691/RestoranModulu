using RestoranModulu.Ekranlar.Mutfak;
using System;
using System.Windows.Forms;

namespace RestoranModulu
{
    internal static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Mutfak(0));
        }
    }
}
