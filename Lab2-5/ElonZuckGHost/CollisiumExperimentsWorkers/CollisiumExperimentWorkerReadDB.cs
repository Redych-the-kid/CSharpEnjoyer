using System.Diagnostics;

namespace CollisiumExperimentsWorkers;

public class CollisiumExperimentWorkerReadDb : CollisiumExperimentWorker
{
    public CollisiumExperimentWorkerReadDb(CollisiumSandbox collisiumSandbox) : base(collisiumSandbox)
    {
        
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    var experiments = db.Experiments.ToList();
                    foreach (var ex in experiments)
                    {
                        var cards = ex.Cards?.Split(' ').Select(n => Convert.ToInt32(n)).ToArray();
                        Debug.Assert(cards != null, nameof(cards) + " != null");
                        CollisiumSandbox.AddCards(cards);
                        if (CollisiumSandbox.RunControllableSandboxIteration())
                        {
                            Console.WriteLine($"{ex.Id} Sucess!");
                        }
                    }
                }
                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }, cancellationToken);
        return Task.CompletedTask;
    }
}