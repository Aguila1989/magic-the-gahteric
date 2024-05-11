
using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SQLArtistRepository : IArtistRepository
    {
        private readonly MTGContext _db;

        public SQLArtistRepository(MTGContext db)
        {
            _db = db;
        }


        public async Task <Artist> GetArtistById(long id)
        {
            var existingArtist = await _db.Artists.Where(a => a.Id == id).FirstOrDefaultAsync();
            return existingArtist;
        }

        public async Task<IQueryable<Artist>> GetArtists()
        {
            IQueryable<Artist> allArtists = _db.Artists.Include(a => a.Cards).Select(a => a);
            return await Task.FromResult(allArtists);
        }
    }
}
