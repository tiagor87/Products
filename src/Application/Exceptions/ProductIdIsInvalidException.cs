using System;
using Products.Domain.Exceptions;

namespace Products.Application.Exceptions
{
    public class ProductIdIsInvalidException : BaseException
    {
        private const string MESSAGE = "Product ID must be given.";

        public ProductIdIsInvalidException() : base(MESSAGE)
        {
        }
    }
}