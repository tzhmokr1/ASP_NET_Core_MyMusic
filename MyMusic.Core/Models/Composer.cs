using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyMusic.Core.Models
{
    public class Composer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Color { get; set; }

        public Zebra zebra = new Zebra
        {
            Legs = 4,
            age = 17
        };
    }
}
