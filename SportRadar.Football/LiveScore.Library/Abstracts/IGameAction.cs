using LiveScore.Library.Models;

namespace LiveScore.Library.Abstracts;

public interface IGameAction
{
    /// <summary>
    /// Start the game, it add the live score to the scoreboard
    /// </summary>
    /// <param name="game">Record of two teams with initial goal 0</param>
    /// <returns>Boolean, true for success and false for any failure</returns>
    bool StartGame(Game game);

    
    
    
    // bool UpdateScore(Game game); // by game Id as well ?? , client has overhead to save id 
    // Summary AllSummary(); // sort action ??
    // FinishGame(Game game);

}