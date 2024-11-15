using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Bank.Bank.Domain.Models
{
    public class User : IdentityUser<Guid>
    {
        public string Document { get; set; } = string.Empty;
        public Account? Account { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
        }
    }
}