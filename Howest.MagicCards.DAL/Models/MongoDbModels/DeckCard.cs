using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Howest.MagicCards.DAL.Models.MongoDbModels
{
    public class DeckCard
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? DeckCardId { get; set; }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Quantity { get; set; }
    }
}
