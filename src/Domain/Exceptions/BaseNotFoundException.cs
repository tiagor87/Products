using System;

namespace Products.Domain.Exceptions
{
    public abstract class BaseNotFoundException : BaseException {
        public BaseNotFoundException(string message) : base(message) {

        }

        public BaseNotFoundException(string message, Exception innerException) : base(message, innerException) {
            
        }
    }
}