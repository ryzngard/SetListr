using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime;
using System.Runtime.CompilerServices;

using SetListr.MauiBlazor;

namespace SetListr.Razor.Utilities;

public static class NullableExtensions
{
    [DebuggerStepThrough]
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static T AssumeNotNull<T>(
        [NotNull] this T? value,
        string? message = null,
        [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
        where T : class
    {
        value.NotNull(message, valueExpression, path, line);
        return value;
    }

    [DebuggerStepThrough]
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static T AssumeNotNull<T>(
        [NotNull] this T? value,
        [InterpolatedStringHandlerArgument(nameof(value))] Assumed.ThrowIfNullInterpolatedStringHandler<T> message,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
        where T : class
    {
        value.NotNull(message, path, line);
        return value;
    }

    [DebuggerStepThrough]
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static T AssumeNotNull<T>(
        [NotNull] this T? value,
        string? message = null,
        [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
        where T : struct
    {
        value.NotNull(message, valueExpression, path, line);
        return value.GetValueOrDefault();
    }

    [DebuggerStepThrough]
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static T AssumeNotNull<T>(
        [NotNull] this T? value,
        [InterpolatedStringHandlerArgument(nameof(value))] Assumed.ThrowIfNullInterpolatedStringHandler<T> message,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
        where T : struct
    {
        value.NotNull(message, path, line);
        return value.GetValueOrDefault();
    }
}
