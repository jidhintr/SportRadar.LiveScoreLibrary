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

        #region Start_Test
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
            GreaterOrEqual(result.Item2.Score.AwayTeam.Goal, 0);
            AreEqual(game.HomeTeam, result.Item2.Score.HomeTeam);

        }


        [Test]
        public void StartGame_ValidParams_MultipleGame_ScoreCheck_Invalidate()
        {
            var result = _football.StartGame(game4);
            IsFalse(result.Item1);

        }

        #endregion

        #region UpdateGAmeTests


        [Test]
        public void UpdateScore_AfterAddingSingleGameWithValidParams()
        {
            _football.StartGame(game3);
            game3 = new Game(new Team("Liverpool", 0), new Team("Chelsea", 1));
            var result = _football.UpdateScore(game3);
            IsTrue(result.Item1);
        }


        [Test]
        public void UpdateScore_SingleGameWithoutStartGame_ValidParams()
        {
            game3 = new Game(new Team("Liverpool", 0), new Team("Chelsea", 1));
            var result = _football.UpdateScore(game3);
            IsFalse(result.Item1);
        }


        [Test]
        public void UpdateScore_AfterAddingMultipleGameWithValidParams()
        {
            _football.StartGame(game);
            _football.StartGame(game2);

            // update game2 
            var homeTeamUpdated = new Team("Manchester United", 2);
            var awayTeamUpdated = new Team("AC Milan", 0);
            var updatedGame = new Game(homeTeamUpdated, awayTeamUpdated);
            var updatedGameResult = _football.UpdateScore(updatedGame);


            // update game 1
            var homeTeamUpdated2 = new Team("Real Madrid", 1);
            var awayTeamUpdated2 = new Team("FC Barcelona", 0);
            var updatedGame2 = new Game(homeTeamUpdated2, awayTeamUpdated2);
            var updatedGameResult2 = _football.UpdateScore(updatedGame2);


            IsTrue(updatedGameResult.Item1);
            IsTrue(updatedGameResult2.Item1);

            IsTrue(updatedGameResult.Item2.IsLive);
            IsTrue(updatedGameResult2.Item2.IsLive);

            IsTrue(updatedGameResult.Item2.IsLive);

            // validate scores passed 
            AreEqual(updatedGameResult.Item2.Score.HomeTeam.Goal, 2);
            AreEqual(updatedGameResult2.Item2.Score.HomeTeam.Goal, homeTeamUpdated2.Goal);

            AreEqual(updatedGameResult.Item2.Score.HomeTeam, homeTeamUpdated);
            AreEqual(updatedGameResult2.Item2.Score.AwayTeam, awayTeamUpdated2);

        } 
        #endregion

    }
}