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
        // Stores file paths for heroes and summary text files
        private string heroFilePath;
        private string summaryFilePath;

        // Sets up file paths when the form starts
        public Form1(string heroFile, string summaryFile)
        {
            InitializeComponent();
            heroFilePath = heroFile;
            summaryFilePath = summaryFile;
        }

        // Loads hero data from the text file into the DataGridView
        private void LoadHeroes()
        {
            dgvHeroes.Rows.Clear();

            if (dgvHeroes.Columns.Count == 0)
            {
                dgvHeroes.Columns.Add("HeroID", "Hero ID");
                dgvHeroes.Columns.Add("Name", "Name");
                dgvHeroes.Columns.Add("Age", "Age");
                dgvHeroes.Columns.Add("SuperPower", "Super Power");
                dgvHeroes.Columns.Add("ExamScore", "Exam Score");
                dgvHeroes.Columns.Add("Rank", "Rank");
                dgvHeroes.Columns.Add("ThreatLevel", "Threat Level");
            }

            if (!File.Exists(heroFilePath)) return;

            foreach (string line in File.ReadAllLines(heroFilePath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split(',');
                if (parts.Length >= 7)
                    dgvHeroes.Rows.Add(parts);
            }
        }
        // Calculates the hero's rank and threat level based on exam score
        public string[] CalculateLevels(int HeroExamScore)
        {
            if (HeroExamScore >= 81)
                return new[] { "S-Rank", "Finals Week" };
            else if (HeroExamScore >= 61)
                return new[] { "A-Rank", "Midterm Madness" };
            else if (HeroExamScore >= 41)
                return new[] { "B-Rank", "Group Project Gone Wrong" };
            else
                return new[] { "C-Rank", "Pop Quiz" };

        }
        // Adds a new hero to the text file
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

                // Save hero info in text file
                string hero = $"Hero details: {HeroID}, {name} , {age}, {superpower}, {HeroExamScore}, {rank},  {threatLevel} \n";
                File.AppendAllText(heroFilePath, hero);
                MessageBox.Show("New Hero Added");

                LoadHeroes(); // refresh display
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
        // Updates an existing hero's information
        private void button3_Click(object sender, EventArgs e)
        {
            if (dgvHeroes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a hero to update.");
                return;
            }

            DataGridViewRow row = dgvHeroes.SelectedRows[0];
            string selectedID = row.Cells["HeroID"].Value.ToString().Trim();

            try
            {
                var lines = File.ReadAllLines(heroFilePath).ToList();

                for (int i = 0; i < lines.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i])) continue;

                    var parts = lines[i].Split(',');
                    if (parts.Length < 7) continue;

                    if (parts[0].Trim() == selectedID)
                    {
                        // Get updated values from textboxes
                        int.TryParse(txtAge.Text.Trim(), out int age);
                        int.TryParse(textBox5.Text.Trim(), out int score);

                        string[] lvl = CalculateLevels(score);
                        string rank = lvl[0];
                        string threat = lvl[1];

                        // Replace the old line with the new one
                        lines[i] = $"{selectedID},{txtName.Text.Trim()},{age},{txtSuperpower.Text.Trim()},{score},{rank},{threat}";
                        break;
                    }
                }
                // Save all heroes back to file
                File.WriteAllLines(heroFilePath, lines);
                MessageBox.Show("Hero updated successfully!");
                LoadHeroes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating hero: " + ex.Message);
            }
        }
        // Generates and saves a summary report of all heroes
        private void Summary_Click(object sender, EventArgs e)
        {
            if (!File.Exists(heroFilePath))
            {
                MessageBox.Show("No hero file found.");
                return;
            }

            var lines = File.ReadAllLines(heroFilePath)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToArray();

            if (lines.Length == 0)
            {
                MessageBox.Show("No heroes to summarize.");
                return;
            }

            int total = 0;
            double totalAge = 0, totalScore = 0;
            int sCount = 0, aCount = 0, bCount = 0, cCount = 0;

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length < 7) continue;

                total++;
                double.TryParse(parts[2], out double age);
                double.TryParse(parts[4], out double score);
                string rank = parts[5].Trim();

                totalAge += age;
                totalScore += score;
                // Count how many heroes per rank
                switch (rank)
                {
                    case "S-Rank": sCount++; break;
                    case "A-Rank": aCount++; break;
                    case "B-Rank": bCount++; break;
                    case "C-Rank": cCount++; break;
                }
            }

            if (total == 0)
            {
                MessageBox.Show("No valid data found.");
                return;
            }

            double avgAge = totalAge / total;
            double avgScore = totalScore / total;

            // Create summary text
            string summary = $"Total Heroes: {total}\n" +
                             $"Average Age: {avgAge:F2}\n" +
                             $"Average Score: {avgScore:F2}\n" +
                             $"S-Rank: {sCount}\nA-Rank: {aCount}\nB-Rank: {bCount}\nC-Rank: {cCount}";

            File.WriteAllText(summaryFilePath, summary);
            MessageBox.Show("Summary saved to Desktop!");
        }
        // Deletes the selected hero from the text file
        private void Delete_Click(object sender, EventArgs e)
        {
            if (dgvHeroes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a hero to delete.");
                return;
            }

            DataGridViewRow sel = dgvHeroes.SelectedRows[0];
            string selectedID = sel.Cells["HeroID"].Value.ToString().Trim();

            DialogResult confirm = MessageBox.Show(
                $"Delete Hero ID {selectedID}?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                var lines = File.ReadAllLines(heroFilePath).ToList();
                int index = -1;

                // Find the hero's line in file
                for (int i = 0; i < lines.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i])) continue;
                    var parts = lines[i].Split(',');
                    if (parts[0].Trim() == selectedID)
                    {
                        index = i;
                        break;
                    }
                }

                // Remove that hero from the file
                if (index != -1)
                    lines.RemoveAt(index);

                File.WriteAllLines(heroFilePath, lines);
                MessageBox.Show("Hero deleted successfully!");
                LoadHeroes();
            }
            catch
            {
                MessageBox.Show("Error deleting hero.");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        // Displays all heroes in the DataGridView
        private void View_Click(object sender, EventArgs e)
        {
            LoadHeroes();
        }

        // Clears all textboxes on the form
        private void button7_Click(object sender, EventArgs e)
        {
            // Clears all text boxes
            textBox1.Clear();       
            txtName.Clear();        
            txtAge.Clear();        
            txtSuperpower.Clear();  
            textBox5.Clear();       

            // Optional: Deselect grid
            dgvHeroes.ClearSelection();
        }


        }
    }

