using LiveScore.Library.Abstracts;
using LiveScore.Library.Models;
using LiveScore.Library.Utility;
using System.Text;

namespace LiveScore.Library;

public sealed class GameController : IControlGame
{
    public event EventHandler<bool>? OnGameStatusChangeProcessCompleted;

    public GameController() => Globals.InternalScoreBoard = GameFactory.GetInternalScoreModelList();

    #region Contracts
    public (bool IsStarted, Scores Score) StartGame(Game game)
    {
        try
        {
            var result = GameFactory.GetScoreModel();
            if (!game.IsValidGame())
            {
                OnGameStatusChanged(false);
                return (false, result);
            }

            var startTime = TimeOnly.FromDateTime(DateTime.Now);
            var endTime = startTime.AddMinutes(90);
            var liveGames = Globals.InternalScoreBoard.Count;
            var gameId = liveGames + 1;

            // internal model for DB / data manipulation 
            var liveGame = GameFactory.GetInternalScoreModel();
            liveGame.GameId = gameId;
            liveGame.AwayTeam = game.AwayTeam;
            liveGame.HomeTeam = game.HomeTeam;
            liveGame.IsLive = true;
            liveGame.StartTime = startTime;
            liveGame.LastUpdatedOn = TimeOnly.FromDateTime(DateTime.Now);
            liveGame.Message = new StringBuilder(
                $"Game between {game.AwayTeam} and {game.HomeTeam} started at {startTime} and expected to finish by {endTime}");


            Globals.InternalScoreBoard.Add(liveGame);

            // TODO : Use automapper to map result => 

            result.GameId = gameId;
            result.IsLive = liveGame.IsLive;
            result.Game = game;
            result.Message = liveGame.Message.ToString();

            if (Globals.InternalScoreBoard.Count == gameId)
            {
                OnGameStatusChanged(true);
                return (true, result);
            }

            return (false, result);
        }
        catch (Exception)
        {
            // log ex
            throw;
        }
    }

    public bool FinishGame(Game game)
    {
        try
        {
            var playingGame = game.IsGameLive();
            // notify the summary model
            if (playingGame != null)
            {
                Globals.InternalScoreBoard.Remove(playingGame);
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

    private void OnGameStatusChanged(bool isSuccessful) => OnGameStatusChangeProcessCompleted?.Invoke(this, isSuccessful);
}