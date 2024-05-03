
using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlArtistRepository : IArtistRepository
    {
        private readonly CardsContext _db;

        public SqlArtistRepository(CardsContext con)
        {
            _db = con;
        }


        public Artist GetArtistById(long id)
        {
            Artist artists = _db.Artists.Where(a => a.Id == id).FirstOrDefault();
            return artists;
        }

        public IQueryable<Artist> GetArtists()
        {
            IQueryable<Artist> artists = _db.Artists.Include(a => a.Cards).Select(a => a);
            return artists;
        }
    }
}
