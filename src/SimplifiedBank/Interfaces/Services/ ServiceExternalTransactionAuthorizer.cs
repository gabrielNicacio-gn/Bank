using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using SimplifiedBank.Application.UseCases;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Interfaces.Exceptions;
using SimplifiedBank.Services.Interfaces;

namespace SimplifiedBank.Services
{
    public class ServiceExternalTransactionAuthorizer : IServiceExternalTransactionAuthorizer
    {
        private HttpClient http = new()
        {
            BaseAddress = new Uri("http://localhost:3000/transaction/validate")
        };
        public async ValueTask Authorizer()
        {
            var response = await http.GetAsync(http.BaseAddress);
            string valueString = string.Empty;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var message = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var json = JsonObject.Parse(message);
                valueString = json["message"].ToString();
            }
            var IsAthorized = valueString.Equals("Autorizado");

            if (!IsAthorized)
                throw new InvalidTransactionException("Transação Invalida");
        }
    }
}