using BlinkRush.Api;
using BlinkRush.Data;
using Microsoft.EntityFrameworkCore;

namespace BlinkRush.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ApplyDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BlinkRushDbContext>();
        db.Database.Migrate();
        return app;
    }

    public static WebApplication MapBlinkRushApi(this WebApplication app)
    {
        app.MapGroup("/api").MapLeaderboardApi();
        return app;
    }
}
