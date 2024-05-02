using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SQLCardRepository : ICardRepository
    {
        private readonly CardsContext _db;

        public SQLCardRepository()
        {
            _db = new CardsContext();
        }
        public IQueryable<Card> GetAllCards()
        {
            IQueryable<Card> allCards = _db.Cards
                                                .Select(c => c);
            return allCards;
        }

        public Card GetCardbyId(int id)
        {
            Card oneCard = _db.Cards
                                .SingleOrDefault(c => c.Id == id);

            return oneCard;
        }
    }

}
