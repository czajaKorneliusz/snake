using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Snake.HighScores.Options;

namespace Snake.HighScores
{
    public class StorageMethodFactory
    {
        public IHighScores GetHighScoreOption(string storageType)
        {
            if (storageType.Equals("msSqlDatabase", StringComparison.OrdinalIgnoreCase))
                return DatabaseOption.CreateInstance(new DbContextOptionsBuilder<HighScoreDbContext>()
                    .UseSqlServer(ConfigurationManager.ConnectionStrings["MsSqlConnection"].ConnectionString).Options);
            if (storageType.Equals("sqLiteDatabase", StringComparison.OrdinalIgnoreCase))
                return DatabaseOption.CreateInstance(new DbContextOptionsBuilder<HighScoreDbContext>()
                    .UseSqlite(ConfigurationManager.ConnectionStrings["MySqLiteConnection"].ConnectionString).Options);
            if (storageType.Equals("file", StringComparison.OrdinalIgnoreCase))
                return FileOption.CreateInstance();
            return null;
        }
    }
}