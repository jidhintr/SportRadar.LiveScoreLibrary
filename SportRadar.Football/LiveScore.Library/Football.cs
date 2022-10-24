using LiveScore.Library.Abstracts;
using LiveScore.Library.Models;

namespace LiveScore.Library
{
    public class Football : IGameAction
    {

        public bool StartGame(Game game)
        {
            return true;
        }
    }
}