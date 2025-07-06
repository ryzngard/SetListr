using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SetListr.Data;

internal class SetListrDb(DbContextOptions options) : DbContext(options), ISetListrDb
{
    public DbSet<Song> Songs { get; set; }
    public DbSet<Setlist> Setlists { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Artist> Artists { get; set; }

    IQueryable<Song> ISetListrDb.Songs => Songs;

    IQueryable<Setlist> ISetListrDb.Setlists => Setlists.Include(s => s.Songs);

    IQueryable<Album> ISetListrDb.Albums => Albums.Include(a => a.Songs);

    IQueryable<Artist> ISetListrDb.Artists => Artists.Include(a => a.Songs).Include(a => a.Setlists);

    public Task<bool> SaveAsync(Song song) => SaveAsync(song, song.Id, Songs);
    public Task<bool> RemoveAsync(Song song) => RemoveAsync(song.Id, Songs);
    public Task<bool> SaveAsync(Setlist setlist) => SaveAsync(setlist, setlist.Id, Setlists);
    public Task<bool> RemoveAsync(Setlist setlist) => RemoveAsync(setlist.Id, Setlists);
    public Task<bool> SaveAsync(Album album) => SaveAsync(album, album.Id, Albums);
    public Task<bool> RemoveAsync(Album album) => RemoveAsync(album.Id, Albums);
    public Task<bool> SaveAsync(Artist artist) => SaveAsync(artist, artist.Id, Artists);
    public Task<bool> RemoveAsync(Artist artist) => RemoveAsync(artist.Id, Artists);

    public void ClearChanges() => ChangeTracker.Clear();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Song>()
            .Property(s => s.Duration)
            .HasConversion(
                v => v.Ticks,
                v => TimeSpan.FromTicks(v));

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

    private async Task<bool> SaveAsync<T>(T entity, int id, DbSet<T> currentSet)
        where T : class
    {
        var entry = ChangeTracker
            .Entries<T>()
            .FirstOrDefault(e => e.CurrentValues["Id"] is int eId && eId == id);

        if (entry is null)
        {
            var existingAlbum = await currentSet.FindAsync(id);
            if (existingAlbum != null)
            {
                entry = Entry(existingAlbum);
                entry.CurrentValues.SetValues(entity);
            }
            else
            {
                entry = currentSet.Add(entity);
            }
        }

        if (entry is null)
        {
            return false;
        }

        var otherEntries = ChangeTracker.Entries()
            .Where(e => e.Entity != entry)
            .Where(e => e.State is not EntityState.Detached or EntityState.Unchanged);

        try
        {
            ChangeTracker.Clear();
            Attach(entry);
            var changes = await SaveChangesAsync();
            return changes > 0;
        }
        finally
        {
            foreach (var otherEntry in otherEntries)
            {
                Attach(otherEntry.Entity);
            }
        }
    }

    private async Task<bool> RemoveAsync<T>(int id, DbSet<T> currentSet)
        where T : class
    {
        var entry = ChangeTracker
            .Entries<T>()
            .FirstOrDefault(e => e.CurrentValues["Id"] is int eId && eId == id);
        if (entry is null)
        {
            return false;
        }

        if (entry.State == EntityState.Detached)
        {
            currentSet.Remove(entry.Entity);
        }
        else
        {
            entry.State = EntityState.Deleted;
        }

        var otherEntries = ChangeTracker.Entries()
            .Where(e => e.Entity != entry)
            .Where(e => e.State is not EntityState.Detached);

        try
        {
            ChangeTracker.Clear();
            Attach(entry);
            var changes = await SaveChangesAsync();
            return changes > 0;
        }
        finally
        {
            foreach (var otherEntry in otherEntries)
            {
                Attach(otherEntry.Entity);
            }
        }
    }
}