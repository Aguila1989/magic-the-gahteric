using Howest.MagicCards.DAL.DBContext;
using Type = Howest.MagicCards.DAL.Models.Type;

namespace Howest.MagicCards.DAL.Repositories
{

    public class SQLTypeRepository : ITypeRepository
    {

        private readonly MTGContext _db;

        public SQLTypeRepository(MTGContext db)
        {
            _db = db;
        }

        public async Task<IQueryable<Type>>GetNormalTypes()
        {
            IQueryable<Type> allNormalTypes = _db.Types.Where(t => t.Type1.Equals("normal"));
            return await Task.FromResult(allNormalTypes);
        }
    }
}
