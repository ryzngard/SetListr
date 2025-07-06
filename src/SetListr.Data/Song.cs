using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SetListr.Data;

public class Song
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public TimeSpan Duration { get; set; }
}
