namespace BlinkRush.Api.Contracts;

public sealed record CreateRecordRequest(string Mode, double Value, DateTimeOffset? OccurredAt);
