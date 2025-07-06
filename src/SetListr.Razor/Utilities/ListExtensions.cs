using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetListr.Razor.Utilities;
internal static class ListExtensions
{
    public static void Swap<T>(this List<T> values, T item1, T item2)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(values));
        ArgumentException.ThrowIfNullOrEmpty(nameof(item1));
        ArgumentException.ThrowIfNullOrEmpty(nameof(item2));

        var index1 = values.IndexOf(item1);
        var index2 = values.IndexOf(item2);
        if (index1 < 0 || index2 < 0)
        {
            throw new ArgumentException("Both items must be present in the list.");
        }

        values[index1] = item2;
        values[index2] = item1;
    }
}
