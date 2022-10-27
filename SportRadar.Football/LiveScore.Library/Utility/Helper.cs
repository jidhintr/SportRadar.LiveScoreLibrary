using LiveScore.Library.Models;
using System.Security.Cryptography;
using System.Text;

namespace LiveScore.Library.Utility;

public static class Helper
{
    private static bool IsValidTeam(this string name) => !string.IsNullOrEmpty(name) && name.Length > 1;

    private static bool IsValidGoal(this int goal) => goal is >= 0 and < 100; // Minimum or start goal = 0, as per requirement
                                                                              // Max goal is assumed to be 99
    public static bool IsValidGame(this Game game) => game.AwayTeam.TeamName.IsValidTeam() && game.AwayTeam.Goal.IsValidGoal();

    internal static InternalScoreModel? IsGameLive(this Game game)
    {
        return Globals.InternalScoreBoard?.FirstOrDefault(match => match.IsLive &&
                                                                  match.AwayTeam.TeamName.Equals(game.AwayTeam.TeamName,
                                                                      StringComparison.OrdinalIgnoreCase)
                                                                  && match.HomeTeam.TeamName.Equals(
                                                                      game.HomeTeam.TeamName,
                                                                      StringComparison.OrdinalIgnoreCase));
    }

    internal static bool IsGameLive(string hash)
    {
        return Globals.InternalScoreBoard != null && Globals.InternalScoreBoard.Any(a => a.GameHash == hash 
        && a.IsLive);
    }
    

    public static string ComputeHash(this string content)
    {
        var contentBytes = Encoding.ASCII.GetBytes(content.ToLower());
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(contentBytes);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}