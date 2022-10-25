namespace LiveScore.Library.Models;

public readonly record struct Team(string TeamName, int Goal)
{
    public override string ToString() => $"{TeamName} {Goal}";
}