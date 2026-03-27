using HabitTracker.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace HabitTracker.Core.Data
{
    public class HabitRepository
    {
        private readonly SqliteHelper _helper;

        public HabitRepository(string dbPath)
        {
            _helper = new SqliteHelper(dbPath);
        }

        public void AddHabit(Habit habit)
        {
            var cmd = @"INSERT INTO Habits (UserId, HabitName, Difficulty, CurrentStreak, ReminderTime, CreatedDate) 
                        VALUES (@u, @h, @d, 0, @r, @c)";
            _helper.ExecuteNonQuery(cmd,
                new SQLiteParameter("@u", habit.UserId),
                new SQLiteParameter("@h", habit.HabitName),
                new SQLiteParameter("@d", habit.Difficulty),
                new SQLiteParameter("@r", habit.ReminderTime.ToString()),
                new SQLiteParameter("@c", DateTime.Now.ToString("o")));
        }

        public void UpdateHabit(Habit habit)
        {
            var cmd = "UPDATE Habits SET HabitName = @h, Difficulty = @d, ReminderTime = @r, CurrentStreak = @s WHERE HabitId = @id AND UserId = @u";
            _helper.ExecuteNonQuery(cmd,
                new SQLiteParameter("@h", habit.HabitName),
                new SQLiteParameter("@d", habit.Difficulty),
                new SQLiteParameter("@r", habit.ReminderTime.ToString()),
                new SQLiteParameter("@s", habit.CurrentStreak),
                new SQLiteParameter("@id", habit.HabitId),
                new SQLiteParameter("@u", habit.UserId));
        }

        public void DeleteHabit(int habitId, int userId)
        {
            var cmd = "DELETE FROM Habits WHERE HabitId = @id AND UserId = @u";
            _helper.ExecuteNonQuery(cmd,
                new SQLiteParameter("@id", habitId),
                new SQLiteParameter("@u", userId));
        }

        public List<Habit> GetHabitsByUser(int userId)
        {
            var list = new List<Habit>();
            using (var conn = _helper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT * FROM Habits WHERE UserId = @u", conn))
                {
                    cmd.Parameters.AddWithValue("@u", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Habit
                            {
                                HabitId = Convert.ToInt32(reader["HabitId"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                HabitName = reader["HabitName"].ToString(),
                                Difficulty = reader["Difficulty"].ToString(),
                                CurrentStreak = Convert.ToInt32(reader["CurrentStreak"]),
                                ReminderTime = TimeSpan.Parse(reader["ReminderTime"].ToString()),
                                CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString())
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}
