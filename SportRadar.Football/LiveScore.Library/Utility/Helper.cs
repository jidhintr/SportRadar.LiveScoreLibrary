namespace LiveScore.Library.Utility;

public static class Helper
{
    public static bool IsValidTeam(this string name) => !string.IsNullOrEmpty(name) && name.Length > 1;
    public static bool IsValidGoal(this int goal) => goal is >= 0 and < 100; // Minimum or start goal = 0, as per requirement
                                                                             // Max goal is assumed to be 99

}