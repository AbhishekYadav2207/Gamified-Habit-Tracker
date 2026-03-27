using System;
using System.Web.Security;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void BtnLogin_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            // Hardcoded mock as requested by specs since Web app does not access local SQLite directly
            if (txtUsername.Text == "admin" && txtPassword.Text == "admin")
            {
                FormsAuthentication.RedirectFromLoginPage(txtUsername.Text, false);
            }
            else
            {
                lblMessage.Text = "Invalid username or password. Try admin/admin.";
            }
        }
    }
}
