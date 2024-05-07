using GraphQL;
using GraphQL.Types;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.GraphQL.Types;

namespace Howest.MagicCards.GraphQL.GraphQL.Query
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery(ICardRepository cardRepo, IArtistRepository artistRepo)
        {
            Field<ListGraphType<CardType>>(
                "cards",
                resolve: context => (cardRepo.GetAllCards()).ToList()
            );
        }
    }
}
