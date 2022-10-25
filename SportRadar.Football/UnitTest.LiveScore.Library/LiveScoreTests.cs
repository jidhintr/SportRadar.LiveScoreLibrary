using LiveScore.Library;
using LiveScore.Library.Models;
using static NUnit.Framework.Assert;

namespace UnitTest.LiveScore.Library;

public class LiveScoreTests
{
    [SetUp]
    public void Setup()
    {
        _football = new Football();
        ConfigureTeam();
    }

    private void ConfigureTeam()
    {
        _homeTeam = new Team("Real Madrid", 0);
        _awayTeam = new Team("FC Barcelona", 0);
        _game = new Game(_homeTeam, _awayTeam);

        _homeTeam2 = new Team("Manchester United", 0);
        _awayTeam2 = new Team("AC Milan", 0);
        _game2 = new Game(_homeTeam2, _awayTeam2);

        _homeTeam3 = new Team("Liverpool", 0);
        _awayTeam3 = new Team("Chelsea", 0);
        _game3 = new Game(_homeTeam3, _awayTeam3);

        _homeTeam4 = new Team("", -5);
        _awayTeam4 = new Team(string.Empty, 101);
        _game4 = new Game(_homeTeam4, _awayTeam4);
    }

    #region Props

    private Football _football;
    private Game _game;
    private Team _homeTeam;
    private Team _awayTeam;

    private Game _game2;
    private Team _homeTeam2;
    private Team _awayTeam2;

    private Game _game3;
    private Team _homeTeam3;
    private Team _awayTeam3;

    private Game _game4;
    private Team _homeTeam4;
    private Team _awayTeam4;

    #endregion

    #region Start_Test

    [Test]
    public void StartGame_ValidParams_ReturnsTrue()
    {
        var result = _football.StartGame(_game);
        if (result.Item2 != null)
            IsTrue(result.IsStarted);
    }

    [Test]
    [TestCase]
    public void StartGame_ValidParams_SingleGame_NullResponse()
    {
        var result = _football.StartGame(_game);
        if (result.Item2 == null)
            IsFalse(result.IsStarted);
    }

    [Test]
    [TestCase]
    public void StartGame_ValidParams_MultipleGame()
    {
        var result = _football.StartGame(_game);
        IsTrue(result.IsStarted);

        var result2 = _football.StartGame(_game2);
        IsTrue(result2.IsStarted);

        var result3 = _football.StartGame(_game3);
        IsTrue(result3.IsStarted);
    }

    [Test]
    public void StartGame_ValidParams_MultipleGame_ScoreCheck()
    {
        var result = _football.StartGame(_game);
        IsTrue(result.Score.IsLive);
        GreaterOrEqual(result.Score.GameId, 1);
        GreaterOrEqual(result.Score.Game.AwayTeam.Goal, 0);
        AreEqual(_game.HomeTeam, result.Score.Game.HomeTeam);
    }


    [Test]
    public void StartGame_ValidParams_MultipleGame_ScoreCheck_Invalidate()
    {
        var result = _football.StartGame(_game4);
        IsFalse(result.IsStarted);
    }

    #endregion

    #region UpdateGAmeTests

    [Test]
    public void UpdateScore_AfterAddingSingleGameWithValidParams()
    {
        _football.StartGame(_game3);
        _game3 = new Game(new Team("Liverpool", 0), new Team("Chelsea", 1));
        var result = _football.UpdateScore(_game3);
        IsTrue(result.IsUpdated);
    }


    [Test]
    public void UpdateScore_SingleGameWithoutStartGame_ValidParams()
    {
        _game3 = new Game(new Team("Liverpool", 0), new Team("Chelsea", 1));
        var result = _football.UpdateScore(_game3);
        IsFalse(result.IsUpdated);
    }


    [Test]
    public void UpdateScore_AfterAddingMultipleGameWithValidParams()
    {
        _football.StartGame(_game);
        _football.StartGame(_game2);

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


        IsTrue(updatedGameResult.IsUpdated);
        IsTrue(updatedGameResult2.IsUpdated);

        IsTrue(updatedGameResult.Score.IsLive);
        IsTrue(updatedGameResult2.Score.IsLive);

        IsTrue(updatedGameResult.Score.IsLive);

        // validate scores passed 
        AreEqual(updatedGameResult.Score.Game.HomeTeam.Goal, 2);
        AreEqual(updatedGameResult2.Score.Game.HomeTeam.Goal, homeTeamUpdated2.Goal);

        AreEqual(updatedGameResult.Score.Game.HomeTeam, homeTeamUpdated);
        AreEqual(updatedGameResult2.Score.Game.AwayTeam, awayTeamUpdated2);
    }

    #endregion
}