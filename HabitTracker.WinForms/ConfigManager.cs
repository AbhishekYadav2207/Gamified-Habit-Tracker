using System;
using System.IO;

namespace HabitTracker.WinForms
{
    public static class ConfigManager
    {
        private static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

        public static string GetDatabasePath()
        {
            if (File.Exists(ConfigPath))
            {
                var content = File.ReadAllText(ConfigPath);
                // Simple parsing without external libraries
                var path = content.Replace("{\"DbPath\":\"", "").Replace("\"}", "");
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    return path;
                }
            }
            return null;
        }

        public static void SetDatabasePath(string dbPath)
        {
            File.WriteAllText(ConfigPath, $"{{\"DbPath\":\"{dbPath.Replace("\\", "\\\\")}\"}}");
        }
    }
}
