using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;

using SetListr.Data;

namespace SetListr.Razor.Pages;
public partial class Setlists
{
    private readonly PaginationState _pagination = new() { ItemsPerPage = 20 };
    private IQueryable<Setlist> _setLists = Array.Empty<Setlist>().AsQueryable();

    protected override void OnInitialized()
    {
        _setLists = Db.Setlists;
    }
}