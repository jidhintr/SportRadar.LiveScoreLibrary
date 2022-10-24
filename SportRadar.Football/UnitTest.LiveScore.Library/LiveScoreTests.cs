using LiveScore.Library;
using LiveScore.Library.Models;
using static NUnit.Framework.Assert;

namespace UnitTest.LiveScore.Library
{
    public class LiveScoreTests
    {

        private Football football;
        [SetUp]
        public void Setup()
        {
            football = new Football();
        }

        [Test]
        public void StartGame_ValidParams_SingleGame()
        {
            var homeTeam = new Team("Real Madrid", 0);
            var awayTeam = new Team("FC Barcelona", 0);
            var game = new Game(homeTeam, awayTeam);

            var result = football.StartGame(game);
            
            IsTrue(result);
        }


    }
}