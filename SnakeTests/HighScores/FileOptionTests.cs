using NUnit.Framework;
using Snake.HighScores;
using Snake.Models;
using System.Configuration;
using System.IO;

namespace SnakeTests.HighScores
{
    [TestFixture]
    public class FileOptionTests
    {
        private readonly string _fileName = ConfigurationManager.AppSettings["highScoreStorageFileName"];
        [SetUp]
        public void DerivedSetUp()
        {
            _highScores = new StorageMethodFactory().GetHighScoreOption("file");
        }

        [TearDown]
        public void DerivedTearDown()
        {
            if (File.Exists(_fileName))
            {
                File.Delete(_fileName);
            }
        }

        private IHighScores _highScores;


        [Test]
        public void ReadHighScoreTest()
        {
            _highScores.SaveHighScore(new HighScoreEntry("testEntry", 100));
            System.Collections.Generic.List<HighScoreEntry> list = _highScores.ReadHighScore();
            Assert.IsTrue(list.Count > 0);
        }

        [Test]
        public void SaveHighScoreTest()
        {
            _highScores.SaveHighScore(new HighScoreEntry("test", 100));
            Assert.IsTrue(File.Exists(_fileName));
        }
    }
}