using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SQLCardRepository : ICardRepository
    {
        private readonly CardsContext _db;

        public SQLCardRepository(CardsContext db)
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
