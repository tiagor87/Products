using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using Products.Domain.Models;

namespace Products.Infrastructure.Providers
{
    public interface IProvider<TModel>
        where TModel : class
    {
        IQueryable<TModel> FindAll();
        TModel FindById(object id);
        long ReplaceOne(object id, TModel model);
        void DeleteOne(object id);
        TModel InsertOne(TModel model);
        Task<long> CountAsync();
        PageResult<TModel> GetPage(int page, int maxItemsPerPage);
    }
}