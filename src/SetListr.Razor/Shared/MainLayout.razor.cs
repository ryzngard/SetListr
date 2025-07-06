using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace SetListr.Razor.Shared;
public partial class MainLayout
{
    private bool _isExpanded = true;
    public bool Expanded
    {
        get => _isExpanded;
        set
        {
            if (_isExpanded == value) return;

            _isExpanded = value;
            ThemeGridHidden = !_isExpanded;
            StateHasChanged();
        }
    }

    public DesignThemeModes Mode { get; set; } = DesignThemeModes.System;

    public bool ThemeGridHidden { get; set; } = false;

    public string CustomColor
    {
        get => OfficeColor.ToAttributeValue() ?? "default";
        set
        {
            if (Enum.TryParse<Microsoft.FluentUI.AspNetCore.Components.OfficeColor>(value, out var parsed))
            {
                OfficeColor = parsed;
            }
        }
    }

    public OfficeColor? OfficeColor { get; set; } = Microsoft.FluentUI.AspNetCore.Components.OfficeColor.Default;

    void PickRandomColor()
    {
        OfficeColor = OfficeColorUtilities.GetRandom();
    }
  private void NavMenuExpansionChanged(bool expanded)
  {
        ThemeGridHidden = !expanded;
        StateHasChanged();
  }
}