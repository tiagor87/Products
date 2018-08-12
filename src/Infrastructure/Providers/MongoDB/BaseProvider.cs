using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Products.Domain.Models;
using Products.Infrastructure.Providers.MongoDB.Entities;
using Products.Infrastructure.Repositories;

namespace Products.Infrastructure.Providers.MongoDB
{

    [ExcludeFromCodeCoverage()]
    public abstract class BaseProvider<T, TModel> : IProvider<TModel>
        where T : IEntity<ObjectId, TModel>, new()
        where TModel : class
    {
        private readonly IMongoCollection<T> collection;

        public BaseProvider(IMongoDatabase database, string collection) {
            this.collection = database.GetCollection<T>(collection, new MongoCollectionSettings { AssignIdOnInsert = true });
        }

        public Task<long> CountAsync()
        {
            return this.collection.CountDocumentsAsync(FilterDefinition<T>.Empty);
        }

        public void DeleteOne(object id)
        {
            this.collection.DeleteOne(d => d.Id.Equals(ObjectId.Parse(id.ToString())));
        }

        public IQueryable<TModel> FindAll()
        {
            return this.collection.Find(FilterDefinition<T>.Empty).ToList().Select(p => p.Model).AsQueryable();
        }

        public TModel InsertOne(TModel model)
        {
            T document = new T() {
                Model = model
            };
            this.collection.InsertOne(document);
            return document.Model;
        }

        public long ReplaceOne(object id, TModel model)
        {
            T document = new T() {
                Model = model
            };
            return this.collection.ReplaceOne(d => d.Id.Equals(ObjectId.Parse(id.ToString())), document).ModifiedCount;            
        }

        public PageResult<TModel> GetPage(int page, int maxItemsPerPage)
        {
            var countTask = this.CountAsync();
            var listTask = this.collection.Find(FilterDefinition<T>.Empty)
                .SortBy(p => p.Id)
                .Limit(maxItemsPerPage)
                .Skip((page - 1) * maxItemsPerPage)
                .ToListAsync();
            Task.WaitAll(countTask, listTask);
            return new PageResult<TModel>((long) Math.Ceiling(1.0m * countTask.Result / maxItemsPerPage), page, listTask.Result.Select(p => p.Model));
        }

        public TModel FindById(object id)
        {
            return this.collection.Find(d => d.Id.Equals(ObjectId.Parse(id.ToString()))).SingleOrDefault()?.Model;
        }
    }
}