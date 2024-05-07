using GraphQL.Types;
using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.GraphQL.Types
{
    public class ArtistType : ObjectGraphType<Artist>
    {
        public ArtistType() 
        {
            Name = "Artist";

            Field(a => a.Id, type: typeof(IdGraphType)).Description("Artist Id");
            Field(a => a.FullName).Description("Artist Complete Name");
        }
    }
}
