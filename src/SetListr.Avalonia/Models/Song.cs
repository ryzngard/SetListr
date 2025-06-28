using System;

namespace SetListr.Avalonia.Models;

public class Song
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public TimeSpan Duration { get; set; }
}
