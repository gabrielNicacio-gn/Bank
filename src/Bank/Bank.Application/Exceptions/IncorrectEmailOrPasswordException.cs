namespace Bank.Bank.Application.Exceptions;

public class IncorrectEmailOrPasswordException : Exception
{
    public IncorrectEmailOrPasswordException(string message):base(message) { }
}