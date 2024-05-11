using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IRarityRepository
    {
        Task <IQueryable<Rarity>> GetAllRarities();
    }
}
