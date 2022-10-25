using System.Text;
using LiveScore.Library.Abstracts;

namespace LiveScore.Library.Models;

internal class InternalScoreModel : IInternalModel
{
    public int GameId { get; set; }
    public Team HomeTeam { get; set; }
    public Team AwayTeam { get; set; }
    public TimeOnly StartTime { get; set; } // just focus only time, realistically  on 90 minutes match , not with dates
    public TimeOnly LastUpdatedOn { get; set; }
    public bool IsLive { get; set; }
    public StringBuilder? Message { get; set; }
}