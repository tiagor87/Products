using Products.Application.Contracts;
using Products.Domain.Models;

namespace Products.Application.Adapters
{
    public class ProductAdapter : IAdapter<ProductContract, Product> {
        public ProductContract Convert(Product product) => new ProductContract {
            Id = product.Id,
            Name = product.Name,
            Value = product.Value
        };

        public Product Convert(ProductContract contract) => new Product(contract.Id, contract.Name, contract.Value);
    }
}