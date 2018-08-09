using System;
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
    public class ProductAppService {
        private readonly IProductRepository productRepository;
        private readonly IAdapter<ProductContract, Product> adapter;

        public ProductAppService(IProductRepository productRepository, IAdapter<ProductContract, Product> adapter) {
            this.productRepository = productRepository;
            this.adapter = adapter;
        }

        public object Add(ProductContract contract) {
            if (contract == null) {
                throw new ProductIsNullException();
            }

            var product = this.adapter.Convert(contract);
            product.Validate();
            return this.productRepository.Add(product).Id;
        }

        public void Edit(int id, ProductContract contract) {
            if (contract == null) {
                throw new ProductIsNullException();
            }
            var product = this.productRepository.GetById(id);
            product.Name = contract.Name;
            product.Value = contract.Value;
            product.Validate();
            
            this.productRepository.Edit(product);
        }

        public ProductContract GetById(object id) {
            if (id == null) {
                throw new ProductIdIsInvalidException();
            }

            var product = this.productRepository.GetById(id);
            
            if (product == null) {
                throw new ProductNotFoundException(id);
            } 
            
            return this.adapter.Convert(product);
        }

        public IEnumerable<ProductContract> All() => this.productRepository.All().Select(p => this.adapter.Convert(p)).ToList();

        public PageResult<ProductContract> GetPage(int page)
        {
            if (page <= 0) {
                throw new PageInvalidException();
            }
            var pageResult = this.productRepository.GetPage(page);
            return new PageResult<ProductContract>(pageResult.PageCount, pageResult.CurrentPage, pageResult.Items.Select(i => this.adapter.Convert(i)));
        }
        
        public void Delete(object id) {
            if (id == null) {
                throw new ProductIdIsInvalidException();
            }

            this.productRepository.Delete(id);
        }
    }

    public class PageInvalidException : ArgumentException {
        private const string MESSAGE = "Page must be greater than zero";
        
        public PageInvalidException() : base(MESSAGE)
        {
        }
    }

    public class ProductNotFoundException : Exception
    {
        private const string MESSAGE = "Product {0} not found.";

        public ProductNotFoundException(object id) : base(string.Format(MESSAGE, id))
        {
        }
    }

    public class ProductIdIsInvalidException : ArgumentException
    {
        private const string MESSAGE = "Product ID must be given.";

        public ProductIdIsInvalidException() : base(MESSAGE)
        {
        }
    }

    public class ProductIsNullException : ArgumentException
    {
        private const string MESSAGE = "Product must be given.";

        public ProductIsNullException() : base(MESSAGE)
        {
        }
    }
}