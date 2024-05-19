namespace Olimpo.Domain;

public class Result
{
    public string Message { get; set; } = "The error was not especified";
    public long Latency { get; set; } = -1;

    public long Value { get; set; } = -1;
}
