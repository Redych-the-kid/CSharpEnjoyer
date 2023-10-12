using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollisiumExperimentsWorkers;
using Microsoft.EntityFrameworkCore;

public class Experiment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Cards { get; set; }
    public bool Success { get; set; }
}

public sealed class ApplicationContext : DbContext
{
    private string _datasource = null!;
    public DbSet<Experiment> Experiments { get; set; } = null!;

    public ApplicationContext()
    {
        Database.EnsureCreated();
    }

    public ApplicationContext(String datasource)
    {
        _datasource = datasource;
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_datasource != null) 
        {
            optionsBuilder.UseSqlite($"Data Source=file:{_datasource}?cache=shared");
        }
        else
        {
            optionsBuilder.UseSqlite("Data Source=/home/opezdal/RiderProjects/ElonZuckGHost/test.db");
        }
        
    }
}