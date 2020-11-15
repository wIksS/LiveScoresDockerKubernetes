using HtmlAgilityPack;
using LiveScores.Messages;
using MassTransit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LiveScoresCrawlerSource.Crawler
{
    public class LiveScoresCrawler
    {
        readonly IBus bus;

        public LiveScoresCrawler(IBus bus)
        {
            this.bus = bus;
        }

        public async Task Craw()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://www.livescores.com/");
            var games = document.DocumentNode.SelectNodes("//div")
                .Where(t => t.HasClass("ply")).ToList();
            foreach (var game in games)
            {
                try
                {
                    var team1 = game.InnerText;
                    var team2 = game.ParentNode.Descendants()?
                        .Where(t => t.HasClass("ply"))?.ToList()[1].InnerText;

                    var score = game.ParentNode.Descendants()
           .First(t => t.HasClass("sco"))?.SelectSingleNode("a")?.InnerText;

                    if (score != null && team2 != null)
                    {
                        var gameResult = new GameAddMessage
                        {
                            Score = score,
                            Team1 = team1,
                            Team2 = team2
                        };
                        await bus.Publish(gameResult);
                    }
                }
                catch (Exception e)
                { }
            }
        }
    }
}
