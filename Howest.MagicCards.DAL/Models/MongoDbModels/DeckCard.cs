using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Howest.MagicCards.DAL.Models.MongoDbModels
{
    public class DeckCard
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public decimal DeckCardId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Quantity { get; set; }
    }
}
