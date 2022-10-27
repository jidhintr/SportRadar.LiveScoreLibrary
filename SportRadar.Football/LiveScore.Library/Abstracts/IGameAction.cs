using LiveScore.Library.Models;

namespace LiveScore.Library.Abstracts;

public interface IGameAction
{
    /// <summary>
    ///     Update score of all live games
    /// </summary>
    /// <param name="game">Expect the game 'record' with both team's absolute scores</param>
    /// <returns>Status of update operation and updated score</returns>
    (bool IsUpdated, Scores Score) UpdateScore(Game game); // by game Id as well ?? , client has overhead to save id 

    /// <summary>
    /// Get list of all live games 
    /// </summary>
    /// <returns>IEnumerable of all games in order</returns>
    IEnumerable< Game> AllSummary(); // sort action ??
    

}