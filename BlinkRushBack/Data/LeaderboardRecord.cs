namespace BlinkRush.Data;

/// <summary>
/// Persisted score: Speed Run stores time in seconds; Endurance stores blink count.
/// Mode values match the iOS <c>LeaderboardMode</c> raw strings: speedRun, endurance.
/// </summary>
public sealed class LeaderboardRecord
{
    public Guid Id { get; set; }

    /// <summary>speedRun | endurance</summary>
    public string Mode { get; set; } = "";

    public double Value { get; set; }

    public DateTimeOffset OccurredAt { get; set; }
}
