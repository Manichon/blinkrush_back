using BlinkRush.Api.Contracts;
using BlinkRush.Data;
using Microsoft.EntityFrameworkCore;

namespace BlinkRush.Api;

public static class LeaderboardEndpoints
{
    public static RouteGroupBuilder MapLeaderboardApi(this RouteGroupBuilder api)
    {
        api.MapPost("/records", CreateRecordAsync).WithName("CreateRecord");
        api.MapGet("/records/{deviceId}", GetRecordsByDeviceIdAsync).WithName("GetRecordsByDeviceId");
        api.MapGet("/leaderboard", GetLeaderboardAsync).WithName("GetLeaderboard");
        api.MapGet("/users/leaderboard", GetUserLeaderboardAsync).WithName("GetUserLeaderboard");
        return api;
    }

    private static async Task<IResult> CreateRecordAsync(
        CreateRecordRequest body,
        BlinkRushDbContext db)
    {
        if (!LeaderboardModes.IsValid(body.Mode))
            return Results.BadRequest(new { error = "mode must be speedRun or endurance" });

        if (body is { Mode: LeaderboardModes.SpeedRun, Value: <= 0 })
            return Results.BadRequest(new { error = "speedRun value must be a positive duration in seconds" });

        if (body.Mode == LeaderboardModes.Endurance && body.Value < 0)
            return Results.BadRequest(new { error = "endurance value must be non-negative" });

        if (string.IsNullOrWhiteSpace(body.DeviceId))
            return Results.BadRequest(new { error = "deviceId is required" });

        var name = string.IsNullOrWhiteSpace(body.Name) ? null : body.Name.Trim();

        var record = new LeaderboardRecord
        {
            Id = Guid.NewGuid(),
            DeviceId = body.DeviceId.Trim(),
            Name = name,
            Mode = body.Mode,
            Value = body.Value,
            OccurredAt = body.OccurredAt ?? DateTimeOffset.UtcNow
        };
        db.LeaderboardRecords.Add(record);
        await db.SaveChangesAsync();

        var response = LeaderboardRecordMapper.ToResponse(record);
        return Results.Created($"/api/records/{record.Id}", response);
    }

    private static async Task<IResult> GetRecordsByDeviceIdAsync(
        string deviceId,
        string? mode,
        int? take,
        BlinkRushDbContext db)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
            return Results.BadRequest(new { error = "deviceId is required" });

        if (!string.IsNullOrEmpty(mode) && !LeaderboardModes.IsValid(mode))
            return Results.BadRequest(new { error = "mode must be speedRun or endurance" });

        var limit = Math.Clamp(take ?? 100, 1, 500);
        var normalizedDeviceId = deviceId.Trim();

        IQueryable<LeaderboardRecord> query = db.LeaderboardRecords
            .Where(r => r.DeviceId == normalizedDeviceId);

        if (!string.IsNullOrEmpty(mode))
            query = query.Where(r => r.Mode == mode);

        var rows = (await query.ToListAsync())
            .OrderByDescending(r => r.OccurredAt)
            .Take(limit)
            .ToList();

        var list = rows.ConvertAll(LeaderboardRecordMapper.ToResponse);
        return Results.Ok(list);
    }

    private static async Task<IResult> GetLeaderboardAsync(
        string? mode,
        int? take,
        BlinkRushDbContext db)
    {
        if (string.IsNullOrEmpty(mode) || !LeaderboardModes.IsValid(mode))
            return Results.BadRequest(new { error = "mode must be speedRun or endurance" });

        var limit = Math.Clamp(take ?? 20, 1, 100);

        IQueryable<LeaderboardRecord> query = db.LeaderboardRecords.Where(r => r.Mode == mode);

        query = mode == LeaderboardModes.SpeedRun
            ? query.OrderBy(r => r.Value)
            : query.OrderByDescending(r => r.Value);

        var rows = await query.Take(limit).ToListAsync();
        var list = rows.ConvertAll(LeaderboardRecordMapper.ToResponse);

        return Results.Ok(list);
    }

    private static async Task<IResult> GetUserLeaderboardAsync(
        string? mode,
        int? take,
        BlinkRushDbContext db)
    {
        if (string.IsNullOrEmpty(mode) || !LeaderboardModes.IsValid(mode))
            return Results.BadRequest(new { error = "mode must be speedRun or endurance" });

        var limit = Math.Clamp(take ?? 20, 1, 100);

        IQueryable<LeaderboardRecord> query = db.LeaderboardRecords.Where(r => r.Mode == mode);

        query = mode == LeaderboardModes.SpeedRun
            ? query.OrderBy(r => r.Value)
            : query.OrderByDescending(r => r.Value);

        var rows = await query.ToListAsync();

        var bestPerDevice = rows
            .GroupBy(r => r.DeviceId)
            .Select(g => g.First())
            .Take(limit)
            .Select((r, i) => new UserLeaderboardEntry(
                i + 1,
                r.DeviceId,
                r.Name,
                r.Value,
                r.OccurredAt))
            .ToList();

        return Results.Ok(bestPerDevice);
    }
}
