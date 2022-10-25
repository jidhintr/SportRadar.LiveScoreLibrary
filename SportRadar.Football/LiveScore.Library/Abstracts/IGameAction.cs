using LiveScore.Library.Models;
namespace LiveScore.Library.Abstracts;

public interface IGameAction
{
    /// <summary>
    /// Start the game, it add the live score to the scoreboard
    /// </summary>
    /// <param name="game">Record of two teams with initial goal 0</param>
    /// <returns>Tuple with status and the object itself to get data based on the object value inserted to memory-collection</returns>
    Tuple<bool, Scores> StartGame(Game game);

    
    /// <summary>
    /// Update score of all live games 
    /// </summary>
    /// <param name="game">Expect the game 'record' with both team's absolute scores</param>
    /// <returns>Return Tuple of boolean with score</returns>
    Tuple<bool, Scores> UpdateScore(Game game); // by game Id as well ?? , client has overhead to save id 
    // Summary AllSummary(); // sort action ??
    // FinishGame(Game game);

}