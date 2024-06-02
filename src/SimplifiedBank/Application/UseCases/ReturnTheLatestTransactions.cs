using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplifiedBank.Application.DTOs.Response;
using SimplifiedBank.Domain.Interfaces;
using SimplifiedBank.Infrastructure.Repositories.TransactionRepositories;

namespace SimplifiedBank.Application.UseCases
{
    public class ReturnTheLatestTransactions : IReturnTheLatestTransactions
    {
        private readonly ITransactionRepository _transactionRepository;
        public ReturnTheLatestTransactions(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public IEnumerable<GetLatestTransactions> GetList(int id)
        {
            var list = _transactionRepository.GetLatestTransaction(id);
            var returnList = list
            .Select(tr => new GetLatestTransactions(tr.Id, tr.IdSender, tr.IdReceiver, tr.Value, tr.HourOfTransaction))
            ?? new List<GetLatestTransactions>();
            return returnList;
        }
    }
}