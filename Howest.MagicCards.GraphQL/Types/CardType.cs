using GraphQL.Types;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;

namespace Howest.MagicCards.GraphQL.Types
{
    public class CardType : ObjectGraphType<Card>
    {
        public CardType(IArtistRepository artistRepo) 
        {
            Name = "Card";

            Field(c => c.Id, type: typeof(IdGraphType)).Description("Card Id");
            Field(c => c.Name).Description("Card Name");
            Field(c => c.ManaCost).Description("Card Mana Cost");
            Field(c => c.ConvertedManaCost).Description("Card Converted Mana Cost");
            Field(c => c.Type).Description("Card Type");
            Field(c => c.RarityCode).Description("Card Rarity Code");
            Field(c => c.SetCode).Description("Card Set Code");
            Field(c => c.Text).Description("Card Text");
            Field(c => c.Flavor).Description("Card Flavor");
            Field(c => c.OriginalImageUrl).Description("Card Url To Image");
            Field(c => c.MultiverseId, nullable: true).Description("Card Multiverse ID");

            Field<ArtistType>("Artist of this card").Resolve(context => artistRepo.GetArtistById(context.Source.Id));
        }
    }
}
