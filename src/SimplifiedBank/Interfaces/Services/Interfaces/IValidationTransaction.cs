using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Services.Interfaces
{
    public interface IValidationTransaction
    {
        Task<bool> Validation();
    }
}