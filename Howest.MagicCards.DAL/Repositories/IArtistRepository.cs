using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IArtistRepository
    {
        Task <IQueryable<Artist>> GetArtists();

        Task <Artist> GetArtistById(long id);
    }
}