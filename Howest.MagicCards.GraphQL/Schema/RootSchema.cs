using GraphQL.Types;
using Howest.MagicCards.GraphQL.GraphQL.Query;

namespace Howest.MagicCards.GraphQL.GraphQL.Schemas
{
    public class RootSchema : Schema
    {
        public RootSchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<RootQuery>();
        }
    }
}
