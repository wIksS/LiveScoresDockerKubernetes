using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LiveScores.Infrastructure;
using LiveScoresAPI.Services.Games;
using LiveScoresAPI.Messages;
using LiveScoresAPI.Data;
using Microsoft.EntityFrameworkCore;
using GraphiQl;
using GraphQL.Server.Ui.Playground;

namespace LiveScoresAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
       => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<DbContext, GamesDbContext>()
                .AddDbContext<GamesDbContext>(options => options

                    .UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection"),
                        sqlOptions => sqlOptions
                            .EnableRetryOnFailure(
                                maxRetryCount: 10,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null)));

            services.AddTransient<IGamesService, GamesService>();

            services.AddMessaging(this.Configuration, consumers: typeof(GameAddConsumer));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();
            app.UseGraphiQl("/graphql");
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
