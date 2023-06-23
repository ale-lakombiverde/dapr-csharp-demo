using System;
using DaprActorSample.ActorsHost.Actors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DaprActorSample.ActorsHost;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddActors(actorRuntime =>
        {
            actorRuntime.ActorIdleTimeout = TimeSpan.FromMinutes(15);
            actorRuntime.ActorScanInterval = TimeSpan.FromSeconds(15);
            actorRuntime.DrainOngoingCallTimeout = TimeSpan.FromSeconds(30);
            actorRuntime.DrainRebalancedActors = true;
            actorRuntime.Actors.RegisterActor<DoorActor>();
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapActorsHandlers(); });
    }
}