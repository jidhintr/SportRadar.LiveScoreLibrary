using LiveScore.Library.Models;

namespace LiveScore.Library.Utility;

public static class Helper
{
    private static bool IsValidTeam(this string name) => !string.IsNullOrEmpty(name) && name.Length > 1;

    private static bool IsValidGoal(this int goal) => goal is >= 0 and < 100; // Minimum or start goal = 0, as per requirement
                                                                             // Max goal is assumed to be 99
    public static bool IsValid(this Game game) => game.AwayTeam.TeamName.IsValidTeam() && game.AwayTeam.Goal.IsValidGoal();

}