using Microsoft.EntityFrameworkCore;
using Signa.Api.Entities;

namespace Signa.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // =========================
    // Tables
    // =========================
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<DeviceHeartbeat> DeviceHeartbeats => Set<DeviceHeartbeat>();
    public DbSet<Media> Media => Set<Media>();
    public DbSet<Playlist> Playlists => Set<Playlist>();
    public DbSet<PlaylistMedia> PlaylistMedia => Set<PlaylistMedia>();
    public DbSet<DevicePlaylist> DevicePlaylists => Set<DevicePlaylist>();
    public DbSet<User> Users => Set<User>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ========== DEVICES ==========
        modelBuilder.Entity<Device>(entity =>
        {
            entity.ToTable("devices");

            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SerialNumber).IsUnique();

            entity.Property(e => e.Status).HasDefaultValue("offline");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasMany(e => e.Heartbeats)
                  .WithOne(h => h.Device)
                  .HasForeignKey(h => h.DeviceId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.DevicePlaylists)
                  .WithOne(dp => dp.Device)
                  .HasForeignKey(dp => dp.DeviceId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ========== DEVICE HEARTBEATS ==========
        modelBuilder.Entity<DeviceHeartbeat>(entity =>
        {
            entity.ToTable("device_heartbeats");
            entity.HasKey(e => e.Id);
        });

        // ========== MEDIA ==========
        modelBuilder.Entity<Media>(entity =>
        {
            entity.ToTable("media");
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.User)
                  .WithMany(u => u.Media)
                  .HasForeignKey(e => e.UploadedBy)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // ========== PLAYLISTS ==========
        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.ToTable("playlists");
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.User)
                  .WithMany(u => u.Playlists)
                  .HasForeignKey(e => e.CreatedBy)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(e => e.DevicePlaylists)
                  .WithOne(dp => dp.Playlist)
                  .HasForeignKey(dp => dp.PlaylistId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ========== PLAYLIST_MEDIA ==========
        modelBuilder.Entity<PlaylistMedia>(entity =>
        {
            entity.ToTable("playlist_media");
            entity.HasKey(e => e.Id);

            entity.HasOne(pm => pm.Playlist)
                  .WithMany(p => p.PlaylistMedia)
                  .HasForeignKey(pm => pm.PlaylistId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pm => pm.Media)
                  .WithMany(m => m.PlaylistMedia)
                  .HasForeignKey(pm => pm.MediaId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ========== DEVICE_PLAYLISTS ==========
        modelBuilder.Entity<DevicePlaylist>(entity =>
        {
            entity.ToTable("device_playlists");
            entity.HasKey(e => e.Id);

            entity.HasOne(dp => dp.Device)
                  .WithMany(d => d.DevicePlaylists)
                  .HasForeignKey(dp => dp.DeviceId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(dp => dp.Playlist)
                  .WithMany(p => p.DevicePlaylists)
                  .HasForeignKey(dp => dp.PlaylistId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ========== USERS ==========
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.IsActive).HasDefaultValue(false);
        });
        
        // ========== REFRESH TOKENS ==========
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_tokens");
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Token).IsUnique();

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
