using System;
using System.Runtime.CompilerServices;

namespace SetListr.Avalonia;


internal static partial class Assumed
{
    [InterpolatedStringHandler]
    public readonly ref struct ThrowIfNullInterpolatedStringHandler<T>
    {
        private readonly PooledStringBuilderHelper _builder;

        public ThrowIfNullInterpolatedStringHandler(int literalLength, int formattedCount, T? value, out bool success)
        {
            success = value is null;
            _builder = new(literalLength, success);
        }

        public void AppendLiteral(string value)
            => _builder.AppendLiteral(value);

        public void AppendFormatted<TValue>(TValue value)
            => _builder.AppendFormatted(value);

        public void AppendFormatted<TValue>(TValue value, string format)
            where TValue : IFormattable
            => _builder.AppendFormatted(value, format);

        public string GetFormattedText()
            => _builder.GetFormattedText();
    }
}


