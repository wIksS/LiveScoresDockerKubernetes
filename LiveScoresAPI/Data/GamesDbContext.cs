namespace LiveScoresAPI.Data
{
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;

    public class GamesDbContext : DbContext
    {
        public GamesDbContext(DbContextOptions<GamesDbContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Game> Games { get; set; }
    }
}
