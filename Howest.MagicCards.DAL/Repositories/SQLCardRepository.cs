using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Models;
using Microsoft.Identity.Client;

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
            return _db.Cards
                  .Select(c => c);
            
        }

        public Card GetCardbyId(int id)
        {
            return _db.Cards
                  .SingleOrDefault(c => c.Id == id);
        }

        

    }

}
