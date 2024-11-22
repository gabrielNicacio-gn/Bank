namespace Bank.Bank.Application.Exceptions;

public class AccountNotExistException : Exception
{
    public AccountNotExistException(string message) : base(message){ }
}