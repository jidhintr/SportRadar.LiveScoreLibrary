using LiveScore.Library.Abstracts;
using LiveScore.Library.Events;
using LiveScore.Library.Models;
using LiveScore.Library.Utility;

namespace LiveScore.Library;

public sealed class LiveScoreController : IGameAction
{
    #region Props
    public event EventHandler<ScoreEventArgs>? OnLiveScoreChangeProcessCompleted;
    #endregion

    public LiveScoreController() => Globals.InternalScoreBoard = GameFactory.GetInternalScoreModelList();

    #region Contracts

    public (bool IsUpdated, Scores Score) UpdateScore(Game game)
    {
        try
        {
            var scoreBoard = GameFactory.GetScoreModel();
            var playingTeam = game.IsGameLive();
            if (playingTeam == null) return (false, scoreBoard);
            playingTeam.AwayTeam = game.AwayTeam;
            playingTeam.HomeTeam = game.HomeTeam;
            playingTeam.Message?.Append(Environment.NewLine).Append($"Goal scored at {DateTime.Now}");

            scoreBoard.GameId = playingTeam.GameHash;
            scoreBoard.IsLive = playingTeam.IsLive;
            scoreBoard.Game = new Game(playingTeam.HomeTeam, playingTeam.AwayTeam);
            scoreBoard.Message = playingTeam.Message?.ToString();

            ScoreUpdated(new ScoreEventArgs(scoreBoard));
            return (true, scoreBoard);
        }
        catch (Exception ex)
        {
            // log ex
            throw;
        }
    }

    public IEnumerable<Game> AllSummary()
    {
        var liveGames = (from game in Globals.InternalScoreBoard
                         let g1 = game.AwayTeam.Goal
                         let g2 = game.HomeTeam.Goal
                         select new
                         {
                             TotalScore = g1 + g2,
                             StartTime = game.StartTime,
                             Id = game.GameId,
                             Game = new Game(game.HomeTeam, game.AwayTeam)
                         });

        var result = liveGames.OrderByDescending(a => a.TotalScore)
            .ThenByDescending(b => b.StartTime)
            .Select(a => new Game(a.Game.HomeTeam, a.Game.AwayTeam));
        return result;
    }

    #endregion

    #region LocalFunctions
    
    private void ScoreUpdated(ScoreEventArgs args)
    {
        OnLiveScoreChangeProcessCompleted?.Invoke(this, args);
    }
    #endregion
}