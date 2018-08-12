using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Products.Domain.Models;
using Products.Domain.Repositories;
using Products.Infrastructure.Exceptions;
using Products.Infrastructure.Providers;

namespace Products.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private bool disposed;
        private readonly IProvider<Product> provider;

        public ProductRepository(IProvider<Product> provider) {
            this.provider = provider;
        }

        public Product Add(Product product)
        {
            return this.provider.InsertOne(product);
        }

        public IQueryable<Product> All()
        {
            return this.provider.FindAll();
        }

        public void Delete(object id)
        {
            this.provider.DeleteOne(id);
        }

        public void Edit(Product product)
        {
            var modifiedCount = this.provider.ReplaceOne(product.Id, product);
            if (modifiedCount == 0) {
                throw new ProductNotUpdatedException(product.Id);
            }
        }

        public Product GetById(object id)
        {
            return this.provider.FindById(id);
        }

        public PageResult<Product> GetPage(int page) => this.GetPage(page, 20);

        public PageResult<Product> GetPage(int page, int maxItemsPerPage)
        {
            return this.provider.GetPage(page, maxItemsPerPage);
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