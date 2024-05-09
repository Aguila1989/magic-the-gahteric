using GraphQL.Types;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;

namespace Howest.MagicCards.GraphQL.Types
{
    public class ArtistType : ObjectGraphType<Artist>
    {
        public ArtistType(ICardRepository cardRepo) 
        {
            Name = "Artist";

            Field(a => a.Id, type: typeof(IdGraphType)).Description("Artist Id");
            Field(a => a.FullName).Description("Artist Complete Name");

            Field<ListGraphType<CardType>>("cards of this artist", resolve:context => cardRepo.GetCardsByArtist(context.Source.Id).ToList());

        }
    }
}
