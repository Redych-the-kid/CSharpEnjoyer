namespace Strategies;

public class ElonStrategy : IStrategy
{
    public int Decide()
    {
        int redCount = 0;
        int blackCount = 0;
        int redPos = 0;
        int blackPos = 0;
        for(int i = 0; i < Cards.Length;++i){
            if(Cards[i] == 1){
                redCount++;
                redPos = i;
            } else{
                blackCount++;
                blackPos = i;
            }
        }
        if(blackCount > redCount){
            return blackPos;
        }
        else{
            return redPos;
        }
    }

    public int[] Cards { private get; set; } = null!;
}