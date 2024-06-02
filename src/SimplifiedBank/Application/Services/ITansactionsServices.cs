using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplifiedBank.Domain.Entities;
using DataDTOs = SimplifiedBank.Application.DTOs;

namespace SimplifiedBank.Application.Services;

public interface ITransactionsServices
{
    void ValidateTransaction(DataDTOs::Request.TransactionCreationData data);
}