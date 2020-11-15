using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MemoryStorage;
using LiveScoresCrawlerSource.Crawler;
using LiveScoresCrawlerSource.HangfireConfig;
using MassTransit;
using MassTransit.Definition;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using LiveScores.Infrastructure;

namespace LiveScoresCrawlerSource
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
       => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(2);
                options.Predicate = (check) => check.Tags.Contains("ready");
            });


            services.AddHangfire(config =>
            {
                config.UseMemoryStorage();
            });

            services.AddHangfireServer();
            services.AddMessaging(this.Configuration);
            //var appSettingsSection = section.GetSection("AppSettings");
            //var appSettings = appSettingsSection.Get<Appsettings>();

            //services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            //services.AddMassTransit(config =>
            //{
            //    config.AddConsumer<SubmitTokenConsumer>();
            //    config.AddBus(context => Bus.Factory.CreateUsingRabbitMq(c =>
            //    {
            //        c.UseHealthCheck(context);
            //        c.Host("rabbitmq://localhost");
            //        c.ConfigureEndpoints(context, KebabCaseEndpointNameFormatter.Instance);
            //    }));
            //});

            //services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobs, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            GlobalConfiguration.Configuration
    .UseActivator(new HangfireActivator(serviceProvider));

           // backgroundJobs.Enqueue<LiveScoresCrawler>(c => c.Craw()); 
            RecurringJob.AddOrUpdate<LiveScoresCrawler>("craw", c => c.Craw(), Cron.Minutely);
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("ready"),
                });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions());
            });
        }
    }
}
