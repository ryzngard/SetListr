using System;
using System.Collections.Generic;
using System.Linq;

namespace SetListr.Data;

public class Setlist
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Song> Songs { get; set; } = [];

    public TimeSpan Duration => Songs.Aggregate(TimeSpan.Zero, (sum, current) => sum + current.Duration);
}
