using System.Collections.Generic;

namespace SetListr.Avalonia.Models;

public class Artist
{     
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Song> Songs { get; set; } = new();
    public List<Setlist> Setlists { get; set; } = new();
}
