namespace SimplifiedBank.Interfaces.Exceptions;

public class UnauthorizedTransactionException : Exception
{
        public UnauthorizedTransactionException() { }
        public UnauthorizedTransactionException(string message) : base(message) { }

}