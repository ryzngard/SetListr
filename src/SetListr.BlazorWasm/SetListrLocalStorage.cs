
using Blazored.LocalStorage;

using SetListr.Data;

namespace SetListr.BlazorWasm;

public class SetListrLocalStorage(ILocalStorageService localStorage, ISyncLocalStorageService syncLocalStorageService) : ISetListrDb
{
    private readonly ILocalStorageService _localStorage = localStorage;
    private readonly ISyncLocalStorageService _syncLocalStorageService = syncLocalStorageService;

    private StoredSong[] StoredSongs
    {
        get => _syncLocalStorageService.GetItem<StoredSong[]>("songs") ?? [];
        set => _syncLocalStorageService.SetItem("songs", value);
    }

    private StoredSetlist[] StoredSetlists
    {
        get => _syncLocalStorageService.GetItem<StoredSetlist[]>("setlists") ?? [];
        set => _syncLocalStorageService.SetItem("setlists", value);
    }

    public IQueryable<Song> Songs => StoredSongs
        .Select(static song => song.ToDataSong())
        .AsQueryable();

    public IQueryable<Setlist> Setlists => StoredSetlists
        .Select(setlist => setlist.ToDataSetlist(_syncLocalStorageService))
        .AsQueryable();

    public IQueryable<Album> Albums => throw new NotImplementedException();

    public IQueryable<Artist> Artists => throw new NotImplementedException();

    public void ClearChanges()
    {
    }

    public Task<bool> RemoveAsync(Song song)
    {
        var currentSongs = StoredSongs;
        var storedSong = currentSongs.FirstOrDefault(s => s.Id == song.Id);
        if (storedSong is null)
        {
            return Task.FromResult(false);
        }

        currentSongs = currentSongs.Except([storedSong]).ToArray();

        StoredSongs = currentSongs;
        return Task.FromResult(true);
    }

    public Task<bool> RemoveAsync(Setlist setlist)
    {
        var currentSetlists = StoredSetlists;
        var storedSetlist = currentSetlists.FirstOrDefault(s => s.Id == setlist.Id);
        if (storedSetlist is null)
        {
            return Task.FromResult(false);
        }

        currentSetlists = currentSetlists.Except([storedSetlist]).ToArray();

        StoredSetlists = currentSetlists;
        return Task.FromResult(true);
    }

    public Task<bool> RemoveAsync(Album album)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAsync(Artist artist)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveAsync(Song song)
    {
        var currentSongs = StoredSongs;
        var storedSong = currentSongs.FirstOrDefault(s => s.Id == song.Id);
        if (storedSong is null)
        {
            StoredSongs = [..currentSongs, new StoredSong
                {
                    Id = GenerateSongId(currentSongs),
                    Name = song.Name,
                    Duration = song.Duration
                }];
            return Task.FromResult(true);
        }


        storedSong.Name = song.Name;
        storedSong.Duration = song.Duration;

        StoredSongs = currentSongs;
        return Task.FromResult(true);
    }

    public Task<bool> SaveAsync(Setlist setlist)
    {
        var currentSetlists = StoredSetlists;
        var storedSetlist = currentSetlists.FirstOrDefault(s => s.Id == setlist.Id);
        if (storedSetlist is null)
        {
            StoredSetlists = [..currentSetlists, new StoredSetlist
                {
                    Id = GenerateSetlistId(currentSetlists),
                    Name = setlist.Name,
                    SongIds = setlist.Songs.Select(s => s.Id).ToList()
                }];
            return Task.FromResult(true);
        }

        storedSetlist.Name = setlist.Name;
        storedSetlist.SongIds = [.. setlist.Songs.Select(s => s.Id)];

        StoredSetlists = currentSetlists;
        return Task.FromResult(true);
    }

    public Task<bool> SaveAsync(Album album)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveAsync(Artist artist)
    {
        throw new NotImplementedException();
    }

    private int GenerateSongId(StoredSong[]? songs)
    {
        songs ??= StoredSongs;
        var currentId = songs.OrderBy(s => s.Id).LastOrDefault()?.Id ?? 0;
        return currentId + 1;
    }

    private int GenerateSetlistId(StoredSetlist[]? setlists)
    {
        setlists ??= StoredSetlists;
        var currentId = setlists.OrderBy(s => s.Id).LastOrDefault()?.Id ?? 0;
        return currentId + 1;
    }

    private class StoredSong
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public TimeSpan Duration { get; set; }


        public Song ToDataSong()
        {
            return new Song
            {
                Id = Id,
                Name = Name,
                Duration = Duration
            };
        }
    }

    private class StoredSetlist
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<int> SongIds { get; set; } = [];

        public Setlist ToDataSetlist(ISyncLocalStorageService localStorageService)
        {
            var songs = localStorageService.GetItem<StoredSong[]>("songs") ?? [];

            return new Setlist
            {
                Id = Id,
                Name = Name,
                Songs = [.. SongIds
                    .Select(id => songs.First(s => s.Id == id))
                    .Select(s => s.ToDataSong())]
            };
        }
    }

    private record StoredAlbum(int Id, string Name, List<int> SongIds);
    private record StoredArtist(int Id, string Name, List<int> SongIds, List<int> SetlistIds);
}
