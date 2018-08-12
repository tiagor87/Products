using System;
using Products.Domain.Exceptions;

namespace Products.Application.Exceptions
{
    public class PageInvalidException : BaseException {
        private const string MESSAGE = "Page must be greater than zero";
        
        public PageInvalidException() : base(MESSAGE)
        {
        }
    }
}