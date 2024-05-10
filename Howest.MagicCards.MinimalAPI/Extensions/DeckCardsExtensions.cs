using Howest.MagicCards.DAL.Models.MongoDbModels;
using Howest.MagicCards.DAL.Repositories.MongoDB;

namespace Howest.MagicCards.MinimalAPI.Extensions
{
    public static class DeckCardsExtensions
    {
        public static void MapDeckCardsEndpoints(this RouteGroupBuilder deckCardsGroup, string urlPrefix, IDeckCardRepository repo)
        {

            deckCardsGroup.MapGet("/", async () =>
            {
                IEnumerable<DeckCard> deckCards = await repo.GetAllDeckCards();
                return Results.Ok(deckCards);
            });

            deckCardsGroup.MapGet("{id}", async (decimal id) =>
            {
                DeckCard foundDeckCard = await repo.GetDeckCardById(id);
                return foundDeckCard != null
                        ? Results.Ok(foundDeckCard)
                        : Results.NotFound($"DeckCard with id {id} not found");
            });
        }
    }
}
