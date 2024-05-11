using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface ICardRepository
    {
        Task <IQueryable<Card>> GetAllCards();
        Task<Card> GetCardById(int id);
        Task <IQueryable<Card>> GetCardsByArtist(long artistId);
    }
}
