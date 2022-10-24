using LiveScore.Library;
using LiveScore.Library.Models;
using static NUnit.Framework.Assert;

namespace UnitTest.LiveScore.Library
{
    public class LiveScoreTests
    {

        private Football _football;
        [SetUp]
        public void Setup()
        {
            _football = new Football();
        }

        [Test]
        public void StartGame_ValidParams_ReturnsTrue()
        {
            var homeTeam = new Team("Real Madrid", 0);
            var awayTeam = new Team("FC Barcelona", 0);
            var game = new Game(homeTeam, awayTeam);

            var result = _football.StartGame(game);
            if (result.Item2 != null)
                IsTrue(result.Item1)
                    ;
        }

        [Test]
        public void StartGame_ValidParams_SingleGame_NullResponse()
        {
            var homeTeam = new Team("Real Madrid", 0);
            var awayTeam = new Team("FC Barcelona", 0);
            var game = new Game(homeTeam, awayTeam);

            var result = _football.StartGame(game);
            if (result.Item2 == null)
                IsFalse(result.Item1);
        }



    }
}