namespace HabitTracker.Core.Models
{
    public class Reward
    {
        public int RewardId { get; set; }
        public int UserId { get; set; } // Filtered by user
        public string RewardName { get; set; }
        public int UnlockedAtLevel { get; set; }
        public bool IsUnlocked { get; set; }
    }
}
