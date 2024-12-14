namespace Bank.Bank.Application.Exceptions;

public class FailUpdateBalanceException : Exception
{
    public FailUpdateBalanceException(string message) : base(message) { }
}