using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace CookBook.Models
{
    public class Recipe
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Method { get; set; }
        public List<CookPhoto> Photos { get; set; }
        public List<CookVideo> Videos { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string CookType { get; set; }
        public string Level { get; set; }
        public int TimeToPrepMin { get; set; }
        public int TimeToCookMin { get; set; }

    }
}