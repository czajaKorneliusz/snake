using System;
using System.Collections.ObjectModel;
using System.Configuration;
using Snake.HighScores;
using Snake.Models;

namespace Snake.ViewModels
{
    public class HighScoreViewModel
    {
        public HighScoreViewModel()
        {
            try
            {
                HighScoreEntries = new ObservableCollection<HighScoreEntry>(new StorageMethodFactory()
                    .GetHighScoreOption(ConfigurationManager.AppSettings["highScoreStorageMethod"])
                    .ReadHighScore());
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
        }

        public ObservableCollection<HighScoreEntry> HighScoreEntries { get; }
    }
}