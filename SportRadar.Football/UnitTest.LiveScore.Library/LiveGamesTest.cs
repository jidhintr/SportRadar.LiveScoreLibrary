using LiveScore.Library;
using LiveScore.Library.Events;
using LiveScore.Library.Models;
using NUnit.Framework.Internal;
using static NUnit.Framework.Assert;

namespace UnitTest.LiveScore.Library;

public class LiveGamesTest
{
    [SetUp]
    public void Setup()
    {
        _gameController = new GameController();
        _liveScoreController = new LiveScoreController();
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

    private GameController _gameController;
    private LiveScoreController _liveScoreController;
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


    #region UpdateGAmeTests

    [Test]
    public void UpdateScore_AfterAddingSingleGameWithValidParams()
    {
        _gameController.StartGame(_game3);
        _game3 = new Game(new Team("Liverpool", 0), new Team("Chelsea", 1));
        var result = _liveScoreController.UpdateScore(_game3);
        IsTrue(result.IsUpdated);
    }


    [Test]
    public void UpdateScore_SingleGameWithoutStartGame_ValidParams()
    {
        _game3 = new Game(new Team("Liverpool", 0), new Team("Chelsea", 1));
        var result = _liveScoreController.UpdateScore(_game3);
        IsFalse(result.IsUpdated);
    }


    [Test]
    public void UpdateScore_AfterAddingMultipleGameWithValidParams()
    {
        _gameController.StartGame(_game);
        _gameController.StartGame(_game2);

        // update game2 
        var homeTeamUpdated = new Team("Manchester United", 2);
        var awayTeamUpdated = new Team("AC Milan", 0);
        var updatedGame = new Game(homeTeamUpdated, awayTeamUpdated);
        var updatedGameResult = _liveScoreController.UpdateScore(updatedGame);


        // update game 1
        var homeTeamUpdated2 = new Team("Real Madrid", 1);
        var awayTeamUpdated2 = new Team("FC Barcelona", 0);
        var updatedGame2 = new Game(homeTeamUpdated2, awayTeamUpdated2);
        var updatedGameResult2 = _liveScoreController.UpdateScore(updatedGame2);


        IsTrue(updatedGameResult.IsUpdated);
        IsTrue(updatedGameResult2.IsUpdated);

        IsTrue(updatedGameResult.Score.IsLive);
        IsTrue(updatedGameResult2.Score.IsLive);

        IsTrue(updatedGameResult.Score.IsLive);
        That(updatedGameResult.Score.Game.HomeTeam.Goal, Is.EqualTo(2));
        That(updatedGameResult2.Score.Game.HomeTeam.Goal, Is.EqualTo(1));

        AreEqual(homeTeamUpdated, updatedGameResult.Score.Game.HomeTeam);
        AreEqual(awayTeamUpdated2, updatedGameResult2.Score.Game.AwayTeam);
    }



    #endregion

    #region SummaryTests

    [Test]
    public void Summary_AddMultipleGame_Sorted()
    {
        var gameStat = _gameController.StartGame(_game);
        Assert.IsTrue(gameStat.IsStarted);
        //Thread.Sleep(31000);
        _gameController.StartGame(_game2);
        //Thread.Sleep(59000);
        _gameController.StartGame(_game3);
        //Thread.Sleep(21000);
        _gameController.StartGame(_game5);
        //Thread.Sleep(39000);
        _gameController.StartGame(_game6);


        var homeTeamUpdated = new Team("Manchester United", 2);
        var awayTeamUpdated = new Team("AC Milan", 0);
        var updatedGame = new Game(homeTeamUpdated, awayTeamUpdated);
        var updatedGameResult = _liveScoreController.UpdateScore(updatedGame);

        var homeTeamUpdated2 = new Team("Real Madrid", 1);
        var awayTeamUpdated2 = new Team("FC Barcelona", 2);
        var updatedGame2 = new Game(homeTeamUpdated2, awayTeamUpdated2);
        var updatedGameResult2 = _liveScoreController.UpdateScore(updatedGame2);

        var homeTeamUpdated3 = new Team("Liverpool", 1);
        var awayTeamUpdated3 = new Team("Chelsea", 1);
        var updatedGame3 = new Game(homeTeamUpdated3, awayTeamUpdated3);
        var updatedGameResult3 = _liveScoreController.UpdateScore(updatedGame3);

        var homeTeamUpdated5 = new Team("Spain", 1);
        var awayTeamUpdated5 = new Team("Portugal", 2);
        var updatedGame5 = new Game(homeTeamUpdated5, awayTeamUpdated5);
        var updatedGameResult5 = _liveScoreController.UpdateScore(updatedGame5);

        var homeTeamUpdated6 = new Team("Poland", 2);
        var awayTeamUpdated6 = new Team("Germany", 2);
        var updatedGame6 = new Game(homeTeamUpdated6, awayTeamUpdated6);
        var updatedGameResult6 = _liveScoreController.UpdateScore(updatedGame6);

        var result = _liveScoreController.Summary().ToList();

        AreEqual(updatedGame6, result[0]);
        AreEqual(updatedGame5, result[1]);
        AreEqual(updatedGame2, result[2]);
        AreEqual(updatedGame3, result[3]);
        AreEqual(updatedGame, result[4]);

        AreEqual(2, result[0].AwayTeam.Goal);
        AreEqual(2, result[0].HomeTeam.Goal);

    }

    #endregion

    #region EventTest


    [Test]
    public void OnGameStatusChangeProcessCompleted_StartGameEventWithValidGame_ExpectedTrue()
    {
        _gameController.OnGameStatusChangeProcessCompleted += IsGameStatusChanged;
        _gameController.StartGame(_game);

    }

    [Test]
    public void OnGameStatusChangeProcessCompleted_StartGameEventWithInValidGame_ExpectedFailure()
    {
        _gameController.OnGameStatusChangeProcessCompleted += IsGameStatusChanged2;
        _gameController.StartGame(_game4);
    }


    [Test]
    public void OnScoreChangeProcessCompleted_StartGameWithUpdateEventWithValidGame()
    {
        _eventScore = new Scores();
        _liveScoreController.OnLiveScoreChangeProcessCompleted += LiveScoreChanged;
        _gameController.StartGame(_game);
        _gameController.StartGame(_game2);

        // update game2 
        var homeTeamUpdated = new Team("Manchester United", 2);
        var awayTeamUpdated = new Team("AC Milan", 0);
        var updatedGame = new Game(homeTeamUpdated, awayTeamUpdated);
        IsNotNull(_eventScore);
        var updatedGameResult = _liveScoreController.UpdateScore(updatedGame);
        AreEqual(_eventScore, updatedGameResult.Score);
        AreEqual(_eventScore.Game.HomeTeam.Goal, homeTeamUpdated.Goal);
        // update game 1
        var homeTeamUpdated2 = new Team("Real Madrid", 1);
        var awayTeamUpdated2 = new Team("FC Barcelona", 0);
        var updatedGame2 = new Game(homeTeamUpdated2, awayTeamUpdated2);
        var updatedGameResult2 = _liveScoreController.UpdateScore(updatedGame2);

        AreEqual(_eventScore, updatedGameResult2.Score);
        AreEqual(1, homeTeamUpdated2.Goal);
    }

    #endregion


    private Scores? _eventScore;
    private void LiveScoreChanged(object sender, ScoreEventArgs e) => _eventScore = e.UpdatedScore;


    private void IsGameStatusChanged(object sender, bool e) => IsTrue(e);

    private void IsGameStatusChanged2(object sender, bool e) => IsFalse(e);
}
