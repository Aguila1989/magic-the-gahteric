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

        public async Task<IQueryable<Card>> GetAllCards()
        {
            IQueryable <Card> allCards = _db.Cards.Select(c => c);
            return await Task.FromResult(allCards);
        }

        public async Task<Card> GetCardById(int id)
        {
            return await _db.Cards.SingleOrDefaultAsync(c => c.Id == id);
        }


        public async Task<IQueryable<Card>> GetCardsByArtist(long artistId)
        {
            IQueryable <Card> allArtistsCards = _db.Cards.Where(card => card.Artist.Id == artistId);
            return await Task.FromResult(allArtistsCards);
        }
    }
}
