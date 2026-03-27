# Gamified Habit Tracker (Offline-First)

This project contains a complete implementation of a Gamified Habit Tracker using C# (.NET Framework 4.8), Windows Forms, ASP.NET Web Forms, and SQLite (via ADO.NET). 

The requirements have been fully met, including the user feedback to include a real login in WinForms, streak freezes, missing DB fallback, smart reminders, simple local .json configs, related components, and a local backup mechanism.

## Solution Structure

1. **HabitTracker.Core (`net48`)**: A Class Library containing all SQLite Data Access logic, Game Engine logic, and Models.
2. **HabitTracker.WinForms (`net48`)**: The Windows Forms application.
3. **HabitTracker.Web**: An ASP.NET Web Site containing the mock login logic and dashboard.

## How to Run

### Windows Forms Application
Since this project uses modern SDK-style `.csproj` files targeted for `.NET 4.8`, you can open it easily:

1. Open `GamifiedHabitTracker.sln` in **Visual Studio 2022** (or 2019).
2. Right-click on **HabitTracker.WinForms** -> **Set as Startup Project**.
3. **Build Solution** (This will automatically restore `System.Data.SQLite` via NuGet).
4. **Run (F5)**.

**First-Time Setup Flow:**
When the WinForms app runs, it will detect if a path has been saved.
If not, it will prompt you to select a folder where `habit_tracker.db` will be created automatically. The path is saved in `config.json`.
You can then `Register` a new user via the Login screen and interact with the Gamified Tracker.

### Web Forms Application
The ASP.NET Web App is independent of the SQLite DB per project instructions. 
1. In Visual Studio, right-click on the `HabitTracker.Web` folder -> **View in Browser** (or **Set as Startup Project** if configured).
2. IIS Express will host the app.
3. Access the `Login.aspx` mock login with credentials: `admin` / `admin`.

## Features Included (Per requirements & feedback)
- **Local SQLite DB** managed by ADO.NET (`System.Data.SQLite`).
- **Core Logic**: XP & non-linear Level system, Streaks.
- **Improved Reminder System**: Checks vs actual habit time and skips if completed today.
- **"Streak Freeze" Mechanic**: If you miss a day, it consumes 1 freeze to save your streak.
- **Local DB Configuration**: Handled smoothly using a simple editable `config.json` fallback, with graceful handling if corrupted/deleted.
- **Relational Integrity**: Habits and Rewards correctly point to Logged-In User.
- **Visual Feedback & UI**: XP progression popups, color highlighting for completed habits, daily summary popups.
