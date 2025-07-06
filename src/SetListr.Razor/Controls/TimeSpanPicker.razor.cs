using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace SetListr.Razor.Controls;

public partial class TimeSpanPicker : FluentInputBase<TimeSpan>
{
    private const string TimeFormat = @"mm\:ss";

    /// <summary>
    /// Gets or sets the design of this input.
    /// </summary>
    [Parameter]
    public virtual FluentInputAppearance Appearance { get; set; } = FluentInputAppearance.Outline;

    /// <summary />
    protected override bool TryParseValueFromString(string? value, out TimeSpan result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        validationErrorMessage = null;

        if (string.IsNullOrWhiteSpace(value))
        {
            result = TimeSpan.Zero;
            return true;
        }

        if (TimeSpan.TryParseExact(value, TimeFormat, null, out result))
        {
            return true;
        }

        validationErrorMessage = "Input must be in mm:ss format";
        return false;
    }
}