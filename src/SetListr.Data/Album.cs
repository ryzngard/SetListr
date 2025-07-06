using System;
using System.Collections.Generic;

namespace SetListr.Data;

public class Album
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Song> Songs { get; set; } = [];
    public DateTime? ReleaseDate { get; set; }
}
