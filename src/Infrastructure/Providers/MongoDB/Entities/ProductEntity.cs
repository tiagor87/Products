using MongoDB.Bson;
using Products.Domain.Models;

namespace Products.Infrastructure.Providers.MongoDB.Entities
{
    public class ProductEntity : IEntity<ObjectId, Product>
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }

        public Product Model {
            get {
                return new Product(this.Id, this.Name, this.Value);
            }

            set {
                if (value == null) {
                    return;
                }
                this.Id = value.Id != null ? ObjectId.Parse(value.Id.ToString()) : ObjectId.Empty;
                this.Name = value.Name;
                this.Value = value.Value;
            }
        }

    }
}