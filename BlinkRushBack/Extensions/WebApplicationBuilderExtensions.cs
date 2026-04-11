using BlinkRush.Data;
using BlinkRush.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BlinkRush.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddBlinkRushPersistence(this WebApplicationBuilder builder)
    {
        var sqliteConnection = SqlitePathHelper.ResolveConnectionString(
            builder.Configuration,
            builder.Environment.ContentRootPath);

        builder.Services.AddDbContext<BlinkRushDbContext>(options =>
            options.UseSqlite(sqliteConnection));

        return builder;
    }
}
