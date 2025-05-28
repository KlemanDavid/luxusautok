using System;
using System.Windows.Forms;

namespace CarMaintenanceManager
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Hagyományos WinForms indítási kód Init nélkül
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}