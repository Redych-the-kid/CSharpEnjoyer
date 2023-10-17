namespace Contracts;

public record NumberMessage()
{
    public int Number { get; init; }
    public string? Signature { get; init;}
}