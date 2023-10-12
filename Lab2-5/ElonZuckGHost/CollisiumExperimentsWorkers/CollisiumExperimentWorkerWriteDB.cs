namespace CollisiumExperimentsWorkers;

public class CollisiumExperimentWorkerWriteDb : CollisiumExperimentWorker
{
    public CollisiumExperimentWorkerWriteDb(CollisiumSandbox collisiumSandbox) : base(collisiumSandbox)
    {
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            try
            {
                int[] array = new int[36];
                Fill(array);
                CollisiumSandbox.AddCards(array);
                using (ApplicationContext db = new ApplicationContext())
                {
                    for (int i = 0; i < ExperimentsCount; ++i)
                    {
                        Experiment experiment = new Experiment();
                        if (CollisiumSandbox.RunSandboxIteration(experiment))
                        {
                            SucessCount++;
                            experiment.Success = true;
                        }
                        else
                        {
                            experiment.Success = false;
                        }

                        db.Experiments.Add(experiment);
                        await db.SaveChangesAsync(cancellationToken);
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
}