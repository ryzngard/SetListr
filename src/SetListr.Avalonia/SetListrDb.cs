using System.IO;

using Microsoft.EntityFrameworkCore;

using SetListr.Avalonia.Models;

namespace SetListr.Avalonia;

public class SetListrDb : DbContext
{
    public DbSet<Song> Songs { get; set; }
    public DbSet<Setlist> Setlists { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Artist> Artists { get; set; }

    public SetListrDb()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source=setlistr.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure many-to-many relationships
        modelBuilder.Entity<Setlist>()
            .HasMany(s => s.Songs)
            .WithMany();

        modelBuilder.Entity<Album>()
            .HasMany(a => a.Songs)
            .WithMany();

        modelBuilder.Entity<Artist>()
            .HasMany(a => a.Songs)
            .WithMany();

        modelBuilder.Entity<Artist>()
            .HasMany(a => a.Setlists)
            .WithMany();
    }
}