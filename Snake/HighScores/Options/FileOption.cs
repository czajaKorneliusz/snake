using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Snake.Models;

namespace Snake.HighScores.Options
{
    public class FileOption : IHighScores
    {
        private readonly string _fileName = ConfigurationManager.AppSettings["highScoreStorageFileName"];

        public void SaveHighScore(HighScoreEntry highScoreEntry)
        {
            var currentHighScoreEntries = new List<HighScoreEntry>();
            if (File.Exists(_fileName)) currentHighScoreEntries = ReadHighScore();

            currentHighScoreEntries.Add(highScoreEntry);
            currentHighScoreEntries = currentHighScoreEntries.OrderBy(o => o.Score).Take(10).ToList();
            using var stream = new StreamWriter(_fileName, true);
            foreach (var item in currentHighScoreEntries) stream.WriteLine($"{item.Name};{item.Score}");
        }

        public List<HighScoreEntry> ReadHighScore()
        {
            var entries = new List<HighScoreEntry>();

            if (!File.Exists(_fileName)) return entries;

            using var reader = new StreamReader(_fileName);
            var lineStrings = reader.ReadLine()?.Split(';');
            if (lineStrings != null) entries.Add(new HighScoreEntry(lineStrings[0], Convert.ToInt32(lineStrings[1])));

            return entries;
        }

        public static FileOption CreateInstance()
        {
            return new FileOption();
        }
    }
}