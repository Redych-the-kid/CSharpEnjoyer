namespace Strategies;

public class TestStrategy : IStrategy
{
    public int Decide()
    {
        var x = 0;
        foreach (var card in Cards)
        {
            if (card == 1)
            {
                return x;
            }

            x++;
        }

        return x;
    }

    public int[] Cards { get; set; }
}