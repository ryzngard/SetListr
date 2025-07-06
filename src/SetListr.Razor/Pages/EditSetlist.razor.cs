using System.Collections.Immutable;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;

using SetListr.Data;
using SetListr.Razor.Utilities;

namespace SetListr.Razor.Pages;
public partial class EditSetlist
{
    [Parameter]
    public int? Id { get; set; }
    public const string NameFieldName = "Name";

    private ValidationMessageStore _messageStore = null!;
    private FluentEditForm? _form = null;

#pragma warning disable IDE0044 // Add readonly modifier
    private JustifyContent Justification = JustifyContent.FlexStart;
#pragma warning restore IDE0044 // Add readonly modifier

    private FluentEditForm Form
    {
        get => _form.AssumeNotNull();
        set
        {
            _form = value;

            var editContext = value.EditContext.AssumeNotNull();
            _messageStore = new(editContext);
            editContext.OnValidationRequested += ValidationRequested;
            editContext.OnFieldChanged += FieldChanged;
        }
    }

    private FluentSelect<Song>? _songSelect;
    private FluentSelect<Song> SongSelect
    {
        get => _songSelect.AssumeNotNull();
        set
        {
            _songSelect = value;
        }
    }

    private bool HasNoSongs => _setlist.Songs.Count == 0;

    private IEnumerable<Song> SongsNotInSetlist => _allSongs.Except(_setlist.Songs);
    private Setlist _setlist = new() { Name = string.Empty, Songs = [] };
    private readonly List<Song> _allSongs = [];

    protected override void OnInitialized()
    {
        _allSongs.AddRange(Db.Songs);

        if (Id.HasValue)
        {
            var dbSetlist = Db.Setlists
                .Include(s => s.Songs)
                .FirstOrDefault(s => s.Id == Id.Value);

            if (dbSetlist is not null)
            {
                _setlist = dbSetlist;
            }
        }
    }

    private async Task SaveAsync()
    {
        await Db.SaveAsync(_setlist);
        Navigation.NavigateTo("/setlists");
    }

    private void Cancel()
    {
        Db.ClearChanges();
        Navigation.NavigateTo("/setlists");
    }

    private void FieldChanged(object? sender, FieldChangedEventArgs e)
    {
        if (e.FieldIdentifier.FieldName != NameFieldName)
        {
            return;
        }

        ValidateNameField(e.FieldIdentifier);
    }

    private void ValidationRequested(object? sender, ValidationRequestedEventArgs e)
    {
        var field = Form.EditContext.AssumeNotNull().Field(NameFieldName);
        ValidateNameField(field);
    }

    private void ValidateNameField(FieldIdentifier fieldIdentifier)
    {
        if (fieldIdentifier.FieldName != NameFieldName)
        {
            throw new InvalidOperationException();
        }

        _messageStore.Clear(fieldIdentifier);

        if (string.IsNullOrWhiteSpace(_setlist.Name))
        {
            _messageStore.Add(fieldIdentifier, "Name is required.");
        }
        else if (Db.Setlists.Any(s => s.Name == _setlist.Name && s.Id != _setlist.Id))
        {
            _messageStore.Add(fieldIdentifier, "A setlist with this name already exists.");
        }

        if (_messageStore[fieldIdentifier].Any())
        {
            Form.EditContext.AssumeNotNull().NotifyValidationStateChanged();
        }
    }

    private void AddSong(Song args)
    {
        _setlist.Songs.Add(args);
    }

    private void SortList(FluentSortableListEventArgs args)
    {
        if (args is null || args.OldIndex == args.NewIndex)
        {
            return;
        }

        var oldIndex = args.OldIndex;
        var newIndex = args.NewIndex;

        var itemToMove = _setlist.Songs[oldIndex];
        _setlist.Songs.RemoveAt(oldIndex);

        if (newIndex < _setlist.Songs.Count)
        {
            _setlist.Songs.Insert(newIndex, itemToMove);
        }
        else
        {
            _setlist.Songs.Add(itemToMove);
        }
    }
}