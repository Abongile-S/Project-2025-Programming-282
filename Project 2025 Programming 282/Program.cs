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
            //  Get user's desktop path
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Create permanent file paths
            string heroFile = Path.Combine(desktopPath, "superheroes.txt");
            string summaryFile = Path.Combine(desktopPath, "summary.txt");

            //  Create the files if they don't exist
            if (!File.Exists(heroFile))
                File.Create(heroFile).Close();

            if (!File.Exists(summaryFile))
                File.Create(summaryFile).Close();

            //store these path globally for form to use
            File.WriteAllText(Path.Combine(desktopPath, "file_paths.txt"),
                $"Hero File: {heroFile}\nSummary File: {summaryFile}");



            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(heroFile, summaryFile));aw
        }
    }
}
