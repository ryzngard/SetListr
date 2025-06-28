using System;
using System.Text;
using System.Threading;

namespace SetListr.Avalonia;


internal static partial class Assumed
{
    private ref struct PooledStringBuilderHelper
    {
        private StringBuilder? _builder;

        public PooledStringBuilderHelper(int capacity, bool condition)
        {
            if (condition)
            {
                _builder = StringBuilderPool.Get();
                _builder.EnsureCapacity(capacity);
            }
        }

        public readonly void AppendLiteral(string value)
            => _builder!.Append(value);

        public readonly void AppendFormatted<T>(T value)
            => _builder!.Append(value?.ToString());

        public readonly void AppendFormatted<TValue>(TValue value, string format)
            where TValue : IFormattable
            => _builder!.Append(value?.ToString(format, formatProvider: null));

        public string GetFormattedText()
        {
            var builder = Interlocked.Exchange(ref _builder, null);

            if (builder is not null)
            {
                var result = builder.ToString();
                StringBuilderPool.Return(builder);

                return result;
            }

            // GetFormattedText() should never be called if the condition passed in was false.
            return Unreachable<string>();
        }
    }
}


