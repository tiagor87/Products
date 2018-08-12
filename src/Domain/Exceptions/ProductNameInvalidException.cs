using Products.Domain.Exceptions;

namespace Products.Domain.Exceptions
{
    public class ProductNameInvalidException : BaseException
    {
        private const string MESSAGE = "The product's name must be defined.";
        
        public ProductNameInvalidException() : base(MESSAGE)
        {
        }
    }
}