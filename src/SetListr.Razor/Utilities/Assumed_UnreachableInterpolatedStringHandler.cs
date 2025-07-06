using System.Runtime.CompilerServices;

namespace SetListr.MauiBlazor;

public static partial class Assumed
{
    [InterpolatedStringHandler]
    public readonly ref struct UnreachableInterpolatedStringHandler(int literalLength, int _)
    {
        private readonly PooledStringBuilderHelper _builder = new(literalLength, condition: true);

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

