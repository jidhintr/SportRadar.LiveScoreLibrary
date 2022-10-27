using LiveScore.Library.Models;

namespace LiveScore.Library.Events
{
    public class ScoreEventArgs : EventArgs
    {
        public ScoreEventArgs(Scores? updatedScore) => UpdatedScore = updatedScore;
        public  Scores? UpdatedScore { get; }
    }

}
