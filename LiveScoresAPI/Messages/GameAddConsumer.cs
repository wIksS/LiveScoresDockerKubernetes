using LiveScores.Messages;
using LiveScoresAPI.Services.Games;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveScoresAPI.Messages
{
    public class GameAddConsumer : IConsumer<GameAddMessage>
    {
        private readonly IGamesService gamesService;

    public GameAddConsumer(IGamesService gamesService)
        => this.gamesService = gamesService;

    public async Task Consume(ConsumeContext<GameAddMessage> context)
        => await this.gamesService.AddGame(context.Message);
}
}
