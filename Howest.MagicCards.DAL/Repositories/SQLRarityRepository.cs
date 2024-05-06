using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SQLRarityRepository : IRarityRepository
    {
        private readonly MTGContext _db;

        public SQLRarityRepository(MTGContext db)
        {
            _db = db;
        }
        public IQueryable<Rarity> GetAllRarities()
        {

            IQueryable<Rarity> allRarities = _db.Rarities
                                           .Select(b => b);
            return allRarities;
        }
    }
}
