namespace Products.Domain.Exceptions
{
    public class PageCountInvalidException : BaseException {
        private const string MESSAGE = "Page count must be greater than zero";
        
        public PageCountInvalidException() : base(MESSAGE)
        {
        }
    }
}