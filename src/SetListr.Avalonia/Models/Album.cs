using System;
using System.Collections.Generic;

namespace SetListr.Avalonia.Models;

public class Album
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Song> Songs { get; set; } = new();
    public DateTime? ReleaseDate { get; set; }
}
