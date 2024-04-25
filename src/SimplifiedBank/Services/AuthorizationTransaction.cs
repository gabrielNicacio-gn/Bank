using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using SimplifiedBank.Application.UseCases;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Services.Repositories;

namespace SimplifiedBank.Services
{
    public class AuthorizationTransaction : IAuthorizationTransaction
    {
        private HttpClient http = new()
        {
            BaseAddress = new Uri("https://run.mocky.io/v3/5794d450-d2e2-4412-8131-73d0293ac1cc")
        };
        public async Task<bool> Authorization(Account account)
        {
            var response = await http.GetAsync(http.BaseAddress);
            string valueString = string.Empty;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var message = await response.Content.ReadAsStringAsync();
                var json = JsonObject.Parse(message);
                valueString = json["message"].ToString();
            }
            var IsAthorized = valueString.Equals("Autorizado");
            return IsAthorized;
        }
    }
}