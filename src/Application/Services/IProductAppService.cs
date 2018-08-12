using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Products.Application.Adapters;
using Products.Application.Contracts;
using Products.Domain.Models;
using Products.Domain.Repositories;

namespace Products.Application.Services
{
    public interface IProductAppService {
        object Add(ProductContract contract);

        void Edit(object id, ProductContract contract);

        ProductContract GetById(object id);

        IEnumerable<ProductContract> All();

        PageResult<ProductContract> GetPage(int page);
        
        void Delete(object id);
    }
}