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
                    if (parts[0].Trim() == selectedID)
                    {
                        int age = int.Parse(txtAge.Text);
                        int score = int.Parse(txtExamScore.Text);
                        string[] lvl = CalculateLevels(score);
                        string rank = lvl[0];
                        string threat = lvl[1];

                        lines[i] = $"{selectedID},{txtName.Text},{age},{txtSuperpower.Text},{score},{rank},{threat}";
                        break;
                    }
                }

                File.WriteAllLines(heroFilePath, lines);
                MessageBox.Show("Hero updated successfully!");
                LoadHeroes();
            }
            catch
            {
                MessageBox.Show("Error updating hero.");
            }
        }

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

            string summary = $"Total Heroes: {total}\n" +
                             $"Average Age: {avgAge:F2}\n" +
                             $"Average Score: {avgScore:F2}\n" +
                             $"S-Rank: {sCount}\nA-Rank: {aCount}\nB-Rank: {bCount}\nC-Rank: {cCount}";

            File.WriteAllText(summaryFilePath, summary);
            MessageBox.Show("Summary saved to Desktop!");
        }

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

        private void View_Click(object sender, EventArgs e)
        {
            LoadHeroes();
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
        private void dgvHeroes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtHeroID.Text = dgvHeroes.Rows[e.RowIndex].Cells["HeroID"].Value.ToString();
                txtName.Text = dgvHeroes.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                txtAge.Text = dgvHeroes.Rows[e.RowIndex].Cells["Age"].Value.ToString();
                txtSuperpower.Text = dgvHeroes.Rows[e.RowIndex].Cells["SuperPower"].Value.ToString();
                txtExamScore.Text = dgvHeroes.Rows[e.RowIndex].Cells["ExamScore"].Value.ToString();
            }
        }
    }
}
