using System;
using System.Web.Security;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!User.Identity.IsAuthenticated)
        {
            FormsAuthentication.RedirectToLoginPage();
        }
        else
        {
            lblUsername.Text = User.Identity.Name;
        }
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
        Response.Redirect("Login.aspx");
    }
}
