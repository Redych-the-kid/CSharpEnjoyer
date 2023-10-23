using System.Reflection;
using BetterGods.Consumers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BetterGods;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder (string[] args)
    {
        return Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.SetInMemorySagaRepositoryProvider();
                var entryAssembly = Assembly.GetEntryAssembly();
                x.AddConsumers(entryAssembly);
                x.UsingRabbitMq((_, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ReceiveEndpoint("new_queue_3", ep =>
                    {
                        ep.Consumer<ReadyMessageConsumer>();
                    });
                });
            });
            services.AddHostedService<ExperimentsSender>();
        });
    }
}