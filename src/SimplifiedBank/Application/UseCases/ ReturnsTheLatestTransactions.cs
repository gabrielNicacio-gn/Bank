
using SimplifiedBank.Application.DTOs.Response;
using SimplifiedBank.Domain.Interfaces;
using SimplifiedBank.Infrastructure.Repositories.TransactionRepositories;

namespace SimplifiedBank.Application.UseCases
{
    public class ReturnsTheLatestTransactions : IReturnsTheLatestTransactions
    {
        private readonly ITransactionRepository _transactionRepository;
        public ReturnsTheLatestTransactions(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task<IEnumerable<GetLatestTransactions>> GetList(int id)
        {
            var list = await _transactionRepository.GetLatestTransaction(id).ConfigureAwait(false);
            var returnList = list
            .Select(tr => new GetLatestTransactions(tr.Id, tr.IdSender, tr.IdReceiver, tr.Value, tr.HourOfTransaction))
            ?? new List<GetLatestTransactions>();
            return returnList;
        }
    }
}