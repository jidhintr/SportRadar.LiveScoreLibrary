using LiveScore.Library;
using LiveScore.Library.Models;

namespace UnitTest.LiveScore.Library;

public class GameControllerTest
{
    private readonly GameController _gameController;
    private Game _game;
    private Team _homeTeam;
    private Team _awayTeam;

    private Game _game2;
    private Team _homeTeam2;
    private Team _awayTeam2;

    private Game _game4;
    private Team _homeTeam4;
    private Team _awayTeam4;
    public GameControllerTest()
    {
        _gameController = new GameController();
    }

    [SetUp]
    public void Setup()
    {
        _homeTeam = new Team("Real Madrid", 0);
        _awayTeam = new Team("FC Barcelona", 0);
        _game = new Game(_homeTeam, _awayTeam);

        _homeTeam2 = new Team("Manchester United", 0);
        _awayTeam2 = new Team("AC Milan", 0);
        _game2 = new Game(_homeTeam2, _awayTeam2);


        _homeTeam4 = new Team("", -5);
        _awayTeam4 = new Team(string.Empty, 101);
        _game4 = new Game(_homeTeam4, _awayTeam4);
    }


    #region Start_Test

    [Test]
    public void StartGame_ValidParams_ReturnsTrue()
    {
        var result = _gameController.StartGame(_game);
        if (result.Score != null)
            Assert.IsTrue(result.IsStarted);
    }

    [Test]
    [TestCase]
    public void StartGame_ValidParams_SingleGame_NullResponse()
    {
        var result = _gameController.StartGame(_game);
        if (result.Score == null)
            Assert.IsFalse(result.IsStarted);
    }

    [Test]
    [TestCase]
    public void StartGame_ValidParams_MultipleGame()
    {
        var result = _gameController.StartGame(_game);
        Assert.IsTrue(result.IsStarted);

        var result2 = _gameController.StartGame(_game2);
        Assert.IsTrue(result2.IsStarted);

    }

    [Test]
    public void StartGame_ValidParams_MultipleGame_ScoreCheck()
    {
        var expected = new Team("Real Madrid", 0);
        var result = _gameController.StartGame(_game);
        Assert.IsTrue(result.Score.IsLive);
        Assert.GreaterOrEqual(result.Score.GameId, 1);
        Assert.GreaterOrEqual(result.Score.Game.AwayTeam.Goal, 0);
        Assert.That(result.Score.Game.HomeTeam, Is.EqualTo(expected));
    }


    [Test]
    public void StartGame_ValidParams_MultipleGame_ScoreCheck_Invalidate()
    {
        var result = _gameController.StartGame(_game4);
        Assert.IsFalse(result.IsStarted);
    }

    [Test]
    [TestCase]
    public void StartGame_ValidParams_SingleGame_ToStringCheck()
    {
        var result = _gameController.StartGame(_game);
        Assert.That(result.Score.ToString(), Is.EqualTo("Real Madrid 0 - FC Barcelona 0"));
    }
    #endregion

    #region FinishGame
    [Test]
    public void FinishGame_NoLiveGames_ExpectFailure()
    {
        var result = _gameController.FinishGame(_game);
        Assert.IsFalse(result);
    }

    [Test]
    public void FinishGame_AddOneValidGame_RemoveSame()
    {
        StartGame_ValidParams_ReturnsTrue(); // reuse existing unit-test for nested functions 
        var result = _gameController.FinishGame(_game);
        Assert.IsTrue(result);
    }


    [Test]
    public void FinishGame_AddMultipleGame_RemoveMultiple()
    {
        StartGame_ValidParams_MultipleGame(); // reuse existing unit-test for nested functions 
        var result = _gameController.FinishGame(_game);
        Assert.IsTrue(result);

        var result2 = _gameController.FinishGame(_game2);
        Assert.IsTrue(result2);

        var result3 = _gameController.FinishGame(_game4);
        Assert.IsFalse( result3);

    }

    #endregion
}