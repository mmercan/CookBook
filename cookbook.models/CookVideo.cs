using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CookBook.Models
{
    public class CookVideo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string VideoType { get; set; }
        public string Data { get; set; }
        public string Url { get; set; }
    }
}