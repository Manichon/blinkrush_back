using BlinkRush.Extensions;

namespace BlinkRush;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.AddBlinkRushPersistence();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.ApplyDatabaseMigrations();
        app.MapBlinkRushApi();

        app.Run();
    }
}
