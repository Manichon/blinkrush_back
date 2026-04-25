using BlinkRush.Extensions;

namespace BlinkRush;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddBlinkRushPersistence();

        var app = builder.Build();

        app.ApplyDatabaseMigrations();
        app.MapBlinkRushApi();

        app.Run();
    }
}
