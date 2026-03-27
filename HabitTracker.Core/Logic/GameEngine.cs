using HabitTracker.Core.Models;
using HabitTracker.Core.Data;

namespace HabitTracker.Core.Logic
{
    public class GameEngine
    {
        private readonly UserRepository _userRepo;
        private readonly HabitRepository _habitRepo;
        private readonly HabitLogRepository _logRepo;

        public GameEngine(string dbPath)
        {
            _userRepo = new UserRepository(dbPath);
            _habitRepo = new HabitRepository(dbPath);
            _logRepo = new HabitLogRepository(dbPath);
        }

        public int CalculateXP(string difficulty)
        {
            switch (difficulty.ToLower())
            {
                case "easy": return 10;
                case "medium": return 20;
                case "hard": return 30;
                default: return 10;
            }
        }

        public void CheckAndApplyStreakFreeze(Habit habit, User user)
        {
            // If they missed yesterday and they have a streak freeze, consume it and save the streak
            if (_logRepo.DidMissYesterday(habit.HabitId) && habit.CurrentStreak > 0)
            {
                if (user.AvailableFreezes > 0)
                {
                    user.AvailableFreezes--;
                    _userRepo.UpdateUserProgress(user);
                    // Streak freeze used! Outputting this to UI happens via events or direct message normally
                }
                else
                {
                    // Missed and no freezes left -> streak reset
                    habit.CurrentStreak = 0;
                }
            }
        }

        public void CompleteHabit(Habit habit, User user)
        {
            if (_logRepo.IsHabitCompletedToday(habit.HabitId))
            {
                return; // already completed today
            }

            // Streak freeze logic
            CheckAndApplyStreakFreeze(habit, user);

            // Add log
            _logRepo.AddLog(new HabitLog
            {
                HabitId = habit.HabitId,
                Date = System.DateTime.Now,
                Completed = true
            });

            // Update stats
            habit.CurrentStreak++;
            user.XP += CalculateXP(habit.Difficulty);
            
            // Level formula (non-linear): Level = 1 + Setpoint (e.g. 100XP per level base but squared) -> Level = (int)Math.Sqrt(XP / 50) + 1
            user.Level = (int)System.Math.Sqrt(user.XP / 50.0) + 1;

            _habitRepo.UpdateHabit(habit);
            _userRepo.UpdateUserProgress(user);
        }
    }
}
