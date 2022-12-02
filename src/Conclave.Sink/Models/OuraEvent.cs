using System.Text.Json.Serialization;
using Conclave.Sink.Extensions;

namespace Conclave.Sink.Models;

public record OuraEvent : IOuraEvent
{
    public OuraContext? Context { get; init; }
    public string? Fingerprint { get; init; }

    [JsonConverter(typeof(OuraVariantJsonConverter))]
    public OuraVariant? Variant { get; init; }

    public ulong? Timestamp { get; init; }
}