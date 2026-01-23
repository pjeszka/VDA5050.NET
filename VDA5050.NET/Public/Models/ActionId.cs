namespace VDA5050.NET.Public.Models;

public sealed record ActionId
{
    public ActionId(string value)
    {
        Value = value;
    }

    public static ActionId New() => new(Guid.NewGuid().ToString());
    public string Value { get; }
}