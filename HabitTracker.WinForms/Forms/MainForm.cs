using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HabitTracker.Core.Data;
using HabitTracker.Core.Logic;
using HabitTracker.Core.Models;

namespace HabitTracker.WinForms.Forms
{
    public class MainForm : Form
    {
        private readonly string _dbPath;
        private User _user;
        private readonly HabitRepository _habitRepo;
        private readonly HabitLogRepository _logRepo;
        private readonly RewardRepository _rewardRepo;
        private readonly UserRepository _userRepo;
        private readonly GameEngine _engine;

        private DataGridView dgvHabits;
        private ProgressBar pbXP;
        private Label lblLevelXP;
        private Label lblFreezes;
        private Timer reminderTimer;

        public MainForm(string dbPath, User loggedInUser)
        {
            _dbPath = dbPath;
            _user = loggedInUser;
            _habitRepo = new HabitRepository(dbPath);
            _logRepo = new HabitLogRepository(dbPath);
            _rewardRepo = new RewardRepository(dbPath);
            _userRepo = new UserRepository(dbPath);
            _engine = new GameEngine(dbPath);

            InitializeComponent();
            LoadData();
            ShowDailySummary();

            reminderTimer = new Timer { Interval = 60000 }; // check every minute
            reminderTimer.Tick += ReminderTimer_Tick;
            reminderTimer.Start();
        }

        private void InitializeComponent()
        {
            this.Text = $"Gamified Habit Tracker - Welcome {_user.Username}!";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            var panelTop = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.WhiteSmoke };
            
            lblLevelXP = new Label { Top = 20, Left = 20, Width = 300, Font = new Font("Arial", 12, FontStyle.Bold) };
            pbXP = new ProgressBar { Top = 50, Left = 20, Width = 400, Height = 20, Maximum=100 };
            lblFreezes = new Label { Top = 20, Left = 450, Width = 200, Font = new Font("Arial", 10, FontStyle.Italic) };

            var btnBackup = new Button { Text = "Backup DB", Top = 20, Left = 650, Width = 100 };
            btnBackup.Click += BtnBackup_Click;

            panelTop.Controls.Add(lblLevelXP);
            panelTop.Controls.Add(pbXP);
            panelTop.Controls.Add(lblFreezes);
            panelTop.Controls.Add(btnBackup);

            var panelButtons = new Panel { Dock = DockStyle.Right, Width = 150, Padding = new Padding(10) };
            var btnAdd = new Button { Text = "Add Habit", Dock = DockStyle.Top, Height = 40, Margin = new Padding(0,0,0,10) };
            btnAdd.Click += BtnAdd_Click;
            
            var btnEdit = new Button { Text = "Edit Habit", Dock = DockStyle.Top, Height = 40, Margin = new Padding(0,0,0,10) };
            btnEdit.Click += BtnEdit_Click;

            var btnDelete = new Button { Text = "Delete Habit", Dock = DockStyle.Top, Height = 40, Margin = new Padding(0,0,0,10) };
            btnDelete.Click += BtnDelete_Click;

            var btnComplete = new Button { Text = "Complete Today!", Dock = DockStyle.Top, Height = 60, BackColor = Color.LightGreen, Font = new Font("Arial", 10, FontStyle.Bold) };
            btnComplete.Click += BtnComplete_Click;

            panelButtons.Controls.Add(btnComplete);
            panelButtons.Controls.Add(new Label{Height=20, Dock=DockStyle.Top});
            panelButtons.Controls.Add(btnDelete);
            panelButtons.Controls.Add(btnEdit);
            panelButtons.Controls.Add(btnAdd);

            dgvHabits = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            
            this.Controls.Add(dgvHabits);
            this.Controls.Add(panelButtons);
            this.Controls.Add(panelTop);
        }

        private void LoadData()
        {
            // Refresh user data from db just in case
            _user = _userRepo.Authenticate(_user.Username, _user.PasswordHash);
            
            // Set ProgressBar max based on next level required XP
            int requiredXp = (int)Math.Pow(_user.Level, 2) * 50; 
            pbXP.Maximum = requiredXp;
            pbXP.Value = Math.Min(_user.XP, requiredXp);
            
            lblLevelXP.Text = $"Level: {_user.Level} | XP: {_user.XP} / {requiredXp}";
            lblFreezes.Text = $"Streak Freezes: {_user.AvailableFreezes}";

            var habits = _habitRepo.GetHabitsByUser(_user.UserId);
            dgvHabits.DataSource = null; // reset binding
            dgvHabits.DataSource = habits.Select(h => new { 
                h.HabitId, 
                h.HabitName, 
                h.Difficulty, 
                h.CurrentStreak, 
                Reminder = h.ReminderTime.ToString(@"hh\:mm"),
                CompletedToday = _logRepo.IsHabitCompletedToday(h.HabitId) ? "Yes" : "No"
            }).ToList();

            // Color rows visually if completed
            foreach (DataGridViewRow row in dgvHabits.Rows)
            {
                if (row.Cells["CompletedToday"].Value.ToString() == "Yes")
                {
                    row.DefaultCellStyle.BackColor = Color.LightCyan;
                }
            }
        }

        private void ShowDailySummary()
        {
            int totalHabits = _habitRepo.GetHabitsByUser(_user.UserId).Count;
            if (totalHabits > 0)
            {
                int completed = _logRepo.GetCompletedHabitsCountToday(_user.UserId);
                MessageBox.Show($"Daily Summary:\nYou have completed {completed}/{totalHabits} habits today!", "Daily Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private Habit GetSelectedHabit()
        {
            if (dgvHabits.SelectedRows.Count > 0)
            {
                int id = (int)dgvHabits.SelectedRows[0].Cells["HabitId"].Value;
                return _habitRepo.GetHabitsByUser(_user.UserId).FirstOrDefault(h => h.HabitId == id);
            }
            return null;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new AddEditHabitForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    form.HabitResult.UserId = _user.UserId;
                    _habitRepo.AddHabit(form.HabitResult);
                    LoadData();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            var habit = GetSelectedHabit();
            if (habit != null)
            {
                using (var form = new AddEditHabitForm(habit))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        _habitRepo.UpdateHabit(form.HabitResult);
                        LoadData();
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var habit = GetSelectedHabit();
            if (habit != null && MessageBox.Show($"Delete '{habit.HabitName}'?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _habitRepo.DeleteHabit(habit.HabitId, _user.UserId);
                LoadData();
            }
        }

        private void BtnComplete_Click(object sender, EventArgs e)
        {
            var habit = GetSelectedHabit();
            if (habit != null)
            {
                int oldLevel = _user.Level;
                int oldXP = _user.XP;

                _engine.CompleteHabit(habit, _user);
                
                // Visual Feedback
                if (_user.XP > oldXP)
                {
                    MessageBox.Show($"+{_user.XP - oldXP} XP gained!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                if (_user.Level > oldLevel)
                {
                    MessageBox.Show($"Level Up! You are now level {_user.Level}.", "Level Up", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _rewardRepo.UnlockEligibleRewards(_user);
                }

                LoadData();
            }
        }

        private void ReminderTimer_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now.TimeOfDay;
            var habits = _habitRepo.GetHabitsByUser(_user.UserId);
            foreach (var h in habits)
            {
                // If it's the exact minute (or within a 1-min window) and NOT completed today
                if (Math.Abs((h.ReminderTime - now).TotalMinutes) < 1.0)
                {
                    if (!_logRepo.IsHabitCompletedToday(h.HabitId))
                    {
                        // Stop timer briefly to prevent multiple popups
                        reminderTimer.Stop();
                        MessageBox.Show($"Reminder: Time to complete your habit '{h.HabitName}'!", "Habit Reminder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        reminderTimer.Start();
                    }
                }
            }
        }

        private void BtnBackup_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog() { Filter = "SQLite DB|*.db", FileName = "habit_tracker_backup.db" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.Copy(_dbPath, sfd.FileName, true);
                        MessageBox.Show("Backup created successfully!", "Success");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error creating backup: " + ex.Message);
                    }
                }
            }
        }
    }
}
