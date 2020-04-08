using System.Collections.Generic;
using Snake.Models;

namespace Snake.HighScores
{
    public interface IHighScores
    {
        public void SaveHighScore(HighScoreEntry highScoreEntry);
        public List<HighScoreEntry> ReadHighScore();
    }
}