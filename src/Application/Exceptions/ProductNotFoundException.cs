using System;
using Products.Domain.Exceptions;

namespace Products.Application.Exceptions
{
    public class ProductNotFoundException : BaseNotFoundException
    {
        private const string MESSAGE = "Product {0} not found.";

        public ProductNotFoundException(object id) : base(string.Format(MESSAGE, id))
        {
        }
    }
}