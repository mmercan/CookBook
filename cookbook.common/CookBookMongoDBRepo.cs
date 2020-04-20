using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using CookBook.Models;
using MongoDB.Driver;

namespace CookBook.Common
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
            // await _recipeRepo.FindAsync(FilterDefinition<Recipe>.Empty)
            return _recipeRepo.Find(FilterDefinition<Recipe>.Empty).ToListAsync();  //.ToList();
        }

        public Task<List<Recipe>> PagedGetAll(int pageNumber, int pageSize)
        {
            var skips = pageSize * (pageNumber - 1);
            // Skip and limit
            var pageditems = _recipeRepo.Find(FilterDefinition<Recipe>.Empty).Skip(skips).Limit(pageSize).ToListAsync();
            return pageditems;
        }

        public Task<List<Recipe>> Find(string Term)
        {
            // FilterDefinition<Recipe> filter=  FilterDefinition<Recipe>.Empty;
            var builder = Builders<Recipe>.Filter;
            //var filter = builder.Eq("x", 10) & builder.Lt("y", 20);
            //var filter = builder.Eq(r => r.Name, Term ); //& builder.Lt("y", 20);
            //var filter = builder.Text(r => r.Name, Term )
            var filter = builder.Text(Term, new TextSearchOptions { CaseSensitive = false });
            return _recipeRepo.Find(filter).ToListAsync();
        }

        public Task Insert(Recipe recipe)
        {
            return _recipeRepo.InsertOneAsync(recipe);
        }

        public Task Update(Recipe recipe)
        {
            var filter = Builders<Recipe>.Filter.Eq((r) => r.Id, recipe.Id);
            //             UpdateDefinition<Recipe> def = new MongoDB.Driver.UpdateDefinition<Recipe>()
            return _recipeRepo.ReplaceOneAsync(filter, recipe);
        }

        public Task Delete(Recipe recipe)
        {
            var filter = Builders<Recipe>.Filter.Eq((r) => r.Id, recipe.Id);
            return _recipeRepo.DeleteOneAsync(filter);
            // DeleteResult
        }
    }
}
