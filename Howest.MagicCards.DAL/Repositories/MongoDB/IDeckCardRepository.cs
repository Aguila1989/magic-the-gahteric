using Howest.MagicCards.DAL.Models.MongoDbModels;

namespace Howest.MagicCards.DAL.Repositories.MongoDB
{
    public interface IDeckCardRepository
    {
        Task CreateDeckCard(DeckCard newDeckCard);
        Task DeleteDeckCard(string id);
        Task<List<DeckCard>> GetAllDeckCards();
        Task<DeckCard> GetDeckCardById(string id);
        Task UpdateDeckCard(DeckCard updatedDeckCard);
    }
}