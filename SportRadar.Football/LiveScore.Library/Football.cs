using LiveScore.Library.Abstracts;
using LiveScore.Library.Events;
using LiveScore.Library.Models;
using LiveScore.Library.Utility;
using System.Text;

namespace LiveScore.Library;

public class Football : IGameAction
{
    #region Props

    private readonly List<InternalScoreModel> _internalScoreBoard;
    public event EventHandler<bool> OnGameStatusChangeProcessCompleted;
    public event EventHandler<ScoreEventArgs> OnLiveScoreChangeProcessCompleted;
    #endregion

    public Football()
    {
        _internalScoreBoard = new List<InternalScoreModel>();
    }

    #region Functions

    public (bool IsStarted, Scores Score) StartGame(Game game)
    {
        try
        {
            if (!game.IsValid())
            {
                OnGameStatusChanged(false);
                return new ValueTuple<bool, Scores>(false, null);
            }

            var startTime = TimeOnly.FromDateTime(DateTime.Now);
            var endTime = startTime.AddMinutes(90);
            var liveGames = _internalScoreBoard.Count;
            var gameId = liveGames + 1;

            // internal model for DB / data manipulation 
            var score = new InternalScoreModel
            {
                GameId = gameId,
                AwayTeam = game.AwayTeam,
                HomeTeam = game.HomeTeam,
                IsLive = true,
                StartTime = startTime,
                LastUpdatedOn = TimeOnly.FromDateTime(DateTime.Now),
                Message = new StringBuilder(
                    $"Game between {game.AwayTeam} and {game.HomeTeam} started at {startTime} and expected to finish by {endTime}")
            };
            _internalScoreBoard.Add(score);

            // TODO : Use automapper to map result => 
            var result = new Scores
            {
                GameId = gameId,
                IsLive = score.IsLive,
                Game = game,
                Message = score.Message.ToString()
            };


            if (_internalScoreBoard.Count == gameId)
            {
                OnGameStatusChanged(true);
                return (true, result);
            }

            return (false, null);
        }
        catch (Exception)
        {
            // log ex
            throw;
        }
    }

    public (bool IsUpdated, Scores Score) UpdateScore(Game game)
    {
        try
        {

            var playingTeam = IsGameExists(game);
            if (playingTeam == null) return (false, new Scores());
            playingTeam.AwayTeam = game.AwayTeam;
            playingTeam.HomeTeam = game.HomeTeam;
            playingTeam.Message?.Append(Environment.NewLine).Append($"Goal scored at {DateTime.Now}");

            var updatedScore = new Scores
            {
                GameId = playingTeam.GameId,
                IsLive = playingTeam.IsLive,
                Game = new Game(playingTeam.HomeTeam, playingTeam.AwayTeam),
                Message = playingTeam.Message?.ToString()
            };

            ScoreUpdated(new ScoreEventArgs(updatedScore));
            return (true, updatedScore);
        }
        catch (Exception ex)
        {
            // log ex
            throw;
        }
    }

    public List<Game> AllSummary()
    {
        var lis = (from game in _internalScoreBoard
                   let g1 = game.AwayTeam.Goal
                   let g2 = game.HomeTeam.Goal
                   select new
                   {
                       TotalScore = g1 + g2,
                       StartTime = game.StartTime,
                       Id = game.GameId,
                       Game = new Game(game.HomeTeam, game.AwayTeam)
                   });

        var result = lis.OrderByDescending(a => a.TotalScore)
            .ThenByDescending(b => b.StartTime)
            .Select(a => new Game(a.Game.HomeTeam, a.Game.AwayTeam));
        return result.ToList();
    }

    public bool FinishGame(Game game)
    {
        try
        {
            var playingGame = IsGameExists(game);
            // notify the summary model
            if (playingGame != null)
            {
                _internalScoreBoard.Remove(playingGame);
                OnGameStatusChanged(true);
                return true;
            }
            OnGameStatusChanged(false);
            return false;
        }
        catch (Exception ex)
        {
            // TODO : log ex
            throw;
        }

    }

    #endregion

    #region LocalFunctions
    private InternalScoreModel? IsGameExists(Game game) => _internalScoreBoard.FirstOrDefault(match => match.IsLive &&
                                                           match.AwayTeam.TeamName.Equals(game.AwayTeam.TeamName, StringComparison.OrdinalIgnoreCase)
                                                          && match.HomeTeam.TeamName.Equals(game.HomeTeam.TeamName, StringComparison.OrdinalIgnoreCase));

    protected virtual void OnGameStatusChanged(bool isSuccessful)
    {
        OnGameStatusChangeProcessCompleted?.Invoke(this, isSuccessful);
    }

    protected virtual void ScoreUpdated(ScoreEventArgs args)
    {
        OnLiveScoreChangeProcessCompleted?.Invoke(this, args);
    }
    #endregion
}