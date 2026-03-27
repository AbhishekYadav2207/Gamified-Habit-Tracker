using HabitTracker.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace HabitTracker.Core.Data
{
    public class HabitLogRepository
    {
        private readonly SqliteHelper _helper;

        public HabitLogRepository(string dbPath)
        {
            _helper = new SqliteHelper(dbPath);
        }

        public void AddLog(HabitLog log)
        {
            var cmd = "INSERT INTO HabitLogs (HabitId, Date, Completed) VALUES (@h, @d, @c)";
            _helper.ExecuteNonQuery(cmd,
                new SQLiteParameter("@h", log.HabitId),
                new SQLiteParameter("@d", log.Date.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@c", log.Completed ? 1 : 0));
        }

        public bool IsHabitCompletedToday(int habitId)
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var result = _helper.ExecuteScalar("SELECT COUNT(*) FROM HabitLogs WHERE HabitId = @h AND Date = @d AND Completed = 1",
                new SQLiteParameter("@h", habitId),
                new SQLiteParameter("@d", today));
            return Convert.ToInt32(result) > 0;
        }
        
        public int GetCompletedHabitsCountToday(int userId)
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var query = @"SELECT COUNT(*) FROM HabitLogs hl 
                          INNER JOIN Habits h ON hl.HabitId = h.HabitId 
                          WHERE h.UserId = @u AND hl.Date = @d AND hl.Completed = 1";
            var result = _helper.ExecuteScalar(query,
                new SQLiteParameter("@u", userId),
                new SQLiteParameter("@d", today));
            return Convert.ToInt32(result);
        }

        // Feature: check if yesterday was missed
        public bool DidMissYesterday(int habitId)
        {
            var yesterday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var result = _helper.ExecuteScalar("SELECT COUNT(*) FROM HabitLogs WHERE HabitId = @h AND Date = @d AND Completed = 1",
                new SQLiteParameter("@h", habitId),
                new SQLiteParameter("@d", yesterday));
            return Convert.ToInt32(result) == 0;
        }
    }
}
