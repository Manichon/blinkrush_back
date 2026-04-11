using BlinkRush.Api.Contracts;
using BlinkRush.Data;
using Microsoft.EntityFrameworkCore;

namespace BlinkRush.Api;

public static class LeaderboardEndpoints
{
    public static RouteGroupBuilder MapLeaderboardApi(this RouteGroupBuilder api)
    {
        api.MapPost("/records", CreateRecordAsync).WithName("CreateRecord");
        api.MapGet("/leaderboard", GetLeaderboardAsync).WithName("GetLeaderboard");
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

        var record = new LeaderboardRecord
        {
            Id = Guid.NewGuid(),
            Mode = body.Mode,
            Value = body.Value,
            OccurredAt = body.OccurredAt ?? DateTimeOffset.UtcNow
        };
        db.LeaderboardRecords.Add(record);
        await db.SaveChangesAsync();

        var response = LeaderboardRecordMapper.ToResponse(record);
        return Results.Created($"/api/records/{record.Id}", response);
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
}
