using System;

namespace LiveScoresAPI.Data
{
    public class Game
    {
        public int Id { get; set; }

        public string Score { get; set; }

        public string Team1 { get; set; }

        public string Team2 { get; set; }

        public DateTime CreatedOn{ get; set; }
    }
}
