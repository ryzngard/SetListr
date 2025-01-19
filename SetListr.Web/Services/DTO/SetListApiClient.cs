
using System.Collections.Generic;
using System.Collections.Immutable;

using SetListr.Web.Extensions;

namespace SetListr.Web.Services.DTO;

public class SetListApiClient(HttpClient httpClient, ILoggerFactory loggerFactory)
{
    private readonly ILogger<SetListApiClient> _logger = loggerFactory.CreateLogger<SetListApiClient>();

    public async Task<SetList?> GetSetListAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await GetAsync<SetList>(httpClient, $"/setList/{id}", cancellationToken);
            response.EnsureAuthorized();

            return response.Value;
        }
        catch (Exception e) when (e is not UnauthorizedAccessException)
        {
            _logger.LogError(e, "Error getting set list {id}", id);
            return null;
        }
    }

    public async Task<IQueryable<Song>?> GetSongsForBandAsync(Guid bandId, int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<Song>? songs = null;

        try
        {
            var result = await GetEnumerableAsync<Song>(httpClient, $"/songs?bandId={bandId}", cancellationToken);
            result.EnsureAuthorized();

            await foreach (var song in result.Values)
            {
                if (songs?.Count >= maxItems)
                {
                    break;
                }
                if (song is not null)
                {
                    songs ??= [];
                    songs.Add(song);
                }
            }

            return songs?.AsQueryable();
        }
        catch (Exception e) when (e is not UnauthorizedAccessException)
        {
            _logger.LogError(e, "Error getting songs for band {bandId}", bandId);
            return null;
        }
    }

    public async Task<IQueryable<SetList>?> GetSetListsAsync(Guid? bandId, CancellationToken cancellationToken, int maxItems = 10)
    {
        List<SetList>? setLists = null;

        var response = bandId.HasValue
            ? await GetEnumerableAsync<SetList>(httpClient, $"/setLists?bandId={bandId}", cancellationToken)
            : await GetEnumerableAsync<SetList>(httpClient, "/setLists", cancellationToken);

        response.EnsureAuthorized();

        try 
        {
            await foreach (var setList in response.Values)
            {
                if (setLists?.Count >= maxItems)
                {
                    break;
                }

                if (setList is not null)
                {
                    setLists ??= [];
                    setLists.Add(setList);
                }
            }
            return setLists?.AsQueryable();
        }
        catch (Exception e) when (e is not UnauthorizedAccessException)
        {
            _logger.LogError(e, "Error getting set lists");
            return null;
        }

    }

    public async Task<IQueryable<SetList>?> GetSetListsAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<SetList>? setLists = null;

        try 
        {
            var result = await GetEnumerableAsync<SetList>(httpClient, "/setLists", cancellationToken);
            result.EnsureAuthorized();

            await foreach (var setList in result.Values)
            {
                if (setLists?.Count >= maxItems)
                {
                    break;
                }

                if (setList is not null)
                {
                    setLists ??= [];
                    setLists.Add(setList);
                }
            }

            return setLists?.AsQueryable();
        }
        catch (Exception e) when (e is not UnauthorizedAccessException)
        {
            _logger.LogError(e, "Error getting set lists");
            return null;
        }
    }

    private static async Task<AsyncEnumerableResult<T>> GetEnumerableAsync<T>(HttpClient httpClient, string uri, CancellationToken cancellationToken)
    {
        var res = await httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
        if (res.IsSuccessStatusCode)
        {
            var values = res.Content.ReadFromJsonAsAsyncEnumerable<T>(cancellationToken);
            return new AsyncEnumerableResult<T>(Success: true, values, Authorized: true);
        }

        if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return new AsyncEnumerableResult<T>(Success: false, AsyncEnumerable.Empty<T>(), Authorized: false);
        }
        
        return new AsyncEnumerableResult<T>(Success: false, AsyncEnumerable.Empty<T>(), Authorized: true);
    }

    private static async Task<Result<T>> GetAsync<T>(HttpClient httpClient, string uri, CancellationToken cancellationToken)
    {
        var res = await httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
        if (res.IsSuccessStatusCode)
        {
            var value = await res.Content.ReadFromJsonAsync<T>(cancellationToken);
            return new Result<T>(Success: true, value, Authorized: true);
        }

        if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return new Result<T>(Success: false, default, Authorized: false);
        }
        
        return new Result<T>(Success: false, default, Authorized: true);
    }

    private record struct Result<T>(bool Success, T? Value, bool Authorized)
    {
        public void EnsureAuthorized() 
        {
            if (!Authorized)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }

    private record struct AsyncEnumerableResult<T>(bool Success, IAsyncEnumerable<T?> Values, bool Authorized)
    {
        public void EnsureAuthorized() 
        {
            if (!Authorized)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}

public record Song(Guid Id, string Name, TimeSpan Duration)
{
}

public record SetList(Guid Id, string Name, ImmutableArray<Song> Songs)
{
    public TimeSpan Duration => Songs.Any() 
        ? Songs.Select(s => s.Duration).Aggregate((t1, t2) => t1 + t2)
        : TimeSpan.Zero;

    public SetList SwapSongs(Song from, Song to)
    {
        var indexFrom = Songs.IndexOf(from);
        var indexTo = Songs.IndexOf(to);

        var newSongs = Songs.ToArray();
        newSongs[indexFrom] = to;
        newSongs[indexTo] = from;

        return this with { Songs = newSongs.ToImmutableArray() };
    }

    public SetList InsertSongAfter(Song before, Song after)
    {
        var newSongs = Songs.ToList();
        newSongs.Remove(after);
        var indexBefore = newSongs.IndexOf(before);
        var newIndex = indexBefore + 1;

        if (newIndex >= newSongs.Count)
        {
            newSongs.Add(after);
            return this with { Songs = newSongs.ToImmutableArray() };
        }
        else 
        {
            newSongs.Insert(newIndex, after);
            return this with { Songs = newSongs.ToImmutableArray() };
        }
    }
}

public record Band(Guid Id, string Name, ImmutableArray<Song> Songs, ImmutableArray<SetList> SetLists)
{
}