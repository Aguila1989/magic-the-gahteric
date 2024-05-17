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



            deckCardsGroup.MapPost("", async (DeckCard newDeckCard) =>
            {
                if (await repo.Exists(newDeckCard.DeckCardId))
                {
                    return Results.Conflict($"DeckCard with id {newDeckCard.DeckCardId} already exists");
                }

                await repo.CreateDeckCard(newDeckCard);
                return Results.Created($"{urlPrefix}/students/{newDeckCard.DeckCardId}", newDeckCard);
            })
            .Accepts<DeckCard>("application/json");


            deckCardsGroup.MapPut("{cardID:int}", async (int cardID) =>
            {
                await repo.UpdateDeckCard(cardID);
                return Results.Ok(cardID);
            })
            .Accepts<DeckCard>("application/json");



            deckCardsGroup.MapDelete("{id:int}", async (int id) =>
            {
                await repo.DeleteDeckCard(id);
                return Results.Ok($"Card with id {id} has been deleted");
            });



        }
    }
}
