using System.ComponentModel.DataAnnotations;

namespace Snake.Models
{
    public class HighScoreEntry
    {
        public HighScoreEntry(string name, int score)
        {
            Name = name;
            Score = score;
        }

        [Key] [Required] public int Id { get; set; }

        public string Name { get; set; }
        public int Score { get; set; }
    }
}