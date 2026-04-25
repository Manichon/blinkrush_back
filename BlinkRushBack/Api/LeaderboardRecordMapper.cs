using BlinkRush.Api.Contracts;
using BlinkRush.Data;

namespace BlinkRush.Api;

public static class LeaderboardRecordMapper
{
    public static RecordResponse ToResponse(LeaderboardRecord record) =>
        new(
            record.Id,
            record.DeviceId,
            record.Name,
            record.Mode,
            record.Value,
            record.OccurredAt);
}
