using LiveScore.Library.Abstracts;

namespace LiveScore.Library.Models;

public class Scores : IScore
{
    public int GameId { get; set; }
    public Game Score { get; set; }
    public bool IsLive { get; set; }
    public string? Message { get; set; }

    public override string ToString() => Score.ToString();
}