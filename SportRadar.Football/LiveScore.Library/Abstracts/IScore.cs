using LiveScore.Library.Models;

namespace LiveScore.Library.Abstracts;

public interface IScore
{
    int GameId { get; set; }
    Game Game { get; set; }
    bool IsLive { get; set; }
    string? Message { get; set; }

    virtual string ToString() => Game.ToString();
    
}