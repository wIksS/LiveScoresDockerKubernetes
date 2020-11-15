using GraphQL;
using GraphQL.Types;
using LiveScoresAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveScoresAPI.GraphQL
{
    public class GameQuery : ObjectGraphType
    {
        public GameQuery(GamesDbContext db)
        {
            var queryArguments = new QueryArgument[]{new QueryArgument<IdGraphType> { Name = "id" },
                new QueryArgument<StringGraphType> { Name = "score" },
                new QueryArgument<StringGraphType> { Name = "team1" },
                new QueryArgument<StringGraphType> { Name = "team2" } };

            Field<GameType>(
              "Game",
              arguments: new QueryArguments(queryArguments),
              resolve: context =>
              {
                  Game game = db
              .Games
              .FirstOrDefault(GetGamesFilter(context));
                  return game;
              });

            Field<ListGraphType<GameType>>(
              "Games",
               arguments: new QueryArguments(queryArguments),
              resolve: context =>
              {
                  var authors = db.Games;
                  return authors.Where(GetGamesFilter(context));
              });
        }

        private Func<Game, bool> GetGamesFilter(IResolveFieldContext<object> context)
        {
            var id = context.GetArgument<int>("id");
            var score = context.GetArgument<string>("score");
            var team1 = context.GetArgument<string>("team1");
            var team2 = context.GetArgument<string>("team2");
            if (id == 0 && score == null && team1 == null && team2 == null)
                return i => true;

            return i => (team1 != null && i.Team1 == team1) ||
                                     (team2 != null && i.Team2 == team2) ||
                                     (score != null && i.Score == score) ||
                                     (id != 0 && i.Id == id);
        }
    }
}