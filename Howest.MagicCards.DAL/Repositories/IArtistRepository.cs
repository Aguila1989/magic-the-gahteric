using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IArtistRepository
    {
        IQueryable<Artist> GetArtists();

        Artist GetArtistById(long id);
    }
}