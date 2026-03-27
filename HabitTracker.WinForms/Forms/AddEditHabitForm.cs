using System;
using System.Windows.Forms;
using HabitTracker.Core.Models;

namespace HabitTracker.WinForms.Forms
{
    public class AddEditHabitForm : Form
    {
        public Habit HabitResult { get; private set; }

        private TextBox txtName;
        private ComboBox cmbDifficulty;
        private DateTimePicker dtpReminder;
        private Button btnSave;

        public AddEditHabitForm(Habit habit = null)
        {
            InitializeComponent();
            if (habit != null)
            {
                HabitResult = habit;
                txtName.Text = habit.HabitName;
                cmbDifficulty.SelectedItem = habit.Difficulty;
                dtpReminder.Value = DateTime.Today.Add(habit.ReminderTime);
            }
            else
            {
                HabitResult = new Habit();
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Add / Edit Habit";
            this.Size = new System.Drawing.Size(350, 300);
            this.StartPosition = FormStartPosition.CenterParent;

            this.Controls.Add(new Label { Text = "Habit Name:", Top = 20, Left = 20 });
            txtName = new TextBox { Top = 40, Left = 20, Width = 280 };

            this.Controls.Add(new Label { Text = "Difficulty:", Top = 80, Left = 20 });
            cmbDifficulty = new ComboBox { Top = 100, Left = 20, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbDifficulty.Items.AddRange(new object[] { "Easy", "Medium", "Hard" });
            cmbDifficulty.SelectedIndex = 0;

            this.Controls.Add(new Label { Text = "Reminder Time:", Top = 140, Left = 20 });
            dtpReminder = new DateTimePicker { Top = 160, Left = 20, Width = 150, Format = DateTimePickerFormat.Time, ShowUpDown = true };

            btnSave = new Button { Text = "Save", Top = 210, Left = 120, Width = 100 };
            btnSave.Click += BtnSave_Click;

            this.Controls.Add(txtName);
            this.Controls.Add(cmbDifficulty);
            this.Controls.Add(dtpReminder);
            this.Controls.Add(btnSave);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a habit name.");
                return;
            }

            HabitResult.HabitName = txtName.Text.Trim();
            HabitResult.Difficulty = cmbDifficulty.SelectedItem.ToString();
            HabitResult.ReminderTime = dtpReminder.Value.TimeOfDay;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
