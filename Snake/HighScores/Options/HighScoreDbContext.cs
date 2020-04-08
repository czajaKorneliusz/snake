using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Snake.Models;

namespace Snake.HighScores.Options
{
    public sealed class HighScoreDbContext : DbContext
    {
        public HighScoreDbContext()
        {
        }

        public HighScoreDbContext(DbContextOptions options)
            : base(options)
        {
            try
            {
                Database.EnsureCreated();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
            }
        }

        public DbSet<HighScoreEntry> Entries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Method intentionally left empty.
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Method intentionally left empty.
        }
    }
}