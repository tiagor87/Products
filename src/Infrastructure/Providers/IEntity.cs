using MongoDB.Bson;

namespace Products.Infrastructure.Providers
{
    public interface IEntity<TKey, T> where T : class {
        TKey Id { get; set; }

        T Model { get; set; }
    }
}