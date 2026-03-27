using HabitTracker.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace HabitTracker.Core.Data
{
    public class RewardRepository
    {
        private readonly SqliteHelper _helper;

        public RewardRepository(string dbPath)
        {
            _helper = new SqliteHelper(dbPath);
        }

        public void AddReward(Reward reward)
        {
            var cmd = "INSERT INTO Rewards (UserId, RewardName, UnlockedAtLevel, IsUnlocked) VALUES (@u, @n, @l, @i)";
            _helper.ExecuteNonQuery(cmd,
                new SQLiteParameter("@u", reward.UserId),
                new SQLiteParameter("@n", reward.RewardName),
                new SQLiteParameter("@l", reward.UnlockedAtLevel),
                new SQLiteParameter("@i", reward.IsUnlocked ? 1 : 0));
        }

        public void UpdateRewardUnlockStatus(int rewardId, bool isUnlocked)
        {
            var cmd = "UPDATE Rewards SET IsUnlocked = @i WHERE RewardId = @id";
            _helper.ExecuteNonQuery(cmd,
                new SQLiteParameter("@i", isUnlocked ? 1 : 0),
                new SQLiteParameter("@id", rewardId));
        }

        public List<Reward> GetRewardsByUser(int userId)
        {
            var list = new List<Reward>();
            using (var conn = _helper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT * FROM Rewards WHERE UserId = @u", conn))
                {
                    cmd.Parameters.AddWithValue("@u", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Reward
                            {
                                RewardId = Convert.ToInt32(reader["RewardId"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                RewardName = reader["RewardName"].ToString(),
                                UnlockedAtLevel = Convert.ToInt32(reader["UnlockedAtLevel"]),
                                IsUnlocked = Convert.ToInt32(reader["IsUnlocked"]) > 0
                            });
                        }
                    }
                }
            }
            return list;
        }

        public void UnlockEligibleRewards(User user)
        {
            // Simple logic: if a reward's UnlockedAtLevel <= user.Level, set it to unlocked
            var rewards = GetRewardsByUser(user.UserId);
            foreach (var reward in rewards)
            {
                if (!reward.IsUnlocked && user.Level >= reward.UnlockedAtLevel)
                {
                    UpdateRewardUnlockStatus(reward.RewardId, true);
                }
            }
        }
    }
}
