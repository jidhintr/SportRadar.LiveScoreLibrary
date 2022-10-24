using LiveScore.Library;
using LiveScore.Library.Models;
using static NUnit.Framework.Assert;

namespace UnitTest.LiveScore.Library
{
    public class LiveScoreTests
    {

        #region Props
        private Football _football;
        private Game game;
        private Team homeTeam;
        private Team awayTeam;

        private Game game2;
        private Team homeTeam2;
        private Team awayTeam2;

        private Game game3;
        private Team homeTeam3;
        private Team awayTeam3;

        private Game game4;
        private Team homeTeam4;
        private Team awayTeam4;

        #endregion

        [SetUp]
        public void Setup()
        {
            _football = new Football();
            ConfigureTeam();
        }
        private void ConfigureTeam()
        {
            homeTeam = new Team("Real Madrid", 0);
            awayTeam = new Team("FC Barcelona", 0);
            game = new Game(homeTeam, awayTeam);

            homeTeam2 = new Team("Manchester United", 0);
            awayTeam2 = new Team("AC Milan", 0);
            game2 = new Game(homeTeam2, awayTeam2);

            homeTeam3 = new Team("Liverpool", 0);
            awayTeam3 = new Team("Chelsea", 0);
            game3 = new Game(homeTeam3, awayTeam3);

            homeTeam4 = new Team("", -5);
            awayTeam4 = new Team(string.Empty, 101);
            game4 = new Game(homeTeam4, awayTeam4);

        }

        [Test]
        public void StartGame_ValidParams_ReturnsTrue()
        {
            var result = _football.StartGame(game);
            if (result.Item2 != null)
                IsTrue(result.Item1);
        }

        [Test]
        [TestCase()]
        public void StartGame_ValidParams_SingleGame_NullResponse()
        {
            var result = _football.StartGame(game);
            if (result.Item2 == null)
                IsFalse(result.Item1);
        }

        [Test]
        [TestCase()]
        public void StartGame_ValidParams_MultipleGame()
        {
            var result = _football.StartGame(game);
            Assert.IsTrue(result.Item1);

            var result2 = _football.StartGame(game2);
            Assert.IsTrue(result2.Item1);

            var result3 = _football.StartGame(game3);
            Assert.IsTrue(result3.Item1);

        }

        [Test] 
        public void StartGame_ValidParams_MultipleGame_ScoreCheck()
        {
            var result = _football.StartGame(game);
            IsTrue(result.Item2.IsLive);
            GreaterOrEqual(result.Item2.GameId, 1);
            GreaterOrEqual(result.Item2.Score.AwayTeam.Goal , 0);
            AreEqual(game.HomeTeam, result.Item2.Score.HomeTeam);
            
        }


        [Test]
        public void StartGame_ValidParams_MultipleGame_ScoreCheck_Invalidate()
        {
            var result = _football.StartGame(game4);
            IsFalse(result.Item1);
           
        }



    }
}