<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Web Login - Habit Tracker</title>
    <style>
        body { font-family: Arial, sans-serif; background-color: #f4f4f4; display: flex; justify-content: center; align-items: center; height: 100vh; margin: 0; }
        .login-container { background: white; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.1); width: 300px; text-align: center; }
        .input-group { margin-bottom: 15px; text-align: left; }
        .input-group label { display: block; margin-bottom: 5px; }
        .input-group input { width: 100%; padding: 8px; box-sizing: border-box; }
        .btn { padding: 10px 20px; background-color: #007bff; color: white; border: none; border-radius: 4px; cursor: pointer; width: 100%; }
        .btn:hover { background-color: #0056b3; }
        .error { color: red; font-size: 0.9em; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <h2>Web Dashboard Login</h2>
            <p style="font-size: 0.8em; color: gray;">(Mock Authentication: admin/admin)</p>
            <div class="input-group">
                <label>Username</label>
                <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername" ErrorMessage="Required" CssClass="error" Display="Dynamic" />
            </div>
            <div class="input-group">
                <label>Password</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Required" CssClass="error" Display="Dynamic" />
            </div>
            <asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label>
            <br /><br />
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn" OnClick="BtnLogin_Click" />
        </div>
    </form>
</body>
</html>
