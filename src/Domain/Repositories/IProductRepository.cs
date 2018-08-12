using System;
using System.Collections.Generic;
using System.Linq;
using Products.Domain.Models;

namespace Products.Domain.Repositories
{
    public interface IProductRepository : IDisposable
    {
        Product Add(Product product);
        void Edit(Product product);
        void Delete(object id);
        Product GetById(object id);
        IQueryable<Product> All();
        PageResult<Product> GetPage(int page, int maxItemsPerPage);
        PageResult<Product> GetPage(int page);
    }
}