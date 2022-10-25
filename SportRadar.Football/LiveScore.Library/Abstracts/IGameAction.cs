﻿using LiveScore.Library.Models;

namespace LiveScore.Library.Abstracts;

public interface IGameAction
{
    /// <summary>
    ///     Start the game, it add the live score to the scoreboard
    /// </summary>
    /// <param name="game">Record of two teams with initial goal 0</param>
    /// <returns>Game status and the Score value inserted to memory-collection</returns>
    (bool IsStarted, Scores Score) StartGame(Game game);


    /// <summary>
    ///     Update score of all live games
    /// </summary>
    /// <param name="game">Expect the game 'record' with both team's absolute scores</param>
    /// <returns>Status of update operation and updated score</returns>
    (bool IsUpdated, Scores Score) UpdateScore(Game game); // by game Id as well ?? , client has overhead to save id 
    
    /// <summary>
    /// Get list of all live games 
    /// </summary>
    /// <returns>List of all games in order</returns>
    List<Game> AllSummary(); // sort action ??
    
    /// <summary>
    /// End the live game if exists
    /// </summary>
    /// <param name="game">Live game's details</param>
    /// <returns>Boolean, game stopped or not</returns>
    bool FinishGame(Game game);
}