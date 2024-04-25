using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Services.Repositories
{
    public interface IAuthorizationTransaction
    {
        Task<bool> Authorization(Account account);
    }
}