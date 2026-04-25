namespace BlinkRush.Api.Contracts;

public sealed record UserLeaderboardEntry(
    int Rank,
    string DeviceId,
    string? Name,
    double Value,
    DateTimeOffset OccurredAt);
