using System;
using System.Collections.Generic;
using System.Linq;

namespace SetListr.Avalonia.Models;

public class Setlist
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Song> Songs { get; set; } = new();

    public TimeSpan Duration => Songs.Aggregate(TimeSpan.Zero, (sum, current) => sum + current.Duration);
}
