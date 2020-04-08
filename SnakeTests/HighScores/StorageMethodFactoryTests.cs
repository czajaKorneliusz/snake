using NUnit.Framework;

namespace Snake.HighScores.Tests
{
    [TestFixture]
    public class StorageMethodFactoryTests
    {
        [Test]
        public void GetHighScoreOptionTest()
        {
            Assert.IsNull(new StorageMethodFactory().GetHighScoreOption("somethingThatDoesNotExists"));
        }
    }
}