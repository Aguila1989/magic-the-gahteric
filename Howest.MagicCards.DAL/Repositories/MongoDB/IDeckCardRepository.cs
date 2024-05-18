using Howest.MagicCards.DAL.Models.MongoDbModels;

namespace Howest.MagicCards.DAL.Repositories.MongoDB
{
    public interface IDeckCardRepository
    {
        Task CreateDeckCard(DeckCard newDeckCard);
        Task DeleteDeckCard(decimal id);
        Task<List<DeckCard>> GetAllDeckCards();
        Task<DeckCard> GetDeckCardById(decimal id);
        Task UpdateDeckCard(int cardID);
        Task<bool> Exists(decimal deckCardId);
        Task DecreaseDeckCardQuantity(int cardID);
    }
}