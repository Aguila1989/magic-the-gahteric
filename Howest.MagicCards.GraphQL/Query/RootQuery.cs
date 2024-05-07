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
                Description = "Get all Cards",
                resolve: context => (cardRepo.GetAllCards()).ToList()
            );

            Field<CardType>(
                "card",
                Description = "Get one Card by Id",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
                resolve: context =>
                {
                    int id = context.GetArgument<int>("id");
                    return cardRepo.GetCardById(id);
                }
            );

            Field<ListGraphType<ArtistType>>(
                "artists",
                Description = "Get all Artists",
                resolve: context => (artistRepo.GetArtists()).ToList()
            );

            Field<ArtistType>(
                "artist",
                Description = "Get one Artist by Id",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
                resolve: context =>
                {
                    int id = context.GetArgument<int>("id");
                    return artistRepo.GetArtistById(id);
                }
            );
        }
    }
}
