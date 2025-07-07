using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;

using SetListr.Razor.Utilities;

namespace SetListr.Razor.Pages;

public partial class AddSong
{
    private ValidationMessageStore _messageStore = null!;

    private FluentEditForm? _form = null;
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

    public const string NameFieldName = "Name";

#pragma warning disable IDE0044 // Add readonly modifier
    private Data.Song _newSong = new() { Name = string.Empty, Duration = TimeSpan.FromMinutes(3) };
#pragma warning restore IDE0044 // Add readonly modifier

    private async Task SubmitAsync()
    {
        await Db.SaveAsync(_newSong);
        Navigation.NavigateTo("songs");
    }

    private void GoBack()
    {
        Navigation.NavigateTo("songs");
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

        if (string.IsNullOrWhiteSpace(_newSong.Name))
        {
            _messageStore.Add(fieldIdentifier, "Name is required.");
        }
        else if (Db.Songs.Any(s => s.Name == _newSong.Name))
        {
            _messageStore.Add(fieldIdentifier, "A song with this name already exists.");
        }

        if (_messageStore[fieldIdentifier].Any())
        {
            Form.EditContext.AssumeNotNull().NotifyValidationStateChanged();
        }
    }
}