using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Infrastructure.Data;

namespace SimplifiedBank.Infrastructure.Repositories.AccountsRepositories
{
    public class AccountRepository : IAccountRepositories
    {
        private readonly BankContext _bankContext;
        public AccountRepository(BankContext bankContext)
        {
            _bankContext = bankContext;
        }
        public async Task<Account?> GetAccount(int id)
        {
            var account = await _bankContext.Accounts
            .Where(ac => ac.Id.Equals(id))
            .SingleOrDefaultAsync();
            return account;
        }
    }
}