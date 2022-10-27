namespace LiveScore.Library.Models;

public readonly record struct Game(Team HomeTeam, Team AwayTeam)
{
    public override string ToString() => $"{HomeTeam} - {AwayTeam}";
}