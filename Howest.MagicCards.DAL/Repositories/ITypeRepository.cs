using Type = Howest.MagicCards.DAL.Models.Type;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface ITypeRepository
    {
        IQueryable<Type> GetNormalTypes();
    }
}
