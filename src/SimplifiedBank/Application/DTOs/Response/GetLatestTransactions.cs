
namespace SimplifiedBank.Application.DTOs.Response
{
    public record GetLatestTransactions(int idTransaction, int IdSender, int IdReceiver, decimal Value, DateTime Hour);
}