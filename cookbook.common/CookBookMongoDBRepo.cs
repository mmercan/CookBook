using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using CookBook.Models;
using MongoDB.Driver;

namespace cookbook.common
{
    public class CookBookMongoDBRepo
    {
        private readonly ICookBookDatabaseSettings _settings;
        private readonly IMongoCollection<Recipe> _recipeRepo;
        public CookBookMongoDBRepo(ICookBookDatabaseSettings settings)
        {

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _settings = settings;
            _recipeRepo = database.GetCollection<Recipe>(settings.CookBookCollectionName);
        }

        public IMongoCollection<Recipe> GetCollection()
        {
            return _recipeRepo;
        }

        public Task<List<Recipe>> GetAll()
        {
            return _recipeRepo.Find(FilterDefinition<Recipe>.Empty).ToList();
        }

        public Task<List<Recipe>> PagedGetAll(int pageNumber, int pageSize)
        {
            var skips = pageSize * (pageNumber - 1);
            // Skip and limit
            var pageditems = _recipeRepo.find().skip(skips).limit(pageSize);

        }

        public Task<List<Recipe>> Find(string Term)
        {

        }

        public Task<Recipe> Insert(Recipe recipe)
        {

        }

        public Task<Recipe> Update(Recipe recipe)
        {

        }

        public Task<Recipe> Delete(Recipe recipe)
        {

        }
    }
}
