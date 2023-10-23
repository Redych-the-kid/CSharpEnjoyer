namespace Contracts;

public record ReadyMessage()
{
    public bool Ready { get; init; }
}