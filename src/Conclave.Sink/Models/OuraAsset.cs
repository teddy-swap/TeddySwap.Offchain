using System.Text.Json.Serialization;

namespace Conclave.Sink.Models;

public class OuraAsset
{
    public string? Policy { get; init; }
    public string? Asset { get; init; }
    public ulong? Amount { get; init; }
}