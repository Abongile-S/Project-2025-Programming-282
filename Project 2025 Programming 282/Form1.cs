using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Project_2025_Programming_282
{
    public partial class Form1 : Form
    {

        private string heroFilePath;
        private string summaryFilePath;
        public Form1(string heroFile, string summaryFile)
        {
            InitializeComponent();
            heroFilePath = heroFile;
            summaryFilePath = summaryFile;
        }

        public string[] CalculateLevels(int HeroExamScore)
        {
            string Rank, ThreatLevel;
            if (HeroExamScore >= 81)
            {
                return new string[] { "S-Rank", "Finals week" };
            }
            else if (HeroExamScore >= 61)
            {
                return new string[] { "A-Rank", "Midterm Madness" };
            }
            else if (HeroExamScore >= 41)
            {
                return new string[] { "B-Rank", "Group Project Gone Wrong" };
            }
            else
            {
                return new string[] { "C-Rank", "Pop Quiz" };
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int HeroID = int.Parse(textBox1.Text);
                string name = txtName.Text;
                int age = int.Parse(txtAge.Text);
                string superpower = txtSuperpower.Text;
                int HeroExamScore = int.Parse(textBox5.Text);
                string[] array = CalculateLevels(HeroExamScore);

                string rank = array[0];
                string threatLevel = array[1];

                CalculateLevels(HeroExamScore);


                string hero = $"Hero details: {HeroID}, {name} , {age}, {superpower}, {HeroExamScore}, {rank},  {threatLevel} \n";
                File.AppendAllText(heroFilePath, hero);
                MessageBox.Show("New Hero Added");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void Summary_Click(object sender, EventArgs e)
        {
            if (!File.Exists(heroFilePath))
            {
                MessageBox.Show("No superhero data found.");
                return;
            }

            var lines = File.ReadAllLines(heroFilePath);
            int total = lines.Length;
            double totalAge = 0, totalScore = 0;
            int sCount = 0, aCount = 0, bCount = 0, cCount = 0;

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                int age = int.Parse(parts[2]);
                int score = int.Parse(parts[4]);
                string rank = parts[5];

                totalAge += age;
                totalScore += score;

                switch (rank)
                {
                    case "S": sCount++; break;
                    case "A": aCount++; break;
                    case "B": bCount++; break;
                    case "C": cCount++; break;
                }
            }

            double avgAge = totalAge / total;
            double avgScore = totalScore / total;

            string summary = $"Total Heroes: {total}\n" +
                             $"Average Age: {avgAge:F2}\n" +
                             $"Average Score: {avgScore:F2}\n" +
                             $"S-Rank: {sCount}\nA-Rank: {aCount}\nB-Rank: {bCount}\nC-Rank: {cCount}";

            File.WriteAllText(summaryFilePath, summary);
            MessageBox.Show("Summary report saved to your Desktop!");
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (dgvHeroes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a superhero to delete.");
                return;
            }

            DataGridViewRow row = dgvHeroes.SelectedRows[0];
            string heroID = row.Cells["HeroID"].Value.ToString();

            DialogResult confirm = MessageBox.Show(
                $"Are you sure you want to delete Hero ID {heroID}?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                var lines = File.ReadAllLines(heroFilePath).ToList();
                lines.RemoveAll(line => line.StartsWith(heroID + ","));
                File.WriteAllLines(heroFilePath, lines);
                MessageBox.Show("Superhero deleted successfully!");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void View_Click(object sender, EventArgs e)
        {

            if (dgvHeroes.Columns.Count == 0)
            {
                dgvHeroes.Columns.Add("HeroID", "HeroID");
                dgvHeroes.Columns.Add("Name", "Name");
                dgvHeroes.Columns.Add("Age", "Age");
                dgvHeroes.Columns.Add("SuperPower", "SuperPower");
                dgvHeroes.Columns.Add("ExamScore", "ExamScore");
                dgvHeroes.Columns.Add("Rank", "Rank");
                dgvHeroes.Columns.Add("ThreatLevel", "ThreatLevel");

            }
            if (!File.Exists(heroFilePath))
            {
                MessageBox.Show("No superhero entries found");
                dgvHeroes.Rows.Clear();
                dgvHeroes.Columns.Clear();
            }
            else
            {

                using (StreamReader read = new StreamReader(heroFilePath))
                {
                    string row;
                    while ((row = read.ReadLine()) != null)
                    {
                        string[] section = row.Split(',');

                        dgvHeroes.Rows.Add(section);
                    }
                }

               // using (StreamReader read = new StreamReader(heroFilePath))
              //  {
                  //  string row;
                   // while ((row = read.ReadLine()) != null)
                  //  {
                   //     string[] section = row.Split(',');

                   //     dgvHeroes.Rows.Add(section);
                  //  }
               // }
            }
        }
    }
}
