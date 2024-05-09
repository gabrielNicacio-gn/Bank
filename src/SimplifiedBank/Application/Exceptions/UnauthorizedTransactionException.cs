namespace SimplifiedBank.Interfaces.Exceptions;

public class InvalidTransactionException : Exception
{
        public InvalidTransactionException() { }
        public InvalidTransactionException(string message) : base(message) { }

}