using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Snake.Models;

namespace Snake.HighScores.Options
{
    public class DatabaseOption : IHighScores
    {
        private readonly HighScoreDbContext _dbContext;

        private DatabaseOption(DbContextOptions<HighScoreDbContext> options)
        {
            _dbContext = new HighScoreDbContext(options);
        }

        public void SaveHighScore(HighScoreEntry highScoreEntry)
        {
            Task.Run(() => SaveToDatabase(highScoreEntry));
        }

        public List<HighScoreEntry> ReadHighScore()
        {
            try
            {
                return _dbContext.Entries.Select(x => x).OrderByDescending(x => x.Score).ToList();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                return new List<HighScoreEntry>();
            }
        }

        public static DatabaseOption CreateInstance(DbContextOptions<HighScoreDbContext> options)
        {
            return new DatabaseOption(options);
        }


        private void SaveToDatabase(HighScoreEntry entry)
        {
            _dbContext.Entries.Add(entry);
            _dbContext.SaveChanges();
        }
    }
}