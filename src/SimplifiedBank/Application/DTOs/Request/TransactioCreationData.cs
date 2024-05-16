using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplifiedBank.Application.DTOs.Request;
public record TransactionCreationData(int IdSender, int IdReceiver, decimal Value);
