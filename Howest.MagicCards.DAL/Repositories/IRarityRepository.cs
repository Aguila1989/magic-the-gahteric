using Howest.MagicCards.DAL.Models;
using Type = Howest.MagicCards.DAL.Models.Type;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IRarityRepository
    {
        IQueryable<Rarity> GetAllRarities();
    }
}
