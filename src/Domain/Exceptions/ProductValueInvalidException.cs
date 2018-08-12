using Products.Domain.Exceptions;

namespace Products.Domain.Exceptions
{
    public class ProductValueInvalidException : BaseException
    {
        private const string MESSAGE = "The product's value must be greater than zero.";

        public ProductValueInvalidException() : base(MESSAGE)
        {
        }
    }
}