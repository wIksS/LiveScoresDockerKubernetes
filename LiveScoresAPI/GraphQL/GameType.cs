using GraphQL.Types;
using LiveScoresAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveScoresAPI.GraphQL
{
    public class GameType : ObjectGraphType<Game>
    {
        public GameType()
        {
            Name = "Game";

            Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the Game.");
            Field(x => x.Score).Description("The score of the Game");
            Field(x => x.Team1).Description("The Team1 of the Game");
            Field(x => x.Team2).Description("The Team2 of the Game");
            Field(x => x.CreatedOn).Description("The Date of the Game");
        }
    }
}
