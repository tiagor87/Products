using System.Diagnostics.CodeAnalysis;
using MongoDB.Driver;
using Products.Domain.Models;
using Products.Infrastructure.Providers.MongoDB.Entities;

namespace Products.Infrastructure.Providers.MongoDB
{
    [ExcludeFromCodeCoverage()]
    public class ProductProvider : BaseProvider<ProductEntity, Product>
    {
        public ProductProvider(IMongoDatabase database) : base(database, "products") {
        }
    }
}