<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Dashboard - Gamified Habit Tracker</title>
    <style>
        body { font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }
        header { background-color: #007bff; color: white; padding: 20px; text-align: center; }
        .container { max-width: 800px; margin: 20px auto; background: white; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }
        .welcome { font-size: 1.2em; margin-bottom: 20px; }
        .logout-btn { padding: 8px 15px; background-color: #dc3545; color: white; border: none; border-radius: 4px; cursor: pointer; float: right; }
        .info-card { background: #e9ecef; padding: 15px; border-radius: 6px; margin-bottom: 15px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <header>
            <h1>Gamified Habit Tracker - Web Dashboard</h1>
        </header>
        <div class="container">
            <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="logout-btn" OnClick="BtnLogout_Click" />
            <div class="welcome">
                Welcome, <asp:Label ID="lblUsername" runat="server" Font-Bold="true"></asp:Label>!
            </div>
            <div class="info-card">
                <h3>Offline Application Status</h3>
                <p>To view your live habits, XP, levels, and streak freezes, please launch the <strong>Windows Forms Desktop Application</strong>.</p>
                <p>The Gamified Habit Tracker is primarily an offline-first Windows desktop experience using local SQLite.</p>
            </div>
        </div>
    </form>
</body>
</html>
