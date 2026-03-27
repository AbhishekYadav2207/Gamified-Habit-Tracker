using System;

namespace HabitTracker.Core.Models
{
    public class HabitLog
    {
        public int LogId { get; set; }
        public int HabitId { get; set; } // Proper relationships
        public DateTime Date { get; set; }
        public bool Completed { get; set; }
    }
}
