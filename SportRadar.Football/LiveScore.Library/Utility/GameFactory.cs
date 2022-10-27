using LiveScore.Library.Models;

namespace LiveScore.Library.Utility;

internal static class GameFactory
{
    internal static List<InternalScoreModel> GetInternalScoreModelList() => new List<InternalScoreModel>();
    internal static InternalScoreModel GetInternalScoreModel() => new InternalScoreModel();
    internal static Scores GetScoreModel() => new Scores();
}