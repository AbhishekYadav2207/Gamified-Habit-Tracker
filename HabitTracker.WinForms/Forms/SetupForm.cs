using System;
using System.IO;
using System.Windows.Forms;
using HabitTracker.Core.Data;

namespace HabitTracker.WinForms.Forms
{
    public class SetupForm : Form
    {
        public string SelectedDatabasePath { get; private set; }

        private TextBox txtPath;
        private Button btnBrowse;
        private Button btnSave;

        public SetupForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "First Time Setup - Gamified Habit Tracker";
            this.Size = new System.Drawing.Size(400, 200);
            this.StartPosition = FormStartPosition.CenterScreen;

            var label = new Label { Text = "Select folder to create local Database:", Top = 20, Left = 20, Width = 300 };
            
            txtPath = new TextBox { Top = 50, Left = 20, Width = 250, ReadOnly = true };
            
            btnBrowse = new Button { Text = "Browse", Top = 48, Left = 280, Width = 80 };
            btnBrowse.Click += BtnBrowse_Click;

            btnSave = new Button { Text = "Save & Initialize", Top = 100, Left = 20, Width = 150, Enabled = false };
            btnSave.Click += BtnSave_Click;

            this.Controls.Add(label);
            this.Controls.Add(txtPath);
            this.Controls.Add(btnBrowse);
            this.Controls.Add(btnSave);
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select a folder to save your habit_tracker.db database file.";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = dialog.SelectedPath;
                    SelectedDatabasePath = Path.Combine(txtPath.Text, "habit_tracker.db");
                    btnSave.Enabled = true;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SelectedDatabasePath))
            {
                try
                {
                    DatabaseSetup.InitializeDatabase(SelectedDatabasePath);
                    MessageBox.Show("Database initialized successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
