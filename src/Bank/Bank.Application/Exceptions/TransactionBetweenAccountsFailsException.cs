namespace Bank.Bank.Application.Exceptions;

public class TransactionBetweenAccountsFailsException : Exception
{
    public TransactionBetweenAccountsFailsException(string? message) : base(message) { }
}