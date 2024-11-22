namespace Bank.Bank.Application.Exceptions;

public class AccountNotFoundException : System.Exception
{
    public AccountNotFoundException(string message) : base(message) { }
}