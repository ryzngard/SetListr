using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

using SetListr.Data;

namespace SetListr.Razor.Pages;
public partial class Songs
{
    private readonly PaginationState _pagination = new() { ItemsPerPage = 20 };
    private IQueryable<Song> _songs = Array.Empty<Song>().AsQueryable();

    protected override void OnInitialized()
    {
        _songs = Db.Songs;
    }

    private async Task DeleteSongAsync(Song song)
    {
        await Db.RemoveAsync(song);
        _songs = Db.Songs;
    }
    private void AddSongClicked(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
    {
        Navigation.NavigateTo("songs/add");
    }
}