using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Project_2025_Programming_282
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Create permanent file paths on Desktop
           
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string heroFile = Path.Combine(desktopPath, "superheroes.txt");
            string summaryFile = Path.Combine(desktopPath, "summary.txt");

            if (!File.Exists(heroFile))
                File.Create(heroFile).Close();

            if (!File.Exists(summaryFile))
                File.Create(summaryFile).Close();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(heroFile, summaryFile));
        }
    }
}
