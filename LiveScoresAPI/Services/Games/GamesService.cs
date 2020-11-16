using LiveScores.Messages;
using LiveScoresAPI.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveScoresAPI.Services.Games
{
    public class GamesService : IGamesService
    {
        private readonly GamesDbContext gamesDb;

        public GamesService(GamesDbContext gamesDb)
        {
            this.gamesDb = gamesDb;
        }
        public async Task AddGame(GameAddMessage game)
        {
            bool test = false;
            try
            {
                test = gamesDb.Games.AsEnumerable().Any(g => g.Team1 == game.Team1
 && g.Team2 == game.Team2
 && (DateTime.Now - g.CreatedOn).TotalMinutes < 120);
            }
            catch (Exception e)
            {


                throw e;
            }


            if (!test)
            {
                await gamesDb.Games.AddAsync(new Game()
                {
                    Team1 = game.Team1,
                    Team2 = game.Team2,
                    Score = game.Score,
                    CreatedOn = DateTime.Now,
                });

                gamesDb.SaveChanges();
            }
            else
            {
                int x = 5;
            }
        }
    }
}
