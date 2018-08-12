using Products.Domain.Exceptions;

namespace Products.Infrastructure.Exceptions
{
    public class ProductNotUpdatedException : BaseException
    {
        private const string MESSAGE = "Product {0} not updated";

        public ProductNotUpdatedException(object id) : base(string.Format(MESSAGE, id))
        {
        }
    }
}