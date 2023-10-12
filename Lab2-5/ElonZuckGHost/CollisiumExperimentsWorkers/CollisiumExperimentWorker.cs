using Microsoft.Extensions.Hosting;

namespace CollisiumExperimentsWorkers;

public class CollisiumExperimentWorker : IHostedService
{
    protected readonly CollisiumSandbox CollisiumSandbox;
    protected int SucessCount;
    protected const int ExperimentsCount = 100;

    public CollisiumExperimentWorker(CollisiumSandbox collisiumSandbox)
    {
        CollisiumSandbox = collisiumSandbox;
    }
    
    public virtual Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            try
            {
                int[] array = new int[36];
                Fill(array);
                CollisiumSandbox.AddCards(array);
                for (int i = 0; i < ExperimentsCount; ++i)
                {
                    if (CollisiumSandbox.RunSandboxIteration())
                    {
                        SucessCount++;
                    }
                }
                Console.WriteLine(Convert.ToDouble(SucessCount) / ExperimentsCount * 100);
                
                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }, cancellationToken);
        return Task.CompletedTask;
    }
    public static void Fill(int[] array){
        for(int i = 0; i < 18; ++i){
            array[i] = 0;
        }
        for(int i = 18; i < 36; ++i){
            array[i] = 1;
        }
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}