namespace Products.Domain.Exceptions
{
    public class ItemsInvalidException : BaseException {
        private const string MESSAGE = "Items should be defined";
        
        public ItemsInvalidException() : base(MESSAGE)
        {
        }
    }
}