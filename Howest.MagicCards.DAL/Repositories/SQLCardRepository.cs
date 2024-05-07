using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SQLCardRepository : ICardRepository
    {
        private readonly MTGContext _db;

        public SQLCardRepository(MTGContext db)
        {
            _db = db;
        }

        public IQueryable<Card> GetAllCards()
        {
            return _db.Cards.Select(c => c);
        }

        public async Task<Card> GetCardById(int id)
        {
            return await _db.Cards.SingleOrDefaultAsync(c => c.Id == id);
        }


        public IQueryable<Card> GetCardsByArtist(long artistId)
        {
            return _db.Cards.Where(card => card.Artist.Id == artistId);
        }
    }
}
