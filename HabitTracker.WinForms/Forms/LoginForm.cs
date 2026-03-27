using System;
using System.Windows.Forms;
using HabitTracker.Core.Data;
using HabitTracker.Core.Models;

namespace HabitTracker.WinForms.Forms
{
    public class LoginForm : Form
    {
        private UserRepository _userRepo;
        public User LoggedInUser { get; private set; }

        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;

        public LoginForm(string dbPath)
        {
            _userRepo = new UserRepository(dbPath);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Login - Gamified Habit Tracker";
            this.Size = new System.Drawing.Size(300, 250);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Controls.Add(new Label { Text = "Username:", Top = 20, Left = 20 });
            txtUsername = new TextBox { Top = 40, Left = 20, Width = 200 };
            
            this.Controls.Add(new Label { Text = "Password:", Top = 70, Left = 20 });
            txtPassword = new TextBox { Top = 90, Left = 20, Width = 200, PasswordChar = '*' };

            btnLogin = new Button { Text = "Login", Top = 130, Left = 20, Width = 90 };
            btnLogin.Click += BtnLogin_Click;

            btnRegister = new Button { Text = "Register", Top = 130, Left = 130, Width = 90 };
            btnRegister.Click += BtnRegister_Click;

            this.Controls.Add(txtUsername);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnRegister);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text.Trim(); // In a real app we'd hash it

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Enter username and password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var user = _userRepo.Authenticate(username, password);
            if (user != null)
            {
                LoggedInUser = user;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Enter username and password to register.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_userRepo.Register(username, password))
            {
                MessageBox.Show("Registration successful. You can now log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Registration failed. Username might already exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
