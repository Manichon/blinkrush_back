namespace BlinkRush.Api.Contracts;

public sealed record RecordResponse(Guid Id, string Mode, double Value, DateTimeOffset OccurredAt);
