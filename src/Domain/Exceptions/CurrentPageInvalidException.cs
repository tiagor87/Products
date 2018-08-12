namespace Products.Domain.Exceptions
{
    public class CurrentPageInvalidException : BaseException {
        private const string MESSAGE = "Current page must be less or equal page count";
        
        public CurrentPageInvalidException() : base(MESSAGE)
        {
        }
    }
}