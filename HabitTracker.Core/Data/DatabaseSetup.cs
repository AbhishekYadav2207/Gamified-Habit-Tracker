using System;
using System.IO;

namespace HabitTracker.Core.Data
{
    public class DatabaseSetup
    {
        public static void InitializeDatabase(string dbPath)
        {
            // If DB doesn't exist, SQLite automatically creates the file on first connection open.
            // But let's be explicit.
            if (!File.Exists(dbPath))
            {
                System.Data.SQLite.SQLiteConnection.CreateFile(dbPath);
            }

            var helper = new SqliteHelper(dbPath);

            // Create Users Table
            helper.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS Users (
                    UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT UNIQUE NOT NULL,
                    PasswordHash TEXT NOT NULL,
                    Level INTEGER NOT NULL DEFAULT 1,
                    XP INTEGER NOT NULL DEFAULT 0,
                    AvailableFreezes INTEGER NOT NULL DEFAULT 1
                );
            ");

            // Create Habits Table
            helper.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS Habits (
                    HabitId INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL,
                    HabitName TEXT NOT NULL,
                    Difficulty TEXT NOT NULL,
                    CurrentStreak INTEGER NOT NULL DEFAULT 0,
                    ReminderTime TEXT NOT NULL,
                    CreatedDate TEXT NOT NULL,
                    FOREIGN KEY(UserId) REFERENCES Users(UserId) ON DELETE CASCADE
                );
            ");

            // Create HabitLogs Table
            helper.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS HabitLogs (
                    LogId INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabitId INTEGER NOT NULL,
                    Date TEXT NOT NULL,
                    Completed INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY(HabitId) REFERENCES Habits(HabitId) ON DELETE CASCADE
                );
            ");

            // Create Rewards Table
            helper.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS Rewards (
                    RewardId INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL,
                    RewardName TEXT NOT NULL,
                    UnlockedAtLevel INTEGER NOT NULL,
                    IsUnlocked INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY(UserId) REFERENCES Users(UserId) ON DELETE CASCADE
                );
            ");
        }
    }
}
