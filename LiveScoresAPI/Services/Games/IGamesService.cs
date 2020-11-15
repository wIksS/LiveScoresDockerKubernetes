using LiveScores.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveScoresAPI.Services.Games
{
    public interface IGamesService
    {
         Task AddGame(GameAddMessage game);
    }
}
