using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.ObjectPool;

namespace SetListr.Avalonia;


// Originally derived from https://github.com/dotnet/razor/blob/1b931e8879948986442ab29949ca57edcea58ac9/src/Shared/Microsoft.AspNetCore.Razor.Utilities.Shared/Assumed.cs
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
internal static partial class Assumed
{
    private static readonly ObjectPool<StringBuilder> StringBuilderPool = ObjectPool.Create<StringBuilder>();
    public static void NotNull<T>(
        [NotNull] this T? value,
        string? message = null,
        [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
        where T : class
    {
        if (value is null)
        {
            ThrowInvalidOperation(message ?? $"Expected {valueExpression} to not be null", path, line);
        }
    }

    public static void NotNull<T>(
        [NotNull] this T? value,
        [InterpolatedStringHandlerArgument(nameof(value))] ThrowIfNullInterpolatedStringHandler<T> message,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
        where T : class
    {
        if (value is null)
        {
            ThrowInvalidOperation(message.GetFormattedText(), path, line);
        }
    }

    public static void NotNull<T>(
        [NotNull] this T? value,
        string? message = null,
        [CallerArgumentExpression(nameof(value))] string? valueExpression = null,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
        where T : struct
    {
        if (value is null)
        {
            ThrowInvalidOperation(message ?? $"Expected {valueExpression} to be non null", path, line);
        }
    }

    public static void NotNull<T>(
        [NotNull] this T? value,
        [InterpolatedStringHandlerArgument(nameof(value))] ThrowIfNullInterpolatedStringHandler<T> message,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
        where T : struct
    {
        if (value is null)
        {
            ThrowInvalidOperation(message.GetFormattedText(), path, line);
        }
    }

    public static void False(
        [DoesNotReturnIf(true)] bool condition,
        string? message = null,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
    {
        if (condition)
        {
            ThrowInvalidOperation(message ?? "Expected condition to be false", path, line);
        }
    }

    public static void False(
        [DoesNotReturnIf(true)] bool condition,
        [InterpolatedStringHandlerArgument(nameof(condition))] ThrowIfFalseInterpolatedStringHandler message,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
    {
        if (condition)
        {
            ThrowInvalidOperation(message.GetFormattedText(), path, line);
        }
    }

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void True(
        [DoesNotReturnIf(false)] bool condition,
        string? message = null,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
    {
        if (!condition)
        {
            ThrowInvalidOperation(message ?? "Expected condition to be true", path, line);
        }
    }

    /// <summary>
    ///  Can be called at points that are assumed to be unreachable at runtime.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    [DoesNotReturn]
    public static void Unreachable(
        string? message = null,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
        => ThrowInvalidOperation(message ?? "This program location is thought to be uncreachable", path, line);

    /// <summary>
    ///  Can be called at points that are assumed to be unreachable at runtime.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    [DoesNotReturn]
    public static void Unreachable(
        UnreachableInterpolatedStringHandler message,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
        => ThrowInvalidOperation(message.GetFormattedText(), path, line);

    /// <summary>
    ///  Can be called at points that are assumed to be unreachable at runtime.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    [DoesNotReturn]
    public static T Unreachable<T>(
        string? message = null,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
        => ThrowInvalidOperation<T>(message ?? "This program location is thought to be uncreachable", path, line);

    /// <summary>
    ///  Can be called at points that are assumed to be unreachable at runtime.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    [DoesNotReturn]
    public static T Unreachable<T>(
        UnreachableInterpolatedStringHandler message,
        [CallerFilePath] string? path = null,
        [CallerLineNumber] int line = 0)
        => ThrowInvalidOperation<T>(message.GetFormattedText(), path, line);

    [DebuggerHidden]
    [DoesNotReturn]
    private static void ThrowInvalidOperation(string message, string? path, int line)
        => ThrowHelper.ThrowInvalidOperationException(message + Environment.NewLine + $"File {path} line {line}");

    [DebuggerHidden]
    [DoesNotReturn]
    private static T ThrowInvalidOperation<T>(string message, string? path, int line)
        => ThrowHelper.ThrowInvalidOperationException<T>(message + Environment.NewLine + $"File {path} line {line}");
}


