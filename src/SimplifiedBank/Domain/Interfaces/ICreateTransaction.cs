using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataDTOs = SimplifiedBank.Application.DTOs;

namespace SimplifiedBank.Domain.Interface
{
    public interface ICreateTransaction
    {
        Task<DataDTOs::Response.TransactionCreationResponseData> Create(DataDTOs::Request.TransactioCreationData data);
    }
}