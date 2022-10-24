using System.Text;
using LiveScore.Library.Models;

namespace LiveScore.Library.Abstracts;

public interface IInternalModel
{
    int GameId { get; set; }
    Team HomeTeam { get; set; }

    Team AwayTeam { get; set; }
    TimeOnly StartTime { get; set; }

    TimeOnly LastUpdatedOn { get; set; }

    bool IsLive { get; set; }
    StringBuilder? Message { get; set; }

}