using Microsoft.EntityFrameworkCore;

namespace BlinkRush.Data;

public sealed class BlinkRushDbContext : DbContext
{
    public BlinkRushDbContext(DbContextOptions<BlinkRushDbContext> options)
        : base(options)
    {
    }

    public DbSet<LeaderboardRecord> LeaderboardRecords => Set<LeaderboardRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<LeaderboardRecord>();
        e.HasKey(x => x.Id);
        e.Property(x => x.DeviceId).HasMaxLength(128).IsRequired();
        e.Property(x => x.Name).HasMaxLength(64);
        e.Property(x => x.Mode).HasMaxLength(32).IsRequired();
        e.HasIndex(x => x.Mode);
        e.HasIndex(x => new { x.Mode, x.DeviceId });
    }
}
