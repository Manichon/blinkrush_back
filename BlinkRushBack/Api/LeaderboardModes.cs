namespace BlinkRush.Api;

public static class LeaderboardModes
{
    public const string SpeedRun = "speedRun";
    public const string Endurance = "endurance";

    public static bool IsValid(string? mode) =>
        mode is SpeedRun or Endurance;
}
