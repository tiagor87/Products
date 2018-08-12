using System;
using Products.Domain.Exceptions;

namespace Products.Application.Exceptions
{
    public class ProductIsNullException : BaseException
    {
        private const string MESSAGE = "Product must be given.";

        public ProductIsNullException() : base(MESSAGE)
        {
        }
    }
}