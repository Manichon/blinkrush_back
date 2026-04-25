namespace BlinkRush.Api.Contracts;

public sealed record CreateRecordRequest(
    string Mode,
    double Value,
    string DeviceId,
    string? Name,
    DateTimeOffset? OccurredAt);
