using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Models;
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
