using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplifiedBank.Application.DTOs.Request;
public record TransactioCreationData(int IdSender, int IdReceiver, decimal Value);
