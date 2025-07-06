using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetListr.Data;

public interface ISetListrDb
{
    IQueryable<Song> Songs { get; }
    IQueryable<Setlist> Setlists { get; }
    IQueryable<Album> Albums { get; }
    IQueryable<Artist> Artists { get; }

    Task<bool> SaveAsync(Song song);
    Task<bool> RemoveAsync(Song song);
    Task<bool> SaveAsync(Setlist setlist);
    Task<bool> RemoveAsync(Setlist setlist);
    Task<bool> SaveAsync(Album album);
    Task<bool> RemoveAsync(Album album);
    Task<bool> SaveAsync(Artist artist);
    Task<bool> RemoveAsync(Artist artist);

    public void ClearChanges();
}
