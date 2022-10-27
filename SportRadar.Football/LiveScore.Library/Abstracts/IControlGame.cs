using LiveScore.Library.Models;

namespace LiveScore.Library.Abstracts;

public interface IControlGame
{
    /// <summary>
    ///     Start the game, it add the live score to the scoreboard
    /// </summary>
    /// <param name="game">Record of two teams with initial goal 0</param>
    /// <returns>Game status and the Score value inserted to memory-collection</returns>
    (bool IsStarted, Scores Score) StartGame(Game game);

    /// <summary>
    /// End the live game if exists
    /// </summary>
    /// <param name="game">Live game's details</param>
    /// <returns>Boolean, game stopped or not</returns>
    bool FinishGame(Game game);

    /// <summary>
    /// Finish game by passing the game-id 
    /// </summary>
    /// <param name="gameId">Same Id received as return type on StartGame function</param>
    /// <returns>Boolean, game stopped or not</returns>
    bool FinishGame(string gameId);

}