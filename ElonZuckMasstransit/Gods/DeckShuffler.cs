namespace Gods;

public class DeckShuffler : IDeckShuffler
{
    public int[] Shuffle(int[] cards)
    {
        return cards.OrderBy(_ => Guid.NewGuid()).ToArray();
    }
}