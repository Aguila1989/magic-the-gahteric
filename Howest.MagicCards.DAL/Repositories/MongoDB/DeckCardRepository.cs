using BookStoreApi.Models;
using Howest.MagicCards.DAL.Models.MongoDbModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories.MongoDB
{
    public class DeckCardRepository : IDeckCardRepository
    {
        private readonly IMongoCollection<DeckCard> _deckCardCollection;
        public DeckCardRepository(IOptions<MongoDBSettings> settings)
        {
            var mongoClient = new MongoClient(
                settings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                settings.Value.DatabaseName);

            _deckCardCollection = mongoDatabase.GetCollection<DeckCard>(
                settings.Value.CollectionName);
        }


        public async Task<List<DeckCard>> GetAllDeckCards() =>
            await _deckCardCollection.Find(dc => true).ToListAsync();

        public async Task<DeckCard> GetDeckCardById(decimal id) =>
            await _deckCardCollection.Find(dc => dc.DeckCardId == id).FirstOrDefaultAsync();

        public async Task CreateDeckCard(DeckCard newDeckCard) =>
            await _deckCardCollection.InsertOneAsync(newDeckCard);

        public async Task UpdateDeckCard(int cardID)
        {
            List<DeckCard> deckCards = await GetAllDeckCards();

            if (FullDeck(deckCards))
            {
                throw new Exception("Deck is full");
            }

            DeckCard cardFound = deckCards.FirstOrDefault(dc => dc.DeckCardId == cardID);

            if (cardFound == null)
            {
                await _deckCardCollection.InsertOneAsync(new DeckCard { DeckCardId = cardID, Quantity = 1 });
            }
            else
            {
                // Increase quantity
                cardFound.Quantity++;
                await _deckCardCollection.ReplaceOneAsync(dc => dc.DeckCardId == cardID, cardFound);
            }

        }

        public async Task DeleteDeckCard(decimal id) =>
            await _deckCardCollection.DeleteOneAsync(dc => dc.DeckCardId == id);

        public async Task<bool> Exists(decimal deckCardId)
        {
            var existingCard = await GetDeckCardById(deckCardId);
            return existingCard != null;
        }

        private bool FullDeck(List<DeckCard> deckCards)
        {
            int fullDeck = 60;
            return deckCards.Sum(dc => dc.Quantity) >= fullDeck;
        }
    }


}
