using System.Collections.Generic;

namespace SetListr.Data;

public class Artist
{     
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Song> Songs { get; set; } = [];
    public List<Setlist> Setlists { get; set; } = [];
}
