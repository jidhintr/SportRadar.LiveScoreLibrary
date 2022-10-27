using System.Diagnostics;
using LiveScore.Library;
using LiveScore.Library.Events;
using LiveScore.Library.Models;
using NUnit.Framework.Internal;
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

        _homeTeam5 = new Team("Spain", 0);
        _awayTeam5 = new Team("Portugal", 0);
        _game5 = new Game(_homeTeam5, _awayTeam5);

        _homeTeam6 = new Team("Poland", 0);
        _awayTeam6 = new Team("Germany", 0);
        _game6 = new Game(_homeTeam6, _awayTeam6);
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
    private Team _homeTeam5;
    private Team _awayTeam5;
    private Game _game5;
    private Team _homeTeam6;
    private Team _awayTeam6;
    private Game _game6;

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

    #region FinishGame
    [Test]
    public void FinishGame_NoLiveGames_ExpectFailure()
    {
        var result = _football.FinishGame(_game);
        IsFalse(result);
    }

    [Test]
    public void FinishGame_AddOneValidGame_RemoveSame()
    {
        StartGame_ValidParams_ReturnsTrue(); // reuse existing unit-test for nested functions 
        var result = _football.FinishGame(_game);
        IsTrue(result);
    }


    [Test]
    public void FinishGame_AddMultipleGame_RemoveMultiple()
    {
        StartGame_ValidParams_MultipleGame(); // reuse existing unit-test for nested functions 
        var result = _football.FinishGame(_game);
        IsTrue(result);

        var result2 = _football.FinishGame(_game2);
        IsTrue(result2);

        var result3 = _football.FinishGame(_game3);
        IsTrue(result3);

    }

    #endregion


    #region SummaryTests

    [Test]
    public void Summary_AddMultipleGame_Sorted()
    {
        var gameStat = _football.StartGame(_game);
        Assert.IsTrue(gameStat.IsStarted);
        //Thread.Sleep(31000);
        _football.StartGame(_game2);
        //Thread.Sleep(59000);
        _football.StartGame(_game3);
        //Thread.Sleep(21000);
        _football.StartGame(_game5);
        //Thread.Sleep(39000);
        _football.StartGame(_game6);


        var homeTeamUpdated = new Team("Manchester United", 2);
        var awayTeamUpdated = new Team("AC Milan", 0);
        var updatedGame = new Game(homeTeamUpdated, awayTeamUpdated);
        var updatedGameResult = _football.UpdateScore(updatedGame);

        var homeTeamUpdated2 = new Team("Real Madrid", 1);
        var awayTeamUpdated2 = new Team("FC Barcelona", 2);
        var updatedGame2 = new Game(homeTeamUpdated2, awayTeamUpdated2);
        var updatedGameResult2 = _football.UpdateScore(updatedGame2);

        var homeTeamUpdated3 = new Team("Liverpool", 1);
        var awayTeamUpdated3 = new Team("Chelsea", 1);
        var updatedGame3 = new Game(homeTeamUpdated3, awayTeamUpdated3);
        var updatedGameResult3 = _football.UpdateScore(updatedGame3);

        var homeTeamUpdated5 = new Team("Spain", 1);
        var awayTeamUpdated5 = new Team("Portugal", 2);
        var updatedGame5 = new Game(homeTeamUpdated5, awayTeamUpdated5);
        var updatedGameResult5 = _football.UpdateScore(updatedGame5);

        var homeTeamUpdated6 = new Team("Poland", 2);
        var awayTeamUpdated6 = new Team("Germany", 2);
        var updatedGame6 = new Game(homeTeamUpdated6, awayTeamUpdated6);
        var updatedGameResult6 = _football.UpdateScore(updatedGame6);

        var result = _football.AllSummary();

        AreEqual(result[0], updatedGame6);
        AreEqual(result[1], updatedGame5);
        AreEqual(result[2], updatedGame2);
        AreEqual(result[3], updatedGame3);
        AreEqual(result[4], updatedGame);

        AreEqual(result[0].AwayTeam.Goal, 2);
        AreEqual(result[0].HomeTeam.Goal, 2);

    }

    #endregion

    [Test]
    public void OnGameStatusChangeProcessCompleted_StartGameEventWithValidGame_ExpectedTrue()
    {
        _football.OnGameStatusChangeProcessCompleted += IsGameStatusChanged;
        _football.StartGame(_game);

    }

    [Test]
    public void OnGameStatusChangeProcessCompleted_StartGameEventWithInValidGame_ExpectedFailure()
    {
        _football.OnGameStatusChangeProcessCompleted += IsGameStatusChanged2;
        _football.StartGame(_game4);
    }


    [Test]
    public void OnScoreChangeProcessCompleted_StartGameWithUpdateEventWithValidGame()
    {

        _eventScore = new Scores();
        _football.OnLiveScoreChangeProcessCompleted += LiveScoreChanged;
        _football.StartGame(_game);
        _football.StartGame(_game2);

        // update game2 
        var homeTeamUpdated = new Team("Manchester United", 2);
        var awayTeamUpdated = new Team("AC Milan", 0);
        var updatedGame = new Game(homeTeamUpdated, awayTeamUpdated);
        IsNotNull(_eventScore);
        var updatedGameResult = _football.UpdateScore(updatedGame);
        AreEqual(updatedGameResult.Score, _eventScore);
        AreEqual(homeTeamUpdated.Goal, _eventScore.Game.HomeTeam.Goal);
        // update game 1
        var homeTeamUpdated2 = new Team("Real Madrid", 1);
        var awayTeamUpdated2 = new Team("FC Barcelona", 0);
        var updatedGame2 = new Game(homeTeamUpdated2, awayTeamUpdated2);
        var updatedGameResult2 = _football.UpdateScore(updatedGame2);

        AreEqual(updatedGameResult2.Score, _eventScore);
        AreEqual(homeTeamUpdated2.Goal, 1);
    }

    private Scores _eventScore;
    private void LiveScoreChanged(object sender, ScoreEventArgs e) => _eventScore = e.UpdatedScore;


    private void IsGameStatusChanged(object sender, bool e) => IsTrue(e);

    private void IsGameStatusChanged2(object sender, bool e) => IsFalse(e);
}
