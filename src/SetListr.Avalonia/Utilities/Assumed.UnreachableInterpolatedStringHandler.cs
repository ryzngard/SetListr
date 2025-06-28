using System;
using System.Runtime.CompilerServices;

namespace SetListr.Avalonia;


internal static partial class Assumed
{
    [InterpolatedStringHandler]
    public readonly ref struct UnreachableInterpolatedStringHandler
    {
        private readonly PooledStringBuilderHelper _builder;

        public UnreachableInterpolatedStringHandler(int literalLength, int formattedCount)
        {
            _builder = new(literalLength, condition: true);
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


