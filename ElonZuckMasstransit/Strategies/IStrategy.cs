namespace Strategies;

public interface IStrategy
{
    int Decide();
    int[] Cards { set; }
}