using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using LiveScore.Library.Abstracts;
using LiveScore.Library.Models;
using System.Text;
using LiveScore.Library.Utility;

namespace LiveScore.Library
{
    public class Football : IGameAction
    {
        #region Props

        private readonly List<InternalScoreModel> _internalScoreBoard;

        #endregion

        public Football()
        {
            _internalScoreBoard = new List<InternalScoreModel>();
        }

        #region Functions

        public Tuple<bool, Scores> StartGame(Game game)
        {
            if (!game.IsValid()) return new Tuple<bool, Scores>(false, null);
            var startTime = TimeOnly.FromDateTime(DateTime.Now);
            var endTime = startTime.AddMinutes(90);
            var liveGames = _internalScoreBoard.Count;
            var gameId = liveGames + 1;

            // internal model for DB / data manipulation 
            var score = new InternalScoreModel()
            {
                GameId = gameId,
                AwayTeam = game.AwayTeam,
                HomeTeam = game.HomeTeam,
                IsLive = true,
                StartTime = startTime,
                LastUpdatedOn = TimeOnly.FromDateTime(DateTime.Now),
                Message = new StringBuilder($"Game between {game.AwayTeam} and {game.HomeTeam} started at {startTime} and expected to finish by {endTime}")
            };
            _internalScoreBoard.Add(score);

            // TODO : Use automapper to map result => 
            var result = new Scores()
            {
                GameId = gameId,
                IsLive = score.IsLive,
                Score = game,
                Message = score.Message.ToString()
            };

            return _internalScoreBoard.Count == gameId ? (new Tuple<bool, Scores>(true, result)) : (new Tuple<bool, Scores>(false, null));
        }

        public Tuple<bool, Scores> UpdateScore(Game game)
        {
            var playingTeam = _internalScoreBoard.FirstOrDefault(match => match.IsLive &&
                            match.AwayTeam.TeamName.Equals(game.AwayTeam.TeamName, StringComparison.OrdinalIgnoreCase) &&
                            match.HomeTeam.TeamName.Equals(game.HomeTeam.TeamName, StringComparison.OrdinalIgnoreCase));

            if (playingTeam == null) return new(false, new Scores());
            playingTeam.AwayTeam = game.AwayTeam;
            playingTeam.HomeTeam = game.HomeTeam;
            playingTeam.Message?.Append(Environment.NewLine).Append($"Goal scored at {DateTime.Now}");

            var updatedScore = new Scores()
            {
                GameId = playingTeam.GameId,
                IsLive = playingTeam.IsLive,
                Score = new Game(playingTeam.HomeTeam, playingTeam.AwayTeam),
                Message = playingTeam.Message?.ToString()
            };


            return new(true, updatedScore);
        }
        
        #endregion
    }
}