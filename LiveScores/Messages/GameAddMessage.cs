using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveScores.Messages
{
    public class GameAddMessage
    {
        public string Score { get; set; }

        public string Team1 { get; set; }

        public string Team2 { get; set; }
    }
}
