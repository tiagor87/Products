using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Products.Domain.Models;
using Products.Domain.Repositories;

namespace Products.Infrastructure.Repositories
{

    internal class ProductDocument
    {
        public ProductDocument(Product product)
        {
            this.Id = product.Id != null ? ObjectId.Parse(product.Id.ToString()) : ObjectId.Empty;
            this.Name = product.Name;
            this.Value = product.Value;
        }

        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }

        public Product GetModel() {
            return new Product(this.Id, this.Name, this.Value);
        }

    }

    public class ProductRepository : IProductRepository
    {
        private bool disposed;
        private readonly IMongoClient client;

        private readonly IMongoDatabase database;

        public ProductRepository() {
            this.client = new MongoClient("mongodb://localhost:27017");
            this.database = this.client.GetDatabase("db");
        }

        public ProductRepository(IMongoDatabase database) {
            this.database = database;
        }

        private IMongoCollection<ProductDocument> Products {
            get
            {
                return this.database.GetCollection<ProductDocument>("products", new MongoCollectionSettings { AssignIdOnInsert = true });
            }
        }

        public Product Add(Product product)
        {
            var productDocument = new ProductDocument(product);
            this.Products.InsertOne(productDocument);
            return productDocument.GetModel();
        }

        public IEnumerable<Product> All()
        {
            return this.Products.Find(FilterDefinition<ProductDocument>.Empty).ToList().Select(p => p.GetModel());
        }

        public void Delete(object id)
        {
            this.Products.DeleteOne(p => p.Id.Equals(ObjectId.Parse(id.ToString())));
        }

        public Product Edit(Product product)
        {
            var productDocument = new ProductDocument(product);
            this.Products.FindOneAndReplace(p => p.Id.Equals(productDocument.Id), productDocument);
            return productDocument.GetModel();
        }

        public Product GetById(object id)
        {
            return this.Products.Find(p => p.Id.Equals(ObjectId.Parse(id.ToString()))).ToEnumerable().Select(p => p.GetModel()).FirstOrDefault();
        }

        public PageResult<Product> GetPage(int page) => this.GetPage(page, 20);

        public PageResult<Product> GetPage(int page, int maxItemsPerPage)
        {
            var countTask = this.Products.CountDocumentsAsync(FilterDefinition<ProductDocument>.Empty);
            var productsTask = this.Products.Find(FilterDefinition<ProductDocument>.Empty)
                .SortBy(p => p.Id)
                .Limit(maxItemsPerPage)
                .Skip((page - 1) * maxItemsPerPage)
                .ToListAsync();
            Task.WaitAll(countTask, productsTask);
            return new PageResult<Product>((long) Math.Ceiling(1.0m * countTask.Result / maxItemsPerPage), page, productsTask.Result.Select(p => p.GetModel()));
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void BeforeDispose() {}

        protected void Dispose(bool disposing) {
            if (this.disposed) return;
            if (disposing) {
                this.BeforeDispose();
            }
            this.disposed = true;
        }
    }
}