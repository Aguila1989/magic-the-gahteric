
using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SQLArtistRepository : IArtistRepository
    {
        private readonly CardsContext _db;

        public SQLArtistRepository(CardsContext db)
        {
            _db = db;
        }


        public Artist GetArtistById(long id)
        {
            return _db.Artists.Where(a => a.Id == id).FirstOrDefault();
        }

        public IQueryable<Artist> GetArtists()
        {
            return _db.Artists.Include(a => a.Cards).Select(a => a);
        }
    }
}
