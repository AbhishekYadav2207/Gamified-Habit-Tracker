using System;
using System.IO;
using System.Windows.Forms;
using HabitTracker.Core.Data;
using HabitTracker.WinForms.Forms;

namespace HabitTracker.WinForms
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string dbPath = ConfigManager.GetDatabasePath();

            // First-Time Setup or DB missing/corrupted logic
            if (string.IsNullOrEmpty(dbPath) || !File.Exists(dbPath))
            {
                using (var setupForm = new SetupForm())
                {
                    if (setupForm.ShowDialog() == DialogResult.OK)
                    {
                        dbPath = setupForm.SelectedDatabasePath;
                        ConfigManager.SetDatabasePath(dbPath);
                    }
                    else
                    {
                        MessageBox.Show("Database setup is required to run the application.", "Setup Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            // Real Login Logic
            using (var loginForm = new LoginForm(dbPath))
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new MainForm(dbPath, loginForm.LoggedInUser));
                }
            }
        }
    }
}
