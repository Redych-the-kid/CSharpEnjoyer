using CollisiumExperimentsWorkers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Strategies;

namespace ElonZuckGHost;

class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: program_name + mode[nodb, writedb, readdb]");
        }
        return Host.CreateDefaultBuilder(args).ConfigureServices((_, services) =>
        {
            switch (args[0])
            {
                case "writedb":
                    services.AddHostedService<CollisiumExperimentWorkerWriteDb>();
                    break;
                case "readdb":
                    services.AddHostedService<CollisiumExperimentWorkerReadDb>();
                    break;
                case "nodb":
                    services.AddHostedService<CollisiumExperimentWorker>();
                    break;
                default:
                    Console.WriteLine("No such mode exists! Exiting!");
                    return;
            }
            
            services.AddScoped<CollisiumSandbox>();
            services.AddScoped<IDeckShuffler, DeckShuffler>();
            services.AddScoped<ElonStrategy>();
            services.AddScoped<ZuckStrategy>();
        });
    }
}