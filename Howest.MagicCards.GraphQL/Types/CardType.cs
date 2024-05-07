using GraphQL.Types;
using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.GraphQL.Types
{
    public class CardType : ObjectGraphType<Card>
    {
        public CardType() {
            Name = "Card";

            Field(x => x.Id, type: typeof(IdGraphType)).Description("Card Id");
            Field(x => x.Name).Description("Card Name");
            Field(x => x.ManaCost).Description("Card Mana Cost");
            Field(x => x.ConvertedManaCost).Description("Card Converted Mana Cost");
            Field(x => x.Type).Description("Card Type");
            Field(x => x.RarityCode).Description("Card Rarity Code");
            Field(x => x.SetCode).Description("Card Set Code");
            Field(x => x.Text).Description("Card Text");
            Field(x => x.Flavor).Description("Card Flavor");
            Field(x => x.OriginalImageUrl).Description("Card Url To Image");
            Field(x => x.MultiverseId, nullable: true).Description("Card Multiverse ID");
        }
    }
}
