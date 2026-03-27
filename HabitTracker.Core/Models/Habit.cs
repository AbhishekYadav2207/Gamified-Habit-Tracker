using System;

namespace HabitTracker.Core.Models
{
    public class Habit
    {
        public int HabitId { get; set; }
        public int UserId { get; set; } // Defines relationship filtering
        public string HabitName { get; set; }
        public string Difficulty { get; set; } // "Easy", "Medium", "Hard"
        public int CurrentStreak { get; set; }
        public TimeSpan ReminderTime { get; set; } // Smarter reminder
        public DateTime CreatedDate { get; set; }
    }
}
